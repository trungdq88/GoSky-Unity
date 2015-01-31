using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float jumpSpeed = 4f;
	public float downSpeed = 2f;
	float flyingSpeed = 0.05f;
	Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			velocity = Vector3.right * flyingSpeed;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			velocity = - Vector3.right * flyingSpeed;
		} else if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			velocity = Vector3.zero;
		} else if (Input.acceleration.x != 0) {
			velocity = new Vector3 (Input.acceleration.x / 10, 0.0f, 0.0f);
		}



	}

	void FixedUpdate () {
		transform.position += velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag.Equals ("Finish")) {
			// Reached the bottom, end the game
			return;
		}
		if (collision.collider.transform.position.y < transform.position.y) {
			// If the rainbow is below the fish, then jump up
			rigidbody2D.velocity = Vector2.up * jumpSpeed;
		} else {
			// If the rainbow is above the fish, "bounch" the fish down
			rigidbody2D.velocity = - Vector2.up * downSpeed;
		}
	}
}
