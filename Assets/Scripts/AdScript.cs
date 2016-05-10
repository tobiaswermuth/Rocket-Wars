using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdScript : MonoBehaviour {
	private String APPODEALS_APP_KEY = "220b434377fdc6264f1a2fa999d86f3147344bc982adb219";

	void Start() {
		Appodeal.disableLocationPermissionCheck();
		Appodeal.initialize(APPODEALS_APP_KEY, Appodeal.INTERSTITIAL | Appodeal.BANNER);
	}

	void OnGUI() {
		Appodeal.show(Appodeal.BANNER_TOP);
	}
}
