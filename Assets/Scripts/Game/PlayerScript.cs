using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	[SerializeField]
	public RocketScript rocket;

	[SerializeField]
	public GameObject paddle;
	
	[SerializeField]
	public KeyCode forwardKey;
	[SerializeField]
	public KeyCode backwardKey;
	
	public enum PlayerShipPosition {left, right};
	[SerializeField]
	public PlayerShipPosition shipPosition;
}
