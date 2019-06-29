using UnityEngine;
using System.Collections;

public class showRenderQueue : MonoBehaviour {
    public int RQ = 3000;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RQ = GetComponent<Renderer>().material.renderQueue;
	}
    private void OnGUI()
    {
        GUILayout.Label(RQ.ToString());
        if (GUILayout.Button("  +  "))
        {
            RQ++;
            GetComponent<Renderer>().material.renderQueue = RQ;
        }
        if (GUILayout.Button("  -  "))
        {
            RQ--;
            GetComponent<Renderer>().material.renderQueue = RQ;
        }
    }
}
