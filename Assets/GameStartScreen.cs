using UnityEngine;
using System.Collections;

public class GameStartScreen : MonoBehaviour {

	static bool sawOnce = false;
	
	// Use this for initialization
	void Start () {
		ShowChildren ();
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale==0 && (Input.GetKeyDown (KeyCode.Space) || 
		                         Input.touches.Length > 0) ) {
			Time.timeScale = 1;
			HideChildren ();
			
		}
	}
	void HideChildren()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			lRenderer.enabled=false;
		}
	}
	void ShowChildren()
	{
		Renderer[] lChildRenderers=gameObject.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			lRenderer.enabled=true;
		}
	}

}
