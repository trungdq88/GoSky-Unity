using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
	public Vector3 velocity = Vector3.zero;
	
	float topDistance = 1f;
	float bottomDistance = 1f;

	public GameObject background;

	// const float ratio = 2.8397f * 750f / 1334f;
	Vector3 _p = Vector3.zero;
	Vector3 _p2 = Vector3.zero;

	GameObject player;

	float bgStartPos = 0f;

	// Use this for initialization
	void Start () {
		// camera.orthographicSize = ratio * (camera.GetScreenHeight()/camera.GetScreenWidth());
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player == null) {
			Debug.LogError("Player is null!");
		}
		Vector3 p = transform.position;
		p.y = player.transform.position.y + 2f;

		bgStartPos = background.transform.position.y;
	}

	void Update() {
		_p = transform.position;

		if (player.transform.position.y - _p.y > topDistance) {
			_p.y = player.transform.position.y - topDistance;
		}
		if (player.transform.position.y - _p.y < -bottomDistance) {
			_p.y = player.transform.position.y + bottomDistance;
		}
		transform.position = _p;
		_p2 = background.transform.position;
		_p2.y = _p.y * 0.5f + bgStartPos;
		background.transform.position = _p2;
	}

	// Update is called once per frame
	void FixedUpdate () {

	}
}
