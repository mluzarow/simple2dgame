using UnityEngine;
using System.Collections;

public class spawnerClone : MonoBehaviour {
	Vector2 direction;



	// Use this for initialization
	void Start () {

		direction = new Vector2 (2f, -2f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction * Time.deltaTime);

		if (Mathf.Abs(transform.position.x) > 200f || Mathf.Abs (transform.position.y) > 400f) {
			Destroy(gameObject);
		}
	}
}
