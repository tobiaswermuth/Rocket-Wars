using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchControllerScript : MonoBehaviour {
	[SerializeField]
	private float rowForceModifier = 1f;
	[SerializeField]
	private ShipScript ship;

	private Dictionary<int, PlayerScript> touches = new Dictionary<int, PlayerScript>();

	void FixedUpdate () {
		foreach (Touch touch in Input.touches) {
			Vector2 position = touch.position;
			if (touches.ContainsKey (touch.fingerId)) {
				ship.addPlayerMovement (touches[touch.fingerId], -touch.deltaPosition.y * rowForceModifier);
				touches.Remove (touch.fingerId);
			} else {
				touches.Add (touch.fingerId, ship.playerOnSite (touch.position.x - Screen.width / 2));
			}
		}
	}
}
