using UnityEngine;
using System.Collections;

public class ApplicationControllerScript : MonoBehaviour {
	void Awake () {
		Application.targetFrameRate = 60;
	}
}
