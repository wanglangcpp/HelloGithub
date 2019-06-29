using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Genesis.GameClient
{
    public class DelaySlash : MonoBehaviour
    {


        public MeshRenderer _m;

        public float _ColdTime = 3;//扫描间隔
        public float _speed = 1;
        private float _NewTime;

        private bool _CanChange;


        private float _v = -1;



        void Start()
        {

            _NewTime = _ColdTime;
            _m.material.SetFloat("_U", 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (_NewTime > 0)
            {
                _NewTime -= Time.deltaTime;
            }
            else
            {
                _CanChange = true;


            }

            if (_CanChange)
            {


                _v += 1.5f * Time.deltaTime;
                _m.material.SetFloat("_U", _v * _speed);
                if (_v >= 1)
                {
                    _v = -1;
                    _m.material.SetFloat("_U", _v * _speed);

                    _CanChange = false;
                    _NewTime = _ColdTime;
                }

            }

        }
    }

}