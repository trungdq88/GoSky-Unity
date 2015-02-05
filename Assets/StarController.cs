using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (this.tag.Equals("CloneStar")) {
			Invoke ("SelfDestroy", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void SelfDestroy() {
		Destroy (gameObject);
	}
}
