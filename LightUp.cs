using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour {
    Light light;
	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	public void LightUpLight()
    {
        light.intensity = 2;
    }

     
}
