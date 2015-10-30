using UnityEngine;
using System.Collections;

public class InventoryPartSpawnerScript : MonoBehaviour {
	[SerializeField]
	RocketBuilderScript rocketBuilder;
	[SerializeField]
	private float partDistance = 1f;
	private GameObject[] rocket;
	private int count = 0;

	private GameObject lastPartObject;

	// Use this for initialization
	void Start () {
		rocket = rocketBuilder.createRocket ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (count < rocket.Length && (!lastPartObject || 
		    transform.position.y - lastPartObject.transform.position.y > partDistance)) {
			
			lastPartObject = Instantiate(rocket[count], transform.position, Quaternion.identity) as GameObject;
			lastPartObject.transform.localScale = transform.parent.localScale;
			lastPartObject.transform.SetParent(transform.parent);
			
			count++;
		}
	}
}
