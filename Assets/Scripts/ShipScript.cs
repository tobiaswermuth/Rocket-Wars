using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {
	[SerializeField]
	private float movementCost = 1f;
	[SerializeField]
	private float acceleration = 3f;
	[SerializeField]
	private Rigidbody2D myRigidbody;

	private int lastPlayer = 0;
	private ArrayList pressedPlayers = new ArrayList();

	[SerializeField]
	private GameObject player1Inventory;
	[SerializeField]
	private GameObject player2Inventory;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.LeftArrow) && getPlayerEnergy(1) > movementCost) {
			myRigidbody.AddForce(Vector2.left  * acceleration);
			removePlayerEnergy(1, movementCost);
		}
		if (Input.GetKey(KeyCode.A) && getPlayerEnergy(-1) > movementCost) {
			myRigidbody.AddForce(Vector2.left  * acceleration);
			removePlayerEnergy(-1, movementCost);
		}
		if (Input.GetKey(KeyCode.RightArrow) && getPlayerEnergy(1) > movementCost) {
			myRigidbody.AddForce(Vector2.right * acceleration);
			removePlayerEnergy(1, movementCost);
		}
		if (Input.GetKey(KeyCode.D) && getPlayerEnergy(-1) > movementCost) {
			myRigidbody.AddForce(Vector2.right * acceleration);
			removePlayerEnergy(-1, movementCost);
		}
		if (Input.GetKeyDown(KeyCode.S) && !pressedPlayers.Contains(-1)) {
			lastPlayer = -1;
			pressedPlayers.Add(-1);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && !pressedPlayers.Contains(1)) {
			lastPlayer = 1;
			pressedPlayers.Add(1);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Part")) {
			if (lastPlayer == 0) {
				lastPlayer = (new int[]{-1, 1})[Random.Range(0, 2)];
			}

			GameObject inventory = lastPlayer == -1 ? player1Inventory : player2Inventory;
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
			if (transform.position.x - other.transform.position.x > 0) {
				position.x += GetComponent<CircleCollider2D>().radius;
			} else {
				position.x -= GetComponent<CircleCollider2D>().radius;
			}
			transform.position = position;
		}
	}

	GameObject findInventoryPart(GameObject inventory, GameObject part) {
		PartScript partScript = part.GetComponent<PartScript>();
		foreach(Transform child in inventory.transform) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript && inventoryPartScript.partIdentifier == partScript.partIdentifier) {
				return child.gameObject;
			}
		}
		return null;
	}

	float getPlayerEnergy(int player) {
		GameObject inventory = player == -1 ? player1Inventory : player2Inventory;
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
		GameObject inventory = player == -1 ? player1Inventory : player2Inventory;

		foreach(Transform child in inventory.transform) {
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
		GameObject inventory = player == -1 ? player1Inventory : player2Inventory;
		
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
