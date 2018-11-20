using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {
	Animator animator;
	bool soundPlayed = false;
    public GameObject checkpointLight;
    public bool spawn;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
        if (spawn)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
            transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetActive(bool active)
	{
		animator.SetBool ("Active", active);
		if (active && !soundPlayed) {
			GetComponent<AudioSource> ().Play ();
			soundPlayed = true;
		} else if (!active && soundPlayed)
			soundPlayed = false;

        if (active && !spawn)
        {
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        }
        else if (!active && !spawn)
        {
			transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
			transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }
        if (!spawn)
            checkpointLight.GetComponent<LightUp>().LightUpLight();
	}

	public Vector3 GetRespawnPos()
	{
		return new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
	}
}
