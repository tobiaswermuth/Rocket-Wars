using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndControllerScript : MonoBehaviour {
	
	[SerializeField]
	Text wonLabel;
	
	void Start () {
		wonLabel.text = ApplicationControllerScript.instance.winner.shipPosition + " Player wins!";
	}
}
