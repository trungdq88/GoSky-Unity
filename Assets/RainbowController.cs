using UnityEngine;
using System.Collections;

public class RainbowController : MonoBehaviour {
	public static float highBlock = -1200f;

	float minDistance = 1f;
	float maxDistance = 1.5f;

	float maxScale = 1f;
	float minScale = 0.1f;

	float minSpeed = 0.005f;
	float maxSpeed = 0.03f;
	float speed = 0f;
	float dying = 0f; // 0 = still living, > 0: dying
	float dieDelay = 1f;
	float alphaLevel = 1f;

	SpriteRenderer spriteRenderer;
	Collider2D myCollider;
	Renderer myRenderer;

	const float ratio = 2.8397f * 750f / 1334f;	
	float screenWidth;


	Vector3 _p = Vector3.zero;

	// Use this for initialization
	void Start () {
		spriteRenderer = transform.GetComponent<SpriteRenderer> ();
		myRenderer = transform.GetComponent<Renderer> ();
		myCollider = GetComponent<Collider2D> ();

		Camera cam = Camera.main;
		float height = 2f * ratio * (camera.GetScreenHeight()/camera.GetScreenWidth()); // height is always twice as orthographicSize
		screenWidth = height * cam.aspect;

		if (!gameObject.tag.Equals ("FirstRainbow")) {
			highBlock += Random.Range (minDistance, maxDistance);
			speed = Random.Range (minSpeed, maxSpeed);
			_p = transform.position;
			_p.y = highBlock;
			transform.position = _p;
			_p = transform.localScale;
			_p.x = Random.Range (minScale, maxScale);
			transform.localScale = _p;

			// This is for the root rainbow, which will be used to copy to other rainbows
			if (highBlock <= -1000f) {
				highBlock = 0.3f;
			}
		}
	}

	void Update() {
		if (dying > 0) {
			alphaLevel -= Time.deltaTime * dieDelay;
			spriteRenderer.color = new Color (1f, 1f, 1f, alphaLevel);
			if (alphaLevel <= 0) {
				DisapearNow();
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		_p = transform.position;
		_p.x += speed;
		transform.position = _p;


		if (Mathf.Abs(_p.x) > (screenWidth - myCollider.bounds.size.x) / 2) {
			speed *= -1;
		}
	}
	void OnTriggerEnter2D(Collider2D collider) {
		// Debug.Log ("TriggerCollision");
		// If the rainbow hit the side edges, reverse the velocity so it will moving around the screen
		if (collider.tag.Equals ("SideEdge")) {

		}
	}
	void OnCollisionEnter2D(Collision2D collision) {
		// Debug.Log ("Collision");
	
		if (collision.collider.transform.position.y > transform.position.y) {
			// Make the rainbow fadeing out
			dying = Time.time;
			// This is for the case the fish is bounched down, we need the rainbow to "flash" again
			alphaLevel = 1;
		}
	}

	
	void DisapearNow() {
		Destroy (gameObject);
	}
}
