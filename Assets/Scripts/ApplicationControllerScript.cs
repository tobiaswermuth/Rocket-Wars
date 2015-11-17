using UnityEngine;
using System.Collections;

public class ApplicationControllerScript : MonoBehaviour {
	public PlayerScript winner;
	
	public static ApplicationControllerScript instance;
	
	void Awake () {
		Application.targetFrameRate = 60;
		
	}
	
	void Start () {
		if (!instance) {
			instance = this;
			DontDestroyOnLoad(transform.gameObject);
		} else {
			Destroy(this);
		}
	}
}
