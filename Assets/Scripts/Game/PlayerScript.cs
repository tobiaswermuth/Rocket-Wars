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
	[SerializeField]
	public KeyCode grabKey;
	
	public enum PlayerShipPosition {left, right};
	[SerializeField]
	public PlayerShipPosition shipPosition;
	
	public bool hasGrabbed = false;

	
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
