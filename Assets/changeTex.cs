using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTex : MonoBehaviour {

    private Texture[] noise;
    public Texture[] noiseDiagonal;
    public Texture[] noiseRed;
    public Texture[] noiseBlue;
    public Texture[] noiseRedBlue;
    public Texture[] noiseLila;
    public Texture[] noiseColors;
    public Texture[] noiseDarkBlue;
    public Texture[] noiseLightLila;
    public Texture[] noiseDiagonalGreen;
    public Texture[] noiseDiagonalBlue;

    private int currentNoise = 0;
	private int maxNoise = 0;

    private float saved = 0f;

    public bool isChanging = true;

	// Use this for initialization
	void Start () {
        noise = noiseDiagonal;
		maxNoise = noise.Length;
	}

    public void changeTextures(int package)
    {
        switch (package)
        {
            case 0:
                noise = noiseDiagonal;
                break;
            case 1:
                noise = noiseRedBlue;
                break;
            case 3:
                noise = noiseBlue;
                break;
            case 2:
                noise = noiseRed;
                break;
            case 4:
                noise = noiseLila;
                break;
            case 7:
                noise = noiseColors;
                break;
            case 5:
                noise = noiseDarkBlue;
                break;
            case 6:
                noise = noiseLightLila;
                break;
            case 8:
                noise = noiseDiagonalBlue;
                break;
            case 9:
                noise = noiseDiagonalGreen;
                break;
            default:
                noise = noiseDiagonal;
                break;
        }
        maxNoise = noise.Length;
        currentNoise = 0;
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
