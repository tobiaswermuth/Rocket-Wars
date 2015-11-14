using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	enum MovementType { Path, Float }

	[SerializeField]
	private MovementType movementType = MovementType.Path;
	[SerializeField]
	private float movementEnergyCost = 1f;
	[SerializeField]
	private float energyReloadAmount = 0.003f;
	[SerializeField]
	private float speed = 20f;
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
	
	void Update () {
		float velocityAngel = Vector2.Angle(new Vector2(1, 0), myRigidbody.velocity) - 90;
		Quaternion rotation = transform.rotation;
		rotation.z += (Quaternion.Euler(0, 0, velocityAngel).z - rotation.z)/20;
		transform.rotation = rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 position = transform.position;
		
		foreach (PlayerScript player in players) {
			if (player.getEnergy() >= movementEnergyCost) {
				if (Input.GetKey(player.forwardKey)) {
					addPlayerMovement(player, 1);
				} else if (Input.GetKey(player.backwardKey)) {
					addPlayerMovement(player, -1);
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
	
	public void addPlayerMovement(PlayerScript player, float direction) {
		Vector2 forceDirection = player.shipPosition == PlayerScript.PlayerShipPosition.left ? Vector2.right : Vector2.left; 
		myRigidbody.AddForce((forceDirection * speed + Vector2.up * speed/2) * direction);
		player.removeEnergy(movementEnergyCost);
	}
	
	void win(PlayerScript player) {
		Destroy (player.rocket.marker);
		player.rocket.start();
		winner = player;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			PartScript levelPart = other.gameObject.GetComponent<PartScript>();
			
			if (lastPlayer == null) {
				foreach (PlayerScript player in players) {
					GameObject pRocketPart = player.rocket.findNextPartWithIdentifier(levelPart.partIdentifier);
					if (pRocketPart) {
						if (lastPlayer == null) {
							lastPlayer = player;
						} else {
							lastPlayer = randomPlayer();
						}
					}
				}
				if (lastPlayer == null) {
					lastPlayer = randomPlayer();
				}
			}

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
	
	public PlayerScript playerOnSite(float x) {
		foreach(PlayerScript player in players) {
			if ((player.shipPosition == PlayerScript.PlayerShipPosition.left && x < 0) || (player.shipPosition == PlayerScript.PlayerShipPosition.right && x > 0)) {
				return player;
			}
		}
		return null;
	}
}
