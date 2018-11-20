using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Player"))
			transform.GetChild (0).gameObject.SetActive (true);
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.CompareTag ("Player"))
			transform.GetChild (0).gameObject.SetActive (false);
	}
}
