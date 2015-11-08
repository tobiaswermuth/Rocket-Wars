using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	[SerializeField]
	public RocketScript rocket;
	
	[SerializeField]
	public KeyCode leftKey;
	[SerializeField]
	public KeyCode rightKey;
	[SerializeField]
	public KeyCode grabKey;
	
	public bool hasGrabbed = false;
	public float lastMovementTimestamp = 0.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public float getEnergy() {
		float aggregatedEnergy = 0;
		
		foreach(Transform child in rocket.transform) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript && !inventoryPartScript.collected) {
				aggregatedEnergy += inventoryPartScript.getEnergy();
			}
		}
		
		return aggregatedEnergy;
	}
	
	public void addEnergy(float energy) {		
		Transform[] allChildren = rocket.GetComponentsInChildren<Transform>();
		System.Array.Reverse (allChildren);
		
		foreach(Transform child in allChildren) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript && !inventoryPartScript.collected) {
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
	
	public void removeEnergy(float energy) {		
		foreach(Transform child in rocket.transform) {
			PartScript inventoryPartScript = child.gameObject.GetComponent<PartScript>();
			if (inventoryPartScript && !inventoryPartScript.collected) {
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
