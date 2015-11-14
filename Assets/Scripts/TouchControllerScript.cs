using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchControllerScript : MonoBehaviour {
	[SerializeField]
	private float yRange = 100.0f;
	[SerializeField]
	private ShipScript ship;

	private Dictionary<int, Vector2> touches = new Dictionary<int, Vector2>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Touch touch in Input.touches) {
			Vector2 position = touch.position;
			if (touches.ContainsKey(touch.fingerId) && touch.phase == TouchPhase.Began) {
				touches.Remove(touch.fingerId);
			}
			if (!touches.ContainsKey(touch.fingerId)) {
				touches.Add(touch.fingerId, position);
			}
			Vector2 startPosition = touches[touch.fingerId];
			
			float deltaY = position.y - startPosition.y;
			if (deltaY > yRange) {
				deltaY = yRange;
			} else if (deltaY < -yRange) {
				deltaY = -yRange;
			}
			float yTraveled = deltaY / yRange;
			ship.addPlayerMovement(ship.playerOnSite(startPosition.x - Screen.width /2), yTraveled);
		}
	}
}
