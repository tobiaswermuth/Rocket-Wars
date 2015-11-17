using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndControllerScript : MonoBehaviour {
	
	[SerializeField]
	Text wonLabel;
	
	// Use this for initialization
	void Start () {
		wonLabel.text = "Player on " + ApplicationControllerScript.instance.winner.shipPosition + " side wins!";
	}
}
