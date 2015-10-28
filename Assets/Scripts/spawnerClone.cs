using UnityEngine;
using System.Collections;

public class spawnerClone : MonoBehaviour {
	///<summary>The speed of the game object.</summary>
	public Vector2 velocity;
	///<summary>The angle of the game object.</summary>
	public float angle;
	///<summary>The distance the game object can travel before it is destroyed.</summary>
	public Vector2 limits;

	// Update is called once per frame
	void Update () {
		transform.Translate (velocity * Time.deltaTime);

		if (Mathf.Abs(transform.position.x) > limits.x || Mathf.Abs (transform.position.y) > limits.y) {
			Destroy(gameObject);
		}
	}
}
