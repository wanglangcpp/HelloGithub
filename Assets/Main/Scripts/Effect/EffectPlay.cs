using UnityEngine;
using System.Collections;
// =============Sawyer Script==============//
namespace Genesis.GameClient
{
    public class EffectPlay : MonoBehaviour
    {
        public Transform[] Effect;
        private int count = 0;
        public float time = 1;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < Effect.Length; i++)
                {
                    if (i == count)
                        Effect[i].gameObject.SetActive(true);
                    else
                        Effect[i].gameObject.SetActive(false);
                }
                count++;
                if (count > Effect.Length - 1)
                    count = 0;

            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Effect[0].gameObject.activeSelf)
                    Time.timeScale = time;
                Effect[count].gameObject.SetActive(false);
                Effect[count].gameObject.SetActive(true);

            }

        }
    }
}