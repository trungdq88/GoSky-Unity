using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour {
	BannerView bannerView;

	// Use this for initialization
	void Start () {
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-4299164781315902/5058695476";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-4299164781315902/1385154673";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
}
