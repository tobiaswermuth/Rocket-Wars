using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyControllerScript : MonoBehaviour {
	[SerializeField]
	Text leftLabel;

	[SerializeField]
	Text rightLabel;
	
	void Start () {
		if (ApplicationModel.winnerSide != null) {
			if (ApplicationModel.winnerSide == PlayerScript.PlayerShipPosition.left) {
				leftLabel.text = "Won!";
				rightLabel.text = "Lost!";
			} else {
				leftLabel.text = "Lost!";
				rightLabel.text = "Won!";
			}
		} else {
			leftLabel.text = "";
			rightLabel.text = "";
		}
	}
}
