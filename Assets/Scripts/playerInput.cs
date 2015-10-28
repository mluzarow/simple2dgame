using UnityEngine;
using System.Collections;

[RequireComponent (typeof(playerController))]
[RequireComponent (typeof(playerActions))]

public class playerInput : MonoBehaviour {
	//Movement
	float moveSpeed = 11f; //Movement force
	Vector2 velocity;     //Player velocity

	//Flag for switching between player inputs impacting standard platforming movement and textbox interaction
	public static bool triggerTextboxMovement;
	public static Vector2 input;

	bool drawgizmos = false;

	//Jumping
	float gravity = -50f; //Gravity force
	//float jumpVelocity = 12f;

	//Jump physics variables
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

	playerActions actions;
	playerController controller;

	public bool dashing = false;
	public bool dashTiming = false;
	

	void Start() {
		//Flag is false, default to regular platforming movement
		triggerTextboxMovement = false;

		controller = GetComponent<playerController> ();
		actions = GetComponent<playerActions> ();

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
		input = new Vector2 ((Input.GetKey (menuMain.keys["LEFT"]) ? -1 : 0) + (Input.GetKey (menuMain.keys["RIGHT"]) ? 1 : 0),
		                     (Input.GetKey (menuMain.keys["UP"]) ? 1 : 0) + (Input.GetKey (menuMain.keys["DOWN"]) ? -1 : 0));

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

		//Freeze player movement while textbox is active
		if (!triggerTextboxMovement) {
			//jump
			if (Input.GetKeyDown (menuMain.keys["JUMP"])) {
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
					} else {
						velocity.x = -wallDirection * wallJumpAway.x;
						velocity.y = wallJumpAway.y;
					}
				}
				//Jumping off of the ground
				if (controller.collisions.down) {
					velocity.y = maxJumpvelocity;
				}
			}

			if (Input.GetMouseButton (0)) {
				drawgizmos = true;
			} else {
				drawgizmos = false;
			}

			//Jumping; waiting on key up to lock upward jump velocity
			if (Input.GetKeyUp (menuMain.keys["JUMP"])) {
				if (velocity.y > minJumpVelocity) {
					velocity.y = minJumpVelocity;
				}
			}

			//Not wall sliding
			if (!wallSliding) {
				if (Input.GetKeyDown (menuMain.keys["ACTION"])) {
					//actions.heal();

					if (velocity.x != 0f && !dashing) {
						print ("dashed");
						velocity.x *= 10;
						dashing = true;
						StartCoroutine(dashingTimer ());
					}
				}

				//direction into velocity
				float targetVelocityX = input.x * moveSpeed;
				velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		                               controller.collisions.down?accelerationTimeGrounded:accelerationTimeAirborn);
			}
		} else {
			velocity = new Vector2(0f, 0f);
		}



		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, false);
	}

	IEnumerator dashingTimer() {
		print ("Timing dash");
		//dashTiming = true;
		yield return new WaitForSeconds (1f);
		dashing = false;
		//dashTiming = false;
		print ("finished timing");

		yield return 0;
	}

	void OnDrawGizmos() {
		if (drawgizmos) {
			Vector2 mouse = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 player = (Vector2) transform.position;
			float radius = 10f;
			float diffy = Mathf.Abs (mouse.y) - Mathf.Abs (player.y);
			float diffx = Mathf.Abs (mouse.x) - Mathf.Abs (player.x);
			float angle = Mathf.Tan (diffy / diffx);
			float x = Mathf.Sin (angle) * radius;
			float y = Mathf.Cos (angle) * radius;


			if ((Mathf.Abs((mouse - player).x) <= 10f) && (Mathf.Abs ((mouse - player).y) <= 10f)) {
				Gizmos.DrawLine (player, mouse);
			} else {
				if (Mathf.Abs((mouse - player).x) > 10f) {
					mouse.x = (player.x + x);
				}
				if (Mathf.Abs((mouse - player).y) > 10f) {
					mouse.y = (player.y + y);
				}
				Gizmos.DrawLine (player, mouse);
			}

			//Gizmos.DrawLine (Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(1f, 0f, 0f), Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(-1f,0f,0f)));
			//Gizmos.DrawLine (Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 1f, 0f), Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0f,-1f,0f)));
		}
	}
}
