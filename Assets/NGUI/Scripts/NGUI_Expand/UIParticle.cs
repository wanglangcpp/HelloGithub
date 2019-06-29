using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 需要增加特效排序功能及特效播放
/// </summary>
public class UIParticle : UIWidget
{
    public List<Renderer> Particles = new List<Renderer>();
    public int renderQueue = 3000;
    public Animator animator = null;
    public AnimationClip defaultClip = null;
    public bool playOnAwake = false;
    public float duration = -1f;
    public int extraRQ = 0;
    protected bool isPlaying = false;
    protected float endTime = 0f;
    protected bool isInited = false;
    public bool IsPlaying { get { return isPlaying; } }
    [ContextMenu("Refresh Particles")]
    public void RefreshParticles()
    {
        Particles.Clear();
        Particles.AddRange(transform.GetComponentsInChildren<Renderer>(true));
        if (Particles.Count > 0)
            extraRQ = Particles.Count - 1;
        //renderQueue = 3000;
        //SetRQ(renderQueue);
        if (animator == null)
        {
            animator = transform.GetComponent<Animator>();
            if (animator != null)
            {
                if (animator && animator.runtimeAnimatorController.animationClips.Length > 0)
                {
                    defaultClip = animator.runtimeAnimatorController.animationClips[0];
                }
                animator.enabled = false;
            }
        }
    }

    bool ExistParticle()
    {
        return Particles != null && Particles.Count > 0;
    }

    #region Play and Stop
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        endTime = 0f;
        for (int i = 0; i < Particles.Count; i++)
        {
            if (Particles[i] == null)
            {
                continue;
            }
            ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Stop(true);
            }
            Particles[i].gameObject.SetActive(false);
            if (animator && defaultClip)
            {
                animator.Stop();
                animator.enabled = false;
            }
        }
        isPlaying = false;
    }
    public void Play()
    {
        if (gameObject.activeInHierarchy == false)
            return;
        if (!isPlaying)
            StartCoroutine(PlayEnd());
        else
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                if (Particles[i] == null)
                {
                    continue;
                }
                Particles[i].gameObject.SetActive(true);
                ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
                if (particle != null)
                {
                    particle.Stop(true);
                    particle.Play(true);
                }
            }
            if (animator && defaultClip)
            {
                animator.Stop();
                animator.enabled = false;
                animator.enabled = true;
                animator.Play(defaultClip.name, -1, 0f);
            }
            endTime = Time.realtimeSinceStartup + duration;
        }
    }
    public void Play(float time)
    {
        if (duration == -1f && time != duration)
            Stop();
        duration = time;
        Play();
    }
    public void Stop()
    {
        duration = 1f;
        endTime = 0f;
        for (int i = 0; i < Particles.Count; i++)
        {
            if (Particles[i] == null)
            {
                continue;
            }
            ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Stop(true);
            }
            Particles[i].gameObject.SetActive(false);
        }
        if (animator && defaultClip)
        {
            animator.Stop();
            animator.enabled = false;
        }
        StopAllCoroutines();
        isPlaying = false;
    }
    IEnumerator PlayEnd()
    {
        isPlaying = true;
        while (isInited == false)
            yield return false;
        for (int i = 0; i < Particles.Count; i++)
        {
            if (Particles[i] == null)
            {
                continue;
            }
            Particles[i].gameObject.SetActive(true);
            ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Stop(true);
                particle.Play(true);
            }
        }
        if (animator && defaultClip)
        {
            animator.enabled = false;
            animator.enabled = true;
            animator.Play(defaultClip.name, -1, 0f);
        }
        //-1永久播放，除非调用stop
        if (duration <= -1f)
            yield break;
        endTime = Time.realtimeSinceStartup + duration;
        while (Time.realtimeSinceStartup <= endTime)
        {
            yield return null;
        }
        endTime = 0f;
        for (int i = 0; i < Particles.Count; i++)
        {
            if (Particles[i] == null)
            {
                continue;
            }
            ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Stop(true);
            }
            Particles[i].gameObject.SetActive(false);
        }
        if (animator && defaultClip)
        {
            animator.Stop();
            animator.enabled = false;
        }
        isPlaying = false;
    }
    #endregion

    #region Set RQ
    /// <summary>
    /// Texture used by the widget.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            Material mat = material;
            return mat ? mat.mainTexture : null;
        }
        set
        {
            throw new System.NotImplementedException(GetType() + " has no mainTexture setter");
        }
    }
    public override Material material
    {
        get; set;
    }


    public override void SetRect(float x, float y, float width, float height)
    {

    }

    protected override void OnInit()
    {
        isInited = true;
        if (Particles == null || Particles.Count <= 0)
            RefreshParticles();
        if (Particles == null || Particles.Count == 0)
            return;
        for (int i = 0; i < Particles.Count; i++)
        {
            ParticleSystem particle = Particles[i].transform.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Stop(true);
            }
            Particles[i].gameObject.SetActive(false);
        }
        if (animator && defaultClip)
            animator.Stop();
        onRender = OnRender;
            onUpdateRQ = OnUpdateRQ;
        if (Application.isPlaying)
        {
            material = Particles[0].GetComponent<Renderer>().material;
        }
        else
            material = Particles[0].GetComponent<Renderer>().sharedMaterial;
        if (material == null)
            return;
        renderQueue = material.renderQueue;
        if (playOnAwake)
        {
            Play(duration);
        }

    }
    int refreshCount = 0;
    void OnUpdateRQ(int rq)
    {
        if (refreshCount<=2 || renderQueue != rq)
        {
            if (renderQueue!=rq)
            {
                refreshCount = 0;
            }
            renderQueue = rq;
            SetRQ(rq);
            refreshCount++;
        }
    }
    private void OnRender(Material mat)
    {
        //return;
        if (renderQueue != mat.renderQueue)
        {
            renderQueue = mat.renderQueue;
            SetRQ(mat.renderQueue);
        }
        if (drawCall.isActive)
        {
            drawCall.GetComponent<Renderer>().enabled = false;
            drawCall.isActive = false;
#if !UNITY_EDITOR
            //提前回收,省内存
            drawCall.gameObject.SetActive(false);
#endif
        }
    }

    override public void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
    {
        Texture tex = mainTexture;
        if (tex == null) return;
        verts.Add(Vector3.zero);
        verts.Add(Vector3.zero);
        verts.Add(Vector3.zero);
        verts.Add(Vector3.zero);

        uvs.Add(new Vector2(0f, 0f));
        uvs.Add(new Vector2(0f, 1f));
        uvs.Add(new Vector2(1f, 1f));
        uvs.Add(new Vector2(1f, 0f));
        cols.Add(new Color(1f, 1f, 1f, 1f));
        cols.Add(new Color(1f, 1f, 1f, 1f));
        cols.Add(new Color(1f, 1f, 1f, 1f));
        cols.Add(new Color(1f, 1f, 1f, 1f));
    }

    public new bool UpdateGeometry(int frame)
    {
        return true;
    }
    public void ResetRQ()
    {
        SetRQ(renderQueue);
    }
    void SetRQ(int renderQueue)
    {
        //Debug.Log("--------" + name + "   " + renderQueue);
        if (gameObject.scene == null)
            return;
        Renderer lastRender = null;
        if (Particles != null)
        {
            for (int i = 0; i < Particles.Count; ++i)
            {
                Renderer ren = Particles[i];

                if (ren)
                {
#if UNITY_EDITOR
                    
                    if (Application.isPlaying)
                    {
                        if (ren.material)
                        {
                            if (lastRender != null && lastRender.sharedMaterial == ren.sharedMaterial)
                            {
                                continue;
                            }
                            if (Application.isPlaying == true)
                                ren.material.renderQueue = renderQueue + i;
                            else
                                ren.material.renderQueue = renderQueue + i;
                            lastRender = ren;
                        }
                    }
                    else
                    {
                        //if (ren.sharedMaterial)
                        //    if (Application.isPlaying == true)
                        //        ren.sharedMaterial.renderQueue = renderQueue + i;
                        //    else
                        //        ren.sharedMaterial.renderQueue = renderQueue + i;
                    }


#else
                    if(ren.material)
                         ren.material.renderQueue = renderQueue + i;
#endif
                    ren.sortingOrder = 0;
                    ren.sortingLayerName = "Default";
                }
            }
        }
        //if (Application.isPlaying)
        //{
        //    string str = "  ";
        //    foreach (var item in Particles)
        //    {
        //        str += item.name + "/" + item.material.renderQueue;
        //    }
        //    if (name == "effect_ui_loading_02_1")
        //        Debug.LogError(renderQueue.ToString() + "  " + gameObject.scene + str);
        //}
    }
    #endregion
}
