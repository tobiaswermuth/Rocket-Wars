using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketScript : MonoBehaviour {
	private bool hasStarted = false;
	
	[SerializeField]
	public GameObject marker;
	[SerializeField]
	public GameObject partSpawn;
	
	[SerializeField]
	RocketBuilderScript rocketBuilder;
	[SerializeField]
	private float partSpawnDistance = 1f;
	private GameObject[] spawnParts = new GameObject[0];
	private GameObject lastSpawnedPart;
	
	private List<PartScript> parts = new List<PartScript>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void spawn() {
		spawnParts = rocketBuilder.createRocket ();
	}
	
	// Update is called once per frame
	void Update() {
		if (parts.Count < spawnParts.Length && (!lastSpawnedPart || transform.position.y - lastSpawnedPart.transform.position.y > partSpawnDistance)) {
			lastSpawnedPart = Instantiate(spawnParts[parts.Count], partSpawn.transform.position, Quaternion.identity) as GameObject;
			//lastSpawnedPart.transform.localScale = transform.parent.localScale;
			lastSpawnedPart.transform.SetParent(transform);
			
			parts.Add(lastSpawnedPart.GetComponent<PartScript>());
		}
	}
	
	void FixedUpdate () {
		if (hasStarted) {
			parts[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100));
		}
	}
	
	public void start() {
		hasStarted = true;
		
		foreach (PartScript part in parts) {
			part.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		}
		foreach (ParticleSystem ps in parts[0].GetComponentsInChildren<ParticleSystem>()) {
			ps.Play();
		}
	}
	
	public GameObject findNextPartWithIdentifier(string partIdentifier) {
		foreach (PartScript inventoryPart in parts) {
			if (!inventoryPart.collected) {
				if (inventoryPart.partIdentifier == partIdentifier) {
					return inventoryPart.gameObject;
				}
				return null;
			}
		}
		return null;
	}
	
	public GameObject findNextPart() {
		foreach (PartScript part in parts) {
			if (!part.collected) {
				return part.gameObject;
			}
		}
		return null;
	}
}
