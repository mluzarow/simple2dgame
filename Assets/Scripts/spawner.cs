using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {
	public GameObject bullet;
	public float x = 0f;
	public float y = 0f;
	
	void Update() {
		if (Input.GetKeyUp (KeyCode.Q)) {
			for (int j = 0; j < 5; j++) {
				y = 10f;

				spawnBullet();
			}

		}
	}

	public void spawnBullet() {
		for (int i = 0; i < 20; i++) {
			Instantiate (bullet, new Vector3 (0f, 0f, 0f), Quaternion.Euler(new Vector3(0f, 0f, x)));
			x += y;
			if (x >= 360) {
				y -= (y -1f);
			} else if (x <= -360) {
				y += 2f;
			}

		}
	}
}
