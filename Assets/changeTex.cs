using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTex : MonoBehaviour {

    public Texture[] noise;
	private int currentNoise = 0;
	private int maxNoise = 0;

    private float saved = 0f;

    public bool isChanging = true;

	// Use this for initialization
	void Start () {
		maxNoise = noise.Length;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isChanging)
        {
            enabled = false;
        }
		if(saved + 0.01f < Time.fixedTime)
        {
            saved = Time.fixedTime;

			GetComponent<Renderer>().material.SetTexture("_MainTex",noise[currentNoise]);
			currentNoise = (currentNoise + 1) % maxNoise;
        }
	}
}
