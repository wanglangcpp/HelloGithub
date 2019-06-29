using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Projector))]
    public class PlayerShaowHigh : MonoBehaviour
    {
        Transform meHeroCharacter = null;
        Projector projector = null;
        public float yOffset = 0f;
        int propertyId = 0;
        Material sceneMaterial = null;
        void Start()
        {
            GameEntry.Event.Subscribe(EventId.SwitchHeroComplete, OnSwitchHeroComplete);
            projector = GetComponent<Projector>();
            propertyId = Shader.PropertyToID("_High");
            if (GameEntry.SceneLogic.BaseInstanceLogic != null && GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter != null)
            {
                meHeroCharacter = GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter.transform;
            }
            if (sceneMaterial == null && projector != null && projector.material != null)
            {
                sceneMaterial = new Material(projector.material);
                projector.material = sceneMaterial;
            }
        }

        private void OnSwitchHeroComplete(object sender, GameEventArgs e)
        {
            meHeroCharacter = null;
        }

        void Update()
        {
            if (meHeroCharacter == null && GameEntry.SceneLogic.BaseInstanceLogic != null && GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter != null)
            {
                meHeroCharacter = GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter.transform;
            }
            if (meHeroCharacter == null)
            {
                return;
            }
            if (projector == null)
            {
                projector = GetComponent<Projector>();
            }
            if (projector.material == null)
            {
                return;
            }
            if (sceneMaterial == null && projector != null && projector.material != null)
            {
                sceneMaterial = new Material(projector.material);
                projector.material = sceneMaterial;
            }
            if (sceneMaterial == null)
            {
                return;
            }
            sceneMaterial.SetFloat(propertyId, meHeroCharacter.position.y + yOffset);

        }
    }

}