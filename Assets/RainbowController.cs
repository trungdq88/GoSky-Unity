using UnityEngine;
using System.Collections;

public class RainbowController : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;

	float dying = 0f; // 0 = still living, > 0: dying
	float dieDelay = 1f;
	float alphaLevel = 1f;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = transform.GetComponent<SpriteRenderer> ();
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
		transform.position += velocity;

	}
	void OnTriggerEnter2D(Collider2D collider) {
		// Debug.Log ("TriggerCollision");
		// If the rainbow hit the side edges, reverse the velocity so it will moving around the screen
		if (collider.tag.Equals ("SideEdge")) {
			velocity *= -1;
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
