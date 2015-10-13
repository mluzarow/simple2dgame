using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Allows creation of hash set

public class platformController : raycastController {

	public Vector2 move;
	public LayerMask passengerMask;

	public override void Start () {
		base.Start ();
	}

	void Update () {

		updateRaycasts ();

		Vector2 velocity = move * Time.deltaTime;
		movePassenger (velocity);
		transform.Translate (velocity);
	}

	void movePassenger(Vector2 velocity) {
		//Store all passengers moved on this frame
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();

		float directionX = Mathf.Sign (velocity.x);
		float directionY = Mathf.Sign (velocity.y);

		//vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + inset;

			for (int i = 0; i < verticalRays; i++) {
				//If directionY is negative (moving down), raycast from the bottom left
				//If directionY is positive (moving up), raycast from the top left
				Vector2 rayOrigin = (directionY == -1) ? raycasts.bottomLeft : raycasts.topLeft;
				//Add vertical ray spacing to raycast position
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				//Found a passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);
						//Give x velocity of platform to passanger if platform is moving up
						float pushX = (directionY == 1) ? velocity.x : 0f;
						//Pushing = platform's velocity - (detection gap - ray inset) in y dir
						float pushY = velocity.y - (hit.distance - inset) * directionY;
						
						hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
		//Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + inset;

			for (int i = 0; i < horizontalRays; i++) {
				//If directionX is negative (moving left), raycast from the bottom left
				//If directionX is positive (moving right), raycast from the bottom right
				Vector2 rayOrigin = (directionX == -1) ? raycasts.bottomLeft : raycasts.bottomRight;
				//Add horizontal ray spacing to raycast position
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				//Found passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);

						//Pushing = platform's velocity - (detection gap - ray inset) in x dir
						float pushX = velocity.x - (hit.distance - inset) * directionX;
						float pushY = 0f;
						
						hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
		//If passeger is on horizontally or downward moving platform
		if ((directionY == -1) || (velocity.y == 0 && velocity.x != 0)) {
			float rayLength = inset * 2;
			
			for (int i = 0; i < verticalRays; i++) {
				//Raycast from the top left
				Vector2 rayOrigin = raycasts.topLeft;
				//Add vertical ray spacing to raycast position
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
				
				//Found a passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);
						//Give x velocity of platform to passanger if platform is moving up
						float pushX = velocity.x;
						//Pushing = platform's velocity in y dir
						float pushY = velocity.y;
						
						hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
	}
}
