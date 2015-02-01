using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	int score = 0;

	public float jumpSpeed = 4f;
	public float downSpeed = 2f;
	float flyingSpeed = 0.05f;
	Vector3 velocity = Vector3.zero;
	bool isEnd = false;
	
	const float ratio = 2.8397f * 750f / 1334f;
	float screenWidth;

	Collider2D myCollider;
	public GameObject rainbow;

	// Should not create new object in Update() or FixedUpdate(), so we need these variables
	Vector3 _p;
	Vector3 _v = Vector3.zero;

	// Use this for initialization
	void Start () {
		Camera cam = Camera.main;
		float height = 2f * ratio * (camera.GetScreenHeight()/camera.GetScreenWidth()); // height is always twice as orthographicSize
		screenWidth = height * cam.aspect;
		myCollider = GetComponent<Collider2D> ();

		// Create 5 first rainbows
		for (int i = 0; i < 5; i++) {
			CreateRainbow();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnd) {
			if (Input.GetKeyDown (KeyCode.Space) || 
			    Input.touches.Length > 0) {
				// Restart game
				RainbowController.highBlock = -1200f;
				Application.LoadLevel( Application.loadedLevel );
			}
			return;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			velocity = Vector3.right * flyingSpeed;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			velocity = - Vector3.right * flyingSpeed;
		} else if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			velocity = Vector3.zero;
		} else if (Input.acceleration.x != 0) {
			_v.x = Input.acceleration.x / 10;
			velocity = _v;
		}
	}

	void FixedUpdate () {
		transform.position += velocity;
		_p = transform.position;
		// If eached the edges, move to another edge
		if (Mathf.Abs(_p.x) > (screenWidth + myCollider.bounds.size.x) / 2) {
			_p.x = - _p.x + 0.01f * (_p.x / Mathf.Abs(_p.x));
			transform.position = _p;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag.Equals ("Finish")) {
			// Reached the bottom, end the game
			isEnd = true;
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

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag.Equals ("ScorePoint")) {
			collider.tag = "ScorePointExpired";
			CreateRainbow ();
			score++;
			Debug.Log("Score: " + score);
		}
	}

	void CreateRainbow() {
		Instantiate (rainbow, new Vector3(0, 3f), Quaternion.identity);
	}
}
