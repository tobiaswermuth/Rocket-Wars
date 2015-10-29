using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	[SerializeField]
	private float movementCost = 1f;
	[SerializeField]
	private Rigidbody2D myRigidbody;

	private int lastPlayer = -1;
	private ArrayList pressedPlayers = new ArrayList();
	private float[] pressedDirectionlast = new float[]{0f, 0f};

	[SerializeField]
	private GameObject player1Inventory;
	[SerializeField]
	private GameObject player2Inventory;

	[SerializeField]
	private LevelSpawnerScript levelSpawner;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject nearestLevelPiece = levelSpawner.getNearestLevelPiece (transform.position);
		float[] pathXs = nearestLevelPiece.GetComponent<LevelPieceScript> ().getNearestTwoPathXs (transform.position);

		Vector3 position = transform.position;
		if (position.y < nearestLevelPiece.transform.position.y - nearestLevelPiece.GetComponent<BoxCollider2D> ().size.y / 2 - GetComponent<CircleCollider2D> ().radius) {
			if (transform.position.x != pathXs [0]) {
				if (Time.time - pressedDirectionlast[0] > 0.1 && Input.GetKeyDown (KeyCode.A) && getPlayerEnergy (0) >= movementCost) {
					position.x = pathXs [0];
					pressedDirectionlast[0] = Time.time;
					removePlayerEnergy (0, movementCost);
				}
				if (Time.time - pressedDirectionlast[1] > 0.1 && Input.GetKeyDown (KeyCode.LeftArrow) && getPlayerEnergy (1) >= movementCost) {
					position.x = pathXs [0];
					pressedDirectionlast[1] = Time.time;
					removePlayerEnergy (1, movementCost);
				}
			}
			if (transform.position.x != pathXs [1]) {
				if (Time.time - pressedDirectionlast[0] > 0.1 && Input.GetKeyDown (KeyCode.D) && getPlayerEnergy (0) >= movementCost) {
					position.x = pathXs [1];
					pressedDirectionlast[0] = Time.time;
					removePlayerEnergy (0, movementCost);
				}
				if (Time.time - pressedDirectionlast[1] > 0.1 && Input.GetKeyDown (KeyCode.RightArrow) && getPlayerEnergy (1) >= movementCost) {
					position.x = pathXs [1];
					pressedDirectionlast[1] = Time.time;
					removePlayerEnergy (1, movementCost);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.S) && !pressedPlayers.Contains(0)) {
			lastPlayer = 0;
			pressedPlayers.Add(0);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && !pressedPlayers.Contains(1)) {
			lastPlayer = 1;
			pressedPlayers.Add(1);
		}
		transform.position = position;

		addPlayerEnergy (0, 0.005f);
		addPlayerEnergy (1, 0.005f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			if (lastPlayer == -1) {
				lastPlayer = Random.Range(0, 2);
			}

			GameObject inventory = lastPlayer == 0 ? player1Inventory : player2Inventory;
			GameObject inventoryPart = findInventoryPart(inventory, other.gameObject);
			if (inventoryPart) {
				Destroy(inventoryPart);
			} else {
				addPlayerEnergy(lastPlayer, other.GetComponent<PartScript>().getEnergy());
			}

			Destroy(other.gameObject);

			pressedPlayers = new ArrayList();
			lastPlayer = 0;
		} else if (other.CompareTag("Obstacle")) {
			Vector3 position = transform.position;
			GameObject nearestLevelPiece = levelSpawner.getNearestLevelPiece (transform.position);
			float[] pathXs = nearestLevelPiece.GetComponent<LevelPieceScript> ().getNearestTwoPathXs (transform.position);
			position.x = pathXs[Random.Range(0, 2)];
			transform.position = position;
		}
	}

	GameObject findInventoryPart(GameObject inventory, GameObject part) {
		PartScript partScript = part.GetComponent<PartScript>();
		PartScript inventoryPartScript = inventory.GetComponentsInChildren<PartScript>()[0];
		if (inventoryPartScript.partIdentifier == partScript.partIdentifier) {
			return inventoryPartScript.gameObject;
		}
		return null;
	}

	float getPlayerEnergy(int player) {
		GameObject inventory = player == 0 ? player1Inventory : player2Inventory;
		float aggregatedEnergy = 0;

		foreach(Transform child in inventory.transform) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript) {
				aggregatedEnergy += inventoryPartScript.getEnergy();
			}
		}

		return aggregatedEnergy;
	}

	void addPlayerEnergy(int player, float energy) {
		GameObject inventory = player == 0 ? player1Inventory : player2Inventory;

		Transform[] allChildren = inventory.GetComponentsInChildren<Transform>();
		System.Array.Reverse (allChildren);

		foreach(Transform child in allChildren) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript) {
				float energyToAddLeft = (inventoryPartScript.getEnergy() + energy) - inventoryPartScript.maxEnergy;
				if (energyToAddLeft < 0) {
					inventoryPartScript.setEnergy(inventoryPartScript.getEnergy() + energy);
					break;
				} else {
					inventoryPartScript.setEnergy(inventoryPartScript.maxEnergy);
				}
				energy = energyToAddLeft;
			}
		}
	}

	void removePlayerEnergy(int player, float energy) {
		GameObject inventory = player == 0 ? player1Inventory : player2Inventory;
		
		foreach(Transform child in inventory.transform) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript) {
				float energyToRemoveLeft = energy - inventoryPartScript.getEnergy();
				if (energyToRemoveLeft < 0) {
					inventoryPartScript.setEnergy(inventoryPartScript.getEnergy() - energy);
					break;
				} else {
					inventoryPartScript.setEnergy(0);
				}
				energy = energyToRemoveLeft;
			}
		}
	}
}
