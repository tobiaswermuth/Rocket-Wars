using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	enum MovementType { Path, Float }

	[SerializeField]
	private MovementType movementType = MovementType.Path;
	[SerializeField]
	private float movementCost = 1f;
	[SerializeField]
	private float energyReloadAmount = 0.003f;
	[SerializeField]
	private float movementTimeDifference = 0.1f;
	[SerializeField]
	private Rigidbody2D myRigidbody;
	
	private PlayerScript[] players;
	private PlayerScript lastPlayer = null;
	private PlayerScript winner = null;

	[SerializeField]
	private LevelSpawnerScript levelSpawner;

	public static ShipScript instance;

	// Use this for initialization
	void Start () {
		instance = this;
		
		players = GetComponents<PlayerScript>();
		foreach (PlayerScript player in players) {
			player.rocket.spawn();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject nearestLevelPiece = levelSpawner.getNearestLevelPiece (transform.position);
		float[] pathXs = nearestLevelPiece.GetComponent<LevelPieceScript> ().getNearestTwoPathXs (transform.position);

		Vector3 position = transform.position;
		
		foreach (PlayerScript player in players) {
			if (Time.time - player.lastMovementTimestamp > movementTimeDifference && player.getEnergy() >= movementCost) {
				if (movementType == MovementType.Path ? Input.GetKeyDown(player.leftKey) : Input.GetKey(player.leftKey)) {
					if (movementType == MovementType.Path) {
						position.x = pathXs [0];
						player.removeEnergy(movementCost);
					} else {
						myRigidbody.AddForce(Vector2.left * 100);
						player.removeEnergy(movementCost / 30);
					}
					player.lastMovementTimestamp = Time.time;
				} else if (movementType == MovementType.Path ? Input.GetKeyDown(player.rightKey) : Input.GetKey(player.rightKey)) {
					if (movementType == MovementType.Path) {
						position.x = pathXs [1];
						player.removeEnergy(movementCost);
					} else {
						myRigidbody.AddForce(Vector2.right * 100);
						player.removeEnergy(movementCost / 30);
					}
					player.lastMovementTimestamp = Time.time;
				}
			}
			
			if (!player.hasGrabbed && Input.GetKeyDown(player.grabKey)) {
				lastPlayer = player;
				player.hasGrabbed = true;
			}
			
			player.addEnergy(energyReloadAmount);
		}

		transform.position = position;
	}
	
	void win(PlayerScript player) {
		Destroy (player.rocket.marker);
		player.rocket.start();
		winner = player;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			if (lastPlayer == null) {
				lastPlayer = randomPlayer();
			}
			PartScript levelPart = other.gameObject.GetComponent<PartScript>();
			RocketScript rocket = lastPlayer.rocket;
			GameObject rocketPart = rocket.findNextPartWithIdentifier(levelPart.partIdentifier);
			if (rocketPart) {
				rocketPart.GetComponent<PartScript>().collected = true;
				GameObject nextPart = rocket.findNextPart();
				if (nextPart) {
					Vector3 markerPosition = rocket.marker.transform.position;
					markerPosition.y = nextPart.transform.position.y;
					rocket.marker.transform.position = markerPosition;
				} else {
					win(lastPlayer);
				}
			} else {
				lastPlayer.addEnergy(levelPart.getEnergy());
			}

			Destroy(other.gameObject);

			foreach(PlayerScript player in players) {
				player.hasGrabbed = false;
			}
			lastPlayer = null;
		} else if (other.CompareTag("Obstacle")) {
			Vector3 position = transform.position;
			GameObject nearestLevelPiece = levelSpawner.getNearestLevelPiece (transform.position);
			float[] pathXs = nearestLevelPiece.GetComponent<LevelPieceScript> ().getNearestTwoPathXs (transform.position);
			if (movementType == MovementType.Path) {
				position.x = pathXs[Random.Range(0, 2)];
			} else if (movementType == MovementType.Float) {
				if (Mathf.Abs(pathXs[0] - transform.position.x) < Mathf.Abs(pathXs[1] - transform.position.x)) {
					position.x = pathXs[0];
				} else {
					position.x = pathXs[1];
				}
			}
			transform.position = position;
		}
	}

	public PlayerScript randomPlayer() {
		return players[Random.Range(0, players.Length)];
	}
}
