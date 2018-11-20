using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockController : RaycastController {
	public float fallTime = 3.0f, resetTime = 5.0f, maxFallSpeed = 10.0f;
	bool falling, fell;
	float gravity = -1;
	float timer;
	Vector3 velocity, initialPos;

	// Use this for initialization
	void Start () {
		falling = false;
		timer = 0;
		velocity = Vector3.zero;
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (falling) 
		{
			if (velocity.y > -maxFallSpeed)
				velocity.y += gravity * Time.deltaTime;
			timer -= Time.deltaTime;

			if (timer <= 0) 
			{
				velocity = Vector3.zero;
				fell = true;
				falling = false;
				timer = resetTime;
			}
		}

		if (fell) 
		{
			timer -= Time.deltaTime;
			if (timer <= 0) 
			{
				fell = false;
				Reset ();
			}
		}

		transform.Translate (velocity);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.CompareTag ("Player")) 
		{
			falling = true;
			timer = fallTime;
		}
	}

	void Reset()
	{
		falling = false;
		fell = false;
		transform.position = initialPos;
	}
}
