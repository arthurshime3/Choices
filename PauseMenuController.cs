using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {
    bool isOpen;
	// Use this for initialization
	void Start () {
        isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
            {
				GameObject.Find("Player").GetComponent<Player3D>().Pause();
				isOpen = true;
            }
            else
            {
				GameObject.Find("Player").GetComponent<Player3D>().Play();
                isOpen = false;
            }
            GetComponentInChildren<Animator>().SetBool("isOpen", isOpen);
        }
	}
}
