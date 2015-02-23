using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
	public Vector3 velocity = Vector3.zero;
	
	float topDistance = 1f;
	float bottomDistance = 1f;

	// const float ratio = 2.8397f * 750f / 1334f;

	GameObject player;
	// Use this for initialization
	void Start () {
		// camera.orthographicSize = ratio * (camera.GetScreenHeight()/camera.GetScreenWidth());
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player == null) {
			Debug.LogError("Player is null!");
		}
		Vector3 p = transform.position;
		p.y = player.transform.position.y + 2f;
	}

	void Update() {
		Vector3 p = transform.position;

		if (player.transform.position.y - p.y > topDistance) {
			p.y = player.transform.position.y - topDistance;
		}
		if (player.transform.position.y - p.y < -bottomDistance) {
			p.y = player.transform.position.y + bottomDistance;
		}
		transform.position = p;
	}

	// Update is called once per frame
	void FixedUpdate () {

	}
}
