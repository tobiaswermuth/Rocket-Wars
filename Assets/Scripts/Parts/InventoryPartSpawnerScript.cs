using UnityEngine;
using System.Collections;

public class InventoryPartSpawnerScript : MonoBehaviour {
	[SerializeField]
	private int partNumber = 5;
	[SerializeField]
	private float partDistance = 1f;
	[SerializeField]
	private GameObject[] partPrefabs;
	private GameObject nextPartObject;
	private GameObject lastPartObject;

	// Use this for initialization
	void Start () {
		nextPartObject = partPrefabs[Random.Range(0, partPrefabs.Length)];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (partNumber > 0 && (!lastPartObject || 
		    transform.position.y - lastPartObject.transform.position.y > partDistance)) {
			
			lastPartObject = Instantiate(nextPartObject, transform.position, Quaternion.identity) as GameObject;
			lastPartObject.transform.localScale = transform.parent.localScale;
			lastPartObject.transform.SetParent(transform.parent);
			
			nextPartObject = partPrefabs[Random.Range(0, partPrefabs.Length)];
			partNumber--;
		}
	}
}
