using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using AppodealAds.Unity;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdScript : MonoBehaviour {
	private String APPODEALS_APP_KEY = "220b434377fdc6264f1a2fa999d86f3147344bc982adb219";

	void Start() {
		UserSettings userSettings = new UserSettings ();
		userSettings.setAge (16);
		userSettings.setInterests ("games, friends");

		Appodeal.disableLocationPermissionCheck();
		Appodeal.initialize(APPODEALS_APP_KEY, Appodeal.INTERSTITIAL | Appodeal.BANNER);
		Appodeal.show(Appodeal.BANNER_TOP);
	}

	void OnDestroy() {
		Appodeal.hide (Appodeal.BANNER_TOP);
	}
}
