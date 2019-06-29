using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public static class AnimationUtility
    {
        public static IEnumerator PlayAnimations(List<Animation> animationList, GameFrameworkAction<object> onComplete, object userData = null)
        {
            for (int i = 0; i < animationList.Count; i++)
            {
                animationList[i].Play(animationList[i].clip.name);
            }

            bool isPlaying = true;
            float timeAtLastFrame = 0F;
            timeAtLastFrame = Time.realtimeSinceStartup;
            float progressTime = 0F;
            float timeAtCurrentFrame = 0F;
            float deltaTime = 0F;
            while (isPlaying)
            {
                timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = timeAtCurrentFrame - timeAtLastFrame;
                timeAtLastFrame = timeAtCurrentFrame;
                if (animationList.Count == 0)
                {
                    isPlaying = false;
                    break;
                }

                for (int i = 0; i < animationList.Count;)
                {
                    AnimationState currState = animationList[i][animationList[i].clip.name];
                    progressTime += deltaTime;
                    if (currState.speed > 0)
                    {
                        currState.normalizedTime = progressTime / currState.length;
                    }
                    else
                    {
                        currState.normalizedTime = (currState.length - progressTime) / currState.length;
                    }
                    animationList[i].Sample();
                    if (progressTime >= currState.length)
                    {
                        if (currState.wrapMode != WrapMode.Loop)
                        {
                            animationList.Remove(animationList[i]);

                            continue;
                        }
                        else
                        {
                            i++;
                            progressTime = 0.0f;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                yield return new WaitForEndOfFrame();
            }

            yield return null;

            if (onComplete != null)
            {
                onComplete(userData);
            }
        }

        public static IEnumerator PlayAnimation(Animation animation, GameFrameworkAction<object> onComplete, object userData = null)
        {
            return PlayAnimations(new List<Animation>() { animation }, onComplete, userData);
        }
    }
}
