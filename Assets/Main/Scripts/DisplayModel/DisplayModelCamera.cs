using UnityEngine;
using System.Collections;


namespace Genesis.GameClient
{
    public class DisplayModelCamera : MonoBehaviour
    {
        //[SerializeField]
        public Transform platformRoot;
        public Camera modelCamera;
        public CameraViewportController viewportController;
        private void Awake()
        {
            modelCamera = GetComponent<Camera>();
            viewportController = GetComponent<CameraViewportController>();
            GameEntry.DisplayModel.SetModelCamera(this);
            
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}