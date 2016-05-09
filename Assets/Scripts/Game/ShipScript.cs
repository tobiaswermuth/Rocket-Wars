using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D myRigidbody;
	
	private PlayerScript[] players;
	private PlayerScript winner = null;

	[SerializeField]
	private LevelSpawnerScript levelSpawner;

	public static ShipScript instance;

	void Start () {
		instance = this;
		
		players = GetComponents<PlayerScript>();
		foreach (PlayerScript player in players) {
			player.rocket.spawn();
		}
	}

	void FixedUpdate () {
		if (!winner) {
			Vector3 position = transform.position;
			
			foreach (PlayerScript player in players) {
				if (Input.GetKey(player.forwardKey)) {
					addPlayerMovement(player, 10f);
				} else if (Input.GetKey(player.backwardKey)) {
					addPlayerMovement(player, -10f);
				}
			}
	
			transform.position = position;
		}
	}
	
	public void addPlayerMovement(PlayerScript player, float strength) {
		Vector2 forcePosition = player.paddle.transform.position;

		var shipAngle = transform.rotation.eulerAngles.z;
		Vector2 forceDirection = new Vector2(
			-Mathf.Sin(Mathf.Deg2Rad * shipAngle), 
			Mathf.Cos(Mathf.Deg2Rad * shipAngle));

		myRigidbody.AddForceAtPosition(forceDirection * strength, forcePosition);
	}
	
	void win(PlayerScript player) {
		Destroy (player.rocket.marker);
		player.rocket.start();
		winner = player;
		
		ApplicationControllerScript.instance.winner = winner;
		
		StartCoroutine(startEndCountdown());
	}
	
	IEnumerator startEndCountdown () {
		yield return new WaitForSeconds (3f);
		GetComponent<ChangeGameStateScript> ().endGame ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			partCollision (other);
		}
	}

	void partCollision(Collider2D partCollider) {
		PartScript levelPart = partCollider.gameObject.GetComponent<PartScript>();

		PlayerScript playerToGetPart = randomPlayer ();
		foreach (PlayerScript player in players) {
			GameObject pRocketPart = player.rocket.findNextPartWithIdentifier(levelPart.partIdentifier);
			if (pRocketPart) {
				if (player.rocket.remainingParts().Count > playerToGetPart.rocket.remainingParts().Count) {
					playerToGetPart = player;
				}
			}
		}

		RocketScript rocket = playerToGetPart.rocket;
		GameObject rocketPart = rocket.findNextPartWithIdentifier(levelPart.partIdentifier);
		if (rocketPart) {
			rocketPart.GetComponent<PartScript>().collected = true;
			GameObject nextPart = rocket.findNextPart();
			if (nextPart) {
				Vector3 markerPosition = rocket.marker.transform.position;
				markerPosition.y = nextPart.transform.position.y;
				rocket.marker.transform.position = markerPosition;
			} else {
				win(playerToGetPart);
			}
		}

		Destroy(partCollider.gameObject);
	}

	public PlayerScript randomPlayer() {
		return players[Random.Range(0, players.Length)];
	}
	
	public PlayerScript playerOnSite(float x) {
		foreach(PlayerScript player in players) {
			if ((player.shipPosition == PlayerScript.PlayerShipPosition.left && x < 0) || (player.shipPosition == PlayerScript.PlayerShipPosition.right && x > 0)) {
				return player;
			}
		}
		return null;
	}
}
