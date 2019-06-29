using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ShowAtlasAndDepth : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
    bool destory = false;
	// Update is called once per frame
	void Update () {
        if (destory==false)
        {
            destory = true;
            return;
        }
        DestroyImmediate(this);
	}

}
