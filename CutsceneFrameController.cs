using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneFrameController : MonoBehaviour {
	public string transitionKey = "Enter";	//key required to transition to this scene
	public int numOfChoices = 1;
	public int choiceNum = 0;
	bool fadingIn, fadingOut;
	Color originalColor, fadeColor;
	float fadeTime = 1f;

    public bool lastCutscene = false;

	// Use this for initialization
	void Start () {
		fadingIn = false;
		fadingOut = false;
		fadeColor = Color.black;
		fadeColor.a = 0;
        //originalColor = GetComponent<SpriteRenderer> ().material.color;
        originalColor = new Color(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingIn) 
		{
			GetComponent<SpriteRenderer> ().material.color = Color.Lerp (GetComponent<SpriteRenderer> ().material.color, originalColor, fadeTime * Time.deltaTime);
			if (GetComponent<SpriteRenderer> ().material.color.a > 0.995) 
			{
				fadingIn = false;
			}
            //print(GetComponent<SpriteRenderer>().material.color.a);
		}
		if (fadingOut) 
		{
			GetComponent<SpriteRenderer> ().material.color = Color.Lerp (GetComponent<SpriteRenderer> ().material.color, fadeColor, fadeTime * Time.deltaTime);
			if (GetComponent<SpriteRenderer> ().material.color.a < 0.05) 
			{
				fadingOut = false;
			}
		}
	}

	public void FadeIn()
	{
		fadingIn = true;
		fadingOut = false;
	}

	public void FadeOut()
	{
		fadingOut = true;
		fadingIn = false;
	}

	public void Clear()
	{
		GetComponent<SpriteRenderer> ().material.color = fadeColor;
	}

	public void SetFadeTime(float f)
	{
		fadeTime = f;
	}
}
