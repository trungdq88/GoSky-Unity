using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	int score = 0;

	public float jumpSpeed = 1f;
	public float downSpeed = 0.3f;
	float flyingSpeed = 0.05f;
	Vector3 velocity = Vector3.zero;
	bool isEnd = false;

	float screenWidth = 3.2f;

	float objectWidth = 0.24f;
	float rotateSpeed = 1f;

	public GameObject rainbow;
	Animator animator;

	// Should not create new object in Update() or FixedUpdate(), so we need these variables
	Vector3 _p;
	Vector3 _v = Vector3.zero;

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		animator = GetComponentInChildren<Animator> ();
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

		// God mode
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.touches.Length > 1) {
			velocity += Vector3.up * 0.1f;
		}
	}

	void FixedUpdate () {
		// Is the game ended?
		if (isEnd) {
			return;
		}

		// Flip the fish to velocity
		_p = transform.localScale;
		_p.y = velocity.x > 0 ? 1 : -1;
		transform.localScale = _p;

		// Rotate the fish to velocity
		var dir = rigidbody2D.velocity  + new Vector2(velocity.x, velocity.y) * 60f;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(angle);

		// Move the fish horizontally
		transform.position += velocity;

		// If eached the edges, move to another edge
		_p = transform.position;
		if (Mathf.Abs(_p.x) > (screenWidth + objectWidth) / 2) {
			_p.x = - _p.x + 0.01f * (_p.x / Mathf.Abs(_p.x));
			transform.position = _p;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		// Collide with the bottom
		if (collision.collider.tag.Equals ("Finish")) {
			// Reached the bottom, end the game
			animator.SetTrigger("die");
			isEnd = true;
			return;
		}

		// Collide with the rainbows
		if (collision.collider.tag.Equals ("Rainbow")
		    || collision.collider.tag.Equals ("FirstRainbow")) {
			if (collision.collider.transform.position.y < transform.position.y) {
				// If the rainbow is below the fish, then jump up
				rigidbody2D.velocity = Vector2.up * jumpSpeed;
			} else {
				// If the rainbow is above the fish, "bounch" the fish down
				rigidbody2D.velocity = - Vector2.up * downSpeed;
			}
		} else
		// Collide with the springs
		if (collision.collider.tag.Equals ("Spring")) {
			rigidbody2D.velocity = Vector2.up * jumpSpeed * 1.4f;
		}
		// Collide with the black hole
		if (collision.collider.tag.Equals ("TheBlackHole")) {
			// Do something here?
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag.Equals ("ScorePoint")) {
			collider.tag = "ScorePointExpired";
			CreateRainbow ();
			score++;
			Debug.Log("Score: " + score);

			GameObject[] objs = GameObject.FindGameObjectsWithTag("ScoreText");
			foreach (GameObject obj in objs) {
				Text t = obj.GetComponent<Text>();
				t.text = score.ToString();

			}
		}
	}

	void CreateRainbow() {
		Instantiate (rainbow, new Vector3(0, 3f), Quaternion.identity);
	}
}
