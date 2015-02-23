using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	int score = 0;

	public float jumpSpeed = 1f;
	public float downSpeed = 0.3f;
	float flyingSpeed = 0.05f;
	Vector3 velocity = Vector3.zero;

	Vector3 centerPoint;

	bool isEnd = false;
	bool isFalling = false;
	bool isPaused = true; // The first value is true to match with the GameStartScreen

	float screenWidth = 3.2f;

	float objectWidth = 0.24f;

	float starDelay = 0.05f;
	float starDelayCount = 0f;
	float starDistance = 0.3f;

	// Set via GUI
	public GameObject rainbow;
	public GameObject star; 
	public GameObject gameOver;
	public GameObject bestScore;
	public GameObject scoreHolder;
	public GameObject bestScoreHolder;
	public GameObject resumeBtn; 
	public AudioClip dieSound;
	public AudioClip jumpSound;
	public AudioClip highJumpSound;
	public AudioClip hitTopSound;
	public AudioClip nyan1Sound;
	public AudioClip nyan2Sound;
	public AudioClip clickSound;

	int bestScoreValue = 0;

	Animator animator;

	// Should not create new object in Update() or FixedUpdate(), so we need these variables
	Vector3 _p;
	Vector3 _v = Vector3.zero;
	Vector3 _starPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// Set position for texts
		centerPoint = new Vector3 ( Screen.width / 2, Screen.height / 2f,  10f);
		_p = centerPoint;
		// Score
		_p.y = Screen.height * 0.9f;
		scoreHolder.transform.position = _p;
		// Best score
		_p.y = Screen.height * 0.285f;
		bestScoreHolder.transform.position = _p;

		animator = GetComponentInChildren<Animator> ();

		gameOver.renderer.enabled = false;
		bestScore.renderer.enabled = false;
		resumeBtn.renderer.enabled = false;
		setText("BestScoreText", "");

		bestScoreValue = PlayerPrefs.GetInt("bestScore", 0);

		// Create 5 first rainbows
		for (int i = 0; i < 5; i++) {
			CreateRainbow();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (rigidbody2D.velocity.y < -6) {
			// Star 
			starDelayCount += Time.deltaTime;
			if (starDelayCount > starDelay) {
				starDelayCount = 0;
				CreateStar ();
			}
			if (isFalling == false) {
				playEndMusic();
			}
			isFalling = true;
			gameObject.layer = LayerMask.NameToLayer("NoRainbow");

			// If the fish is falling, tap will skip the falling movie
			if (Input.GetKeyDown (KeyCode.Space) || 
			    Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				// Go to bottom immediately
				_p.y = 0.5f;
				transform.position = _p;
			}
		}

		if (isEnd) {
			// If the game is end, tap will restart the game
			if (Input.GetKeyDown (KeyCode.Space) || 
			    Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				// Restart game
				playMusic(clickSound);
				RainbowController.highBlock = -1200f;
				Application.LoadLevel( Application.loadedLevel );
			}
			gameOver.renderer.enabled = true;
			bestScore.renderer.enabled = true;
			setText("BestScoreText", bestScoreValue.ToString());

			scoreHolder.transform.position = centerPoint;
			return;
		}

		// If game is playing, tap will pause the game
		if (!isFalling && Input.GetKeyDown (KeyCode.Space) || 
		    Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			// Pause/resume game
			isPaused = !isPaused;
			updateGameState();
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
			playMusic(dieSound);
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
				playMusic(jumpSound);
			} else {
				// If the rainbow is above the fish, "bounch" the fish down
				rigidbody2D.velocity = - Vector2.up * downSpeed;
				playMusic(hitTopSound);
			}
		} else
		// Collide with the springs
		if (collision.collider.tag.Equals ("Spring")) {
			rigidbody2D.velocity = Vector2.up * jumpSpeed * 1.6f;
			playMusic(highJumpSound);
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
			setText("ScoreText", score.ToString());

			if (score > bestScoreValue) {
				bestScoreValue = score;
				PlayerPrefs.SetInt("bestScore", bestScoreValue);
			}
		}
	}

	void setText(string tag, string text) {
		GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject obj in objs) {
			Text t = obj.GetComponent<Text>();
			t.text = text;
		}
	}
	void updateGameState() {
		if (isPaused) {
			Time.timeScale = 0;
			AudioListener.pause = true;
			resumeBtn.renderer.enabled = true;
		} else {
			Time.timeScale = 1;
			AudioListener.pause = false;
			resumeBtn.renderer.enabled = false;
		}
		playMusic(clickSound);
	}
	void CreateRainbow() {
		Instantiate (rainbow, new Vector3(0, 3f), Quaternion.identity);
	}
	void CreateStar() {
		_starPos.x = Random.Range (-starDistance, starDistance);
		_starPos.y = Random.Range (-starDistance, starDistance);
		_starPos.z = 0;
		GameObject g = (GameObject) Instantiate (star, transform.position + _starPos, Quaternion.identity);
		g.tag = "CloneStar";
	}
	void playMusic(AudioClip clip) {
		CancelInvoke ("playLoopSection");
		audio.Stop ();
		audio.loop = false;
		audio.clip = clip;
		audio.Play ();
	}
	void playEndMusic() {
		audio.Stop ();
		audio.volume = 0.7f;
		audio.clip = nyan1Sound;
		audio.Play ();
		Invoke("playLoopSection", nyan1Sound.length);
	}
	void playLoopSection() {
		audio.Stop ();
		audio.volume = 0.7f;
		audio.clip = nyan2Sound;
		audio.loop = true;
		audio.Play ();
	}
}
