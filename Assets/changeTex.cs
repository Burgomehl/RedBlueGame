using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTex : MonoBehaviour {

    public Texture noiseOne;
    public Texture noiseTwo;

    private float saved = 0f;
    private bool currentTex = false;

    public bool isChanging = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isChanging)
        {
            enabled = false;
        }
		if(saved + 0.1f < Time.fixedTime)
        {
            saved = Time.fixedTime;
            if (currentTex)
            {
                GetComponent<Renderer>().material.SetTexture("_MainTex",noiseOne);
            }
            else
            {
                GetComponent<Renderer>().material.SetTexture("_MainTex", noiseTwo);
            }
            currentTex = !currentTex;
        }
	}
}
