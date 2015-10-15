using UnityEngine;
using System.Collections;

[RequireComponent (typeof(playerController))]

public class playerInput : MonoBehaviour {
	//Movement
	float moveSpeed = 11f; //Movement force
	Vector2 velocity;     //Player velocity

	public static Vector2 input;

	//Jumping
	float gravity = -50f; //Gravity force
	float jumpVelocity = 12f;

	public float maxJumpHeight = 4f;
	public float minJumpHeight = 0.000001f;
	public float maxJumpvelocity;
	public float minJumpVelocity;
	public float timeToJumpApex = 0.6f;

	float accelerationTimeAirborn = 0.1f;
	float accelerationTimeGrounded = 0.08f;
	float velocityXSmoothing;

	//Wall jumping
	//float wallStickTime = 2f;
	public float wallSlidingSpeedMax = 5f;
	public Vector2 wallJumpTowards;
	public Vector2 wallJumpAway;
	public Vector2 wallJumpNeutral;
	
	playerController controller;

	void Start() {
		controller = GetComponent<playerController> ();
		wallJumpTowards = new Vector2 (12f, 20f);
		wallJumpAway = new Vector2(18f, 17f);
		wallJumpNeutral = new Vector2(8.5f, 7f);

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);

		maxJumpvelocity = Mathf.Abs (gravity) * timeToJumpApex;
		//minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		//maxJumpvelocity = 16f;
		minJumpVelocity = 0.1f;
	}

	void Update() {
		//Get raw direction - Keyboard returns 1, 0, or -1 each axis
		input = new Vector2 ((Input.GetKey (KeyCode.A) ? -1 : 0) 
										+ (Input.GetKey (KeyCode.D) ? 1 : 0),
		                             	(Input.GetKey (KeyCode.W) ? 1 : 0) 
										+ (Input.GetKey (KeyCode.S) ? -1 : 0));

		bool wallSliding = false;
		//Check if colliding into wall from left side or right
		//-1 is left, 1 is right
		int wallDirection = controller.collisions.left ? -1 : 1;

		//Wall sliding check
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.down) {
			wallSliding = true;

			//Lock max wall sliding speed
			if (velocity.y < -wallSlidingSpeedMax) {
				velocity.y = -wallSlidingSpeedMax;
			}
		}

		if ((controller.collisions.up) || (controller.collisions.down)) {
			velocity.y = 0f;
		}

		//jump
		if (Input.GetKeyDown (KeyCode.J)) {
			//Jumping off of a wall
			if (wallSliding) {
				//Jump towards the wall
				if (wallDirection == input.x) {
					velocity.x = -wallDirection * wallJumpTowards.x;
					velocity.y = wallJumpTowards.y;
				//Jump away from the wall
				} else if (wallDirection == -input.x) {
					velocity.x = -wallDirection * wallJumpAway.x;
					velocity.y = wallJumpAway.y;
				//Jump without direction
				} else if (input.y == -1) {
					velocity.x = -wallDirection * wallJumpNeutral.x;
					velocity.y = wallJumpNeutral.y;
				}
			}
			//Jumping off of the ground
			if (controller.collisions.down) {
				velocity.y = maxJumpvelocity;
			}
		}
		/*if (Mathf.Abs (input.x) == 1 && wallSliding) {
			if(Input.GetKeyDown(KeyCode.J)) {
				//Jump towards the wall
				if (wallDirection == input.x) {
					velocity.x = -wallDirection * wallJumpTowards.x;
					velocity.y = wallJumpTowards.y;
					//Jump away from the wall
				} else if (wallDirection == -input.x) {
					velocity.x = -wallDirection * wallJumpAway.x;
					velocity.y = wallJumpAway.y;
					//Jump without direction
				} else if (input.y == -1) {
					velocity.x = -wallDirection * wallJumpNeutral.x;
					velocity.y = wallJumpNeutral.y;
				}
			}
		}*/

		if (Input.GetKeyUp (KeyCode.J)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}


		//While sticking to wall, disable x-axis until jump is pressed
		if (!wallSliding) {
			//direction into velocity
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		                               controller.collisions.down?accelerationTimeGrounded:accelerationTimeAirborn);
		}
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, false);
	}
}
