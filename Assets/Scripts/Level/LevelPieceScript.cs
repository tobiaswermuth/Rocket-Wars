using UnityEngine;
using System.Collections;

public class LevelPieceScript : MonoBehaviour {
	[SerializeField]
	private GameObject[] possibleParts;

	void Start () {
		replacePlaceholdersWithParts();
	}
	
	private void replacePlaceholdersWithParts() {
		foreach(Transform child in transform){
			if (child.CompareTag("Part Placeholder")) {
				GameObject nextPartPrefab = possibleParts[Random.Range(0, possibleParts.Length)];
				if (Random.Range(0, 3) == 2) {
					GameObject nextPlayerPart = ShipScript.instance.randomPlayer().rocket.findNextPart();
					if (nextPlayerPart) {
						nextPartPrefab = nextPlayerPart;
					}
				}
				
				GameObject part = Instantiate(nextPartPrefab, child.position, Quaternion.identity) as GameObject;
				part.transform.SetParent(transform);
				part.GetComponent<Rigidbody2D>().isKinematic = true;
				part.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
				part.GetComponent<PartScript> ().collectable = true;
				
				Destroy(child.gameObject);
			}
		}
	}
}
