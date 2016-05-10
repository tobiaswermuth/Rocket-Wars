using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	[SerializeField]
	private Rigidbody2D myRigidbody;
	[SerializeField]
	public GameObject tutorial;

	[SerializeField]
	private int maxAngle = 90;

	[SerializeField]
	private LevelSpawnerScript levelSpawner;

	private PlayerScript[] players;
	private PlayerScript winner = null;

	public static ShipScript instance;

	void Start () {
		Application.targetFrameRate = 60;

		instance = this;
		
		players = GetComponents<PlayerScript>();
		foreach (PlayerScript player in players) {
			player.rocket.spawn();
		}

		ApplicationModel.winnerSide = null;
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

	void Update() {
		var eulerAngles = transform.eulerAngles;
		// transform angles 0 - 360 to 0 - 180\-180 - 0
		var shipAngle = ((eulerAngles.z + 180) % 360) - 180;
		if (shipAngle > maxAngle) {
			eulerAngles.z = maxAngle;
		} else if (shipAngle < -maxAngle) {
			eulerAngles.z = -maxAngle;
		}
		transform.eulerAngles = eulerAngles;
	}
	
	public void addPlayerMovement(PlayerScript player, float strength) {
		Vector2 forcePosition = player.paddle.transform.position;

		var shipAngle = transform.eulerAngles.z;
		Vector2 forceDirection = new Vector2(
			-Mathf.Sin(Mathf.Deg2Rad * shipAngle), 
			Mathf.Cos(Mathf.Deg2Rad * shipAngle));

		myRigidbody.AddForceAtPosition(forceDirection * strength, forcePosition);

		if (tutorial != null) {
			Destroy (tutorial);
		}
	}
	
	void win(PlayerScript player) {
		Destroy (player.rocket.marker);
		player.rocket.ignite();
		winner = player;
		
		ApplicationModel.winnerSide = winner.shipPosition;

		StartCoroutine(startEndCountdown());
	}
	
	IEnumerator startEndCountdown () {
		yield return new WaitForSeconds (3f);

		GetComponent<ChangeGameStateScript>().toLobby ();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			partCollision (other);
		}
	}

	void partCollision(Collider2D partCollider) {
		PartScript levelPart = partCollider.gameObject.GetComponent<PartScript>();

		PlayerScript playerToGetPart = null;
		foreach (PlayerScript player in players) {
			GameObject pRocketPart = player.rocket.findNextPartWithIdentifier(levelPart.partIdentifier);
			if (pRocketPart) {
				if (playerToGetPart == null || player.rocket.remainingParts().Count > playerToGetPart.rocket.remainingParts().Count) {
					playerToGetPart = player;
				}
			}
		}

		if (playerToGetPart != null) {
			RocketScript rocket = playerToGetPart.rocket;
			GameObject rocketPart = rocket.findNextPartWithIdentifier (levelPart.partIdentifier);
			if (rocketPart != null) {
				rocketPart.GetComponent<PartScript> ().collected = true;
				GameObject nextPart = rocket.findNextPart ();
				if (nextPart) {
					Vector3 markerPosition = rocket.marker.transform.position;
					markerPosition.y = nextPart.transform.position.y;
					rocket.marker.transform.position = markerPosition;
				} else {
					win (playerToGetPart);
				}
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
