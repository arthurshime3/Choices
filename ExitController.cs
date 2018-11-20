using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
    public bool automatic = true;

	// Use this for initialization
	void Start () {
        transform.GetChild(1).gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.CompareTag("Player"))
			transform.GetChild(1).gameObject.SetActive(false);
    }
}
