using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player2D : MonoBehaviour {
	GameObject rock;
	Controller2D controller;

	float moveSpeed = 6;
	public float jumpHeight = 4;
	public float timeToJumpApex = 0.4f;

	float gravity = -20;
	float jumpVelocity = 10;
	Vector2 velocity;
	bool faceRight;
	float horizontal;

	void Start () 
	{
		rock = (GameObject)Resources.Load ("Rock") as GameObject;
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
 	}

	void Update () 
	{
		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		horizontal = Input.GetAxisRaw ("Horizontal");
		if (horizontal != 0)
			faceRight = horizontal > 0;
		Vector2 input = new Vector2 (horizontal, Input.GetAxisRaw ("Vertical"));

		if ((Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && controller.collisions.below)
			velocity.y = jumpVelocity;
		if (Input.GetKeyDown (KeyCode.F))
			ThrowRock ();
		
		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void ThrowRock()
	{
		float xShift = 0.3f;
		float yShift = 0.3f;

//		GameObject newRock; 
//
//		if (faceRight) 
//		{
//			newRock = Instantiate (rock, new Vector3 (transform.position.x + xShift, transform.position.y + yShift, transform.position.z), transform.rotation) as GameObject;
//			rc = (RockController)newRock.GetComponent (typeof(RockController));
//			rc.Release (new Vector3 (400, 250, 0));
//		} else {
//			newRock = Instantiate (rock, new Vector3(transform.position.x - xShift, transform.position.y + yShift, transform.position.z), transform.rotation) as GameObject;
//			rc = (RockController)newRock.GetComponent (typeof(RockController));
//			rc.Release (new Vector3 (-400, 250, 0));
//		}
	}
}
