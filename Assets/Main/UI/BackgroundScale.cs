using UnityEngine;
using System.Collections;

public class BackgroundScale : MonoBehaviour {

    // Use this for initialization
    void Start () {

        UIRoot uiRoot_ = UIRoot.list[0];
        if (uiRoot_.scalingStyle != UIRoot.Scaling.Constrained)
            return;
        float scale = 1;
        if (uiRoot_.fitHeight)
        {
            scale = Screen.height / (float)uiRoot_.manualHeight;
        }
        else if (uiRoot_.fitWidth)
        {
            scale = Screen.width / (float)uiRoot_.manualWidth;
        }
        transform.localScale = new Vector3(scale, scale, 1);
        BoxCollider col = gameObject.GetComponent<BoxCollider>();
        if (null != col)
        {
            col.size = new Vector3(col.size.x * scale, col.size.y * scale, col.size.z);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
