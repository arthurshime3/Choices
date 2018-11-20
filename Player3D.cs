using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller3D))]
public class Player3D : MonoBehaviour {
	public GameObject dialogueCanvas;
    bool collidingWithSign, dead = false, collidingWithExit;

	public GameObject respawn;
	Controller3D controller;
	BoxCollider collider;

	public float moveSpeed = 2;
	public float sprintSpeed = 4;
	public float jumpHeight = 4;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float timeToJumpApex = 0.4f;

	//sound
	public AudioClip walkingSound;
	public AudioClip dashSound;
	public AudioClip impactSound;
	public AudioClip jumpSound;
	float stepTimer = 0f;
	float stepTime = 0.1f;
	bool impactSoundPlayed = false, fallSoundPlayed = false, jumpSoundPlayed = false;

	bool sprinting = false;
	bool jumping;

	bool wallSliding, climbable;
	public float wallSlideSpeedMax = 3;
	public Vector3 wallJumpClimb, wallJumpOff, wallLeap;
	public float wallStickTime = 0.25f, floorStickTime = 0.25f;
	float timeToWallUnstick, timeToFloorUnstick;

	public float dashSpeed = 50;
	public float dashTime = 0.2f, dashCoolDownTime = 0.2f, canDashTime = 0.5f;
	float dashTimeLeft, dashCoolDownTimeLeft, canDashTimeLeft;
	bool dashing = false, dashSoundPlayed = false;

	float gravity = -20;
	float jumpVelocity = 10;
	Vector3 velocity;
//	float horizontal;
	float velocityXSmoothing;
	bool hit = false;

	Vector3 defaultColliderSize, crouchColliderSize, defaultColliderCenter, crouchColliderCenter;
	bool crouching = false;

	//player stats
	bool playing;
	bool dialogueMode;
	float respawnTime = 1.5f, timeBeforeRespawn = 0f;
	DialogueTrigger currentTrigger;
//	float health = 100.0f;

	//Animator
	Animator animator;

	void Start () 
	{
		transform.position = respawn.GetComponent<CheckpointController> ().GetRespawnPos ();

		playing = true;
		controller = GetComponent<Controller3D> ();
		collider = GetComponent<BoxCollider> ();

		defaultColliderSize = collider.size;
		defaultColliderCenter = collider.center;
		crouchColliderSize = new Vector3 (defaultColliderSize.x, 5.7f, defaultColliderSize.z);
		crouchColliderCenter = new Vector3 (defaultColliderCenter.x, -0.72f, defaultColliderCenter.y);

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);

        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();

		animator = GetComponent<Animator> ();
		animator.SetBool ("Jumping", false);
		animator.SetBool ("Falling", false);
		animator.SetBool ("FaceRight", true);
		animator.SetBool ("Dashing", false);
		animator.SetBool ("FaceRight", true);
		animator.SetBool ("Alive", true);
		animator.SetFloat ("Speed", 0f);
 	}

	void Update () 
	{
		if (playing) {
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			int wallDirX = controller.collisions.left ? -1 : 1;

			//normal movement
			sprinting = false;

//			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
//				sprinting = true;

			float targetVelocityX = input.x * moveSpeed;

			if (sprinting)
				targetVelocityX = input.x * sprintSpeed;

			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

			//wall sliding
			wallSliding = false;
			animator.SetBool ("WallStick", false);
            if ((controller.collisions.left || controller.collisions.right) && climbable && !controller.collisions.below && velocity.y < 0) {
				wallSliding = true;

				animator.SetBool ("WallStick", true);

				if (velocity.y < -wallSlideSpeedMax)
					velocity.y = -wallSlideSpeedMax;

				if (timeToWallUnstick > 0) {
					velocity.x = 0;

					if ((int)input.x != wallDirX && (int)input.x != 0)
						timeToWallUnstick -= Time.deltaTime;
					else
						timeToWallUnstick = wallStickTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			}

			if (controller.collisions.above || controller.collisions.below)
				velocity.y = 0;

			if (controller.collisions.below) {
				if (timeToFloorUnstick > 0) {
					velocity.y = 0;

					if ((int)input.y != -1 && (int)input.y != 0)
						timeToFloorUnstick -= Time.deltaTime;
					else
						timeToFloorUnstick = floorStickTime;
				} else
					timeToFloorUnstick = floorStickTime;
			}

			//dashing
			//double tap key
//			if (((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && controller.collisions.faceDir == -1)
//				|| ((Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && controller.collisions.faceDir == 1)) {
//				if (!dashing && canDashTimeLeft <= 0) 
//				{
//					canDashTimeLeft = canDashTime;
//				} 
//				else if (!dashing && canDashTimeLeft > 0) {
//					if (dashCoolDownTimeLeft <= 0) {
//						dashing = true;
//						dashTimeLeft = dashTime;
//					}
//				}
//			}
            //shift dashing
			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
				if (!dashing) {
					if (dashCoolDownTimeLeft <= 0) {
						dashing = true;
						dashTimeLeft = dashTime;
                        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
					}
				}
			}
			if (dashing) {
				dashTimeLeft -= Time.deltaTime;

                velocity.x = dashSpeed * controller.collisions.faceDir;
                velocity.y = 2.0f;

                //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                //{
                //    velocity.y = velocity.x;
                //    if (velocity.y < 0)
                //        velocity.y = -velocity.y;

                //    //dash straight up
                //    //if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                //    //{
                //        velocity.x = 0;
                //    //}

                //    dashTimeLeft /= 2;
                //}

				if (dashTimeLeft <= 0) {
					dashing = false;
					dashCoolDownTimeLeft = dashCoolDownTime;
				}
			}
			if (dashCoolDownTimeLeft > 0)
            {
                dashCoolDownTimeLeft -= Time.deltaTime;
                if (velocity.y > 80)
                    velocity.y += 3 * gravity * Time.deltaTime; //apply gravity 3 times (total of 4 times at end of update)
            }
			if (canDashTimeLeft > 0)
				canDashTimeLeft -= Time.deltaTime;

            if ((controller.collisions.left || controller.collisions.right) && dashing) {
				dashing = false;
				dashCoolDownTimeLeft = dashCoolDownTime;
			}

            if (!dashing && transform.GetChild(1).gameObject.activeSelf)
            {
                transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
            }

			jumping = !controller.collisions.below && !wallSliding;
			if (!jumping)
				animator.SetBool ("Jumping", false);

			//jumping
			if ((Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) && !jumping && !dashing) {
				if (wallSliding) {
					if (wallDirX == (int)input.x) {
						velocity.x = -wallDirX * wallJumpClimb.x;
						velocity.y = wallJumpClimb.y;
					} else if ((int)input.x == 0) {
						velocity.x = -wallDirX * wallJumpOff.x;
						velocity.y = wallJumpOff.y;
					} else {
						velocity.x = -wallDirX * wallLeap.x;
						velocity.y = wallLeap.y;
					}
				}
				if (controller.collisions.below) {
					velocity.y = jumpVelocity;
				}
				animator.SetBool ("Jumping", true);
			}

			velocity.y += gravity * Time.deltaTime;

			//crouch
			if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow))
				crouching = false;
			if ((Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) && !dashing && controller.collisions.below && !wallSliding) {
				crouching = true;
			}

			if (!crouching) {
				animator.SetBool ("Crouching", false);
				collider.size = defaultColliderSize;
				collider.center = defaultColliderCenter;
			} else {
				animator.SetBool ("Crouching", true);
				velocity = Vector3.zero;
				collider.size = crouchColliderSize;
				collider.center = crouchColliderCenter;
			}

            if (currentTrigger != null)
            {
				if (Input.GetKey(KeyCode.E) && !currentTrigger.IsPlaying())
				{
					dialogueCanvas.SetActive(true);
					currentTrigger.TriggerDialogue();
				}
				if (currentTrigger.IsPlaying())
				{
					velocity.x = 0;
					if (velocity.y > 0) velocity.y = 0;
					if (Input.GetKeyDown(currentTrigger.dialogue.continueKey))
					{
						if (!currentTrigger.Next())
						{
							currentTrigger.EndDialogue();
						}
					}
				}   
            }

            if (collidingWithExit && Input.GetKeyDown(KeyCode.Return))
                GlobalController.instance.NextLevel();

			//Sound
			PlaySounds ();

			UpdateAnimation ();
			controller.Move (velocity * Time.deltaTime);

            //Debug.Log(velocity * Time.deltaTime);
		}
		else if (dead)
		{
			if (timeBeforeRespawn > 0)
				timeBeforeRespawn -= Time.deltaTime;
			else
				Respawn ();
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Damage"))
		{
			Death();
		}
		if (col.gameObject.CompareTag("Respawn"))
		{
			if (!col.gameObject.Equals(respawn))
			{
				respawn.GetComponent<CheckpointController>().SetActive(false);
				respawn = col.gameObject;
				respawn.GetComponent<CheckpointController>().SetActive(true);
			}
		}
		if (col.gameObject.CompareTag("Sign"))
		{
			if (currentTrigger == null)
			{
				collidingWithSign = true;
				currentTrigger = col.gameObject.GetComponent<DialogueTrigger>();
			}
		}
        if (col.gameObject.CompareTag("Exit"))
        {
            collidingWithExit = true;
            if (col.gameObject.GetComponent<ExitController>().automatic)
                GlobalController.instance.NextLevel();
        }
        if (col.gameObject.CompareTag("Climbable"))
            climbable = true;
	}

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Sign"))
        {
            currentTrigger = null;
        }
		if (col.gameObject.CompareTag("Exit"))
		{
            collidingWithExit = false;
		}
        if (col.gameObject.CompareTag("Climbable"))
            climbable = false;
    }

	void PlaySounds()
	{
//		if (Mathf.Abs (velocity.x) > 1 && controller.collisions.below && !dashing) 
//		{
//			if (stepTimer > 0)
//				stepTimer -= Time.deltaTime;
//			else 
//			{
//				GetComponent<AudioSource> ().PlayOneShot (walkingSound);
//				stepTimer = stepTime;
//			}
//		}
//		else if (GetComponent<AudioSource> ().isPlaying)
//			GetComponent<AudioSource> ().Stop ();
		if (dashing && !dashSoundPlayed) 
		{
			GetComponent<AudioSource> ().PlayOneShot (dashSound);
			dashSoundPlayed = true;
		}
		if (!dashing && dashSoundPlayed)
			dashSoundPlayed = false;
			
		if (wallSliding && !impactSoundPlayed) 
		{
			GetComponent<AudioSource> ().PlayOneShot (impactSound);
			impactSoundPlayed = true;
		}
		if (!wallSliding && impactSoundPlayed)
			impactSoundPlayed = false;

		if (!jumping && !fallSoundPlayed) 
		{
			GetComponent<AudioSource> ().PlayOneShot (impactSound);
			fallSoundPlayed = true;
		}
		if (jumping && fallSoundPlayed)
			fallSoundPlayed = false;

//		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && !jumpSoundPlayed && jumping) 
//		{
//			GetComponent<AudioSource> ().PlayOneShot (jumpSound);
//			jumpSoundPlayed = true;
//		}
//		if (!jumping && jumpSoundPlayed)
//			jumpSoundPlayed = false;
	}

	void UpdateAnimation()
	{
		animator.SetFloat ("Speed", velocity.x);
		animator.SetBool ("Dashing", dashing);
		animator.SetBool ("Falling", !controller.collisions.below && velocity.y < 0);
		animator.SetBool ("FaceRight", controller.collisions.faceDir == 1);
	}

	void Death()
	{
		playing = false;
		ResetMovement ();
		animator.SetBool ("Alive", false);
		timeBeforeRespawn = respawnTime;
        dead = true;
	}

	void Respawn()
	{
		transform.position = respawn.GetComponent<CheckpointController> ().GetRespawnPos ();
		playing = true;
		animator.SetBool ("Alive", true);
        dead = false;
	}

	void ResetMovement()
	{
		velocity = Vector3.zero;
		dashing = false;
		jumping = false;
		crouching = false;
	}

    public bool IsDashing()
    {
        return dashing;    
    }

    public void Pause()
    {
        playing = false;
    }

    public void Play()
    {
        playing = true;
    }

}