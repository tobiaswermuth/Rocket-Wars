﻿using UnityEngine;
using System.Collections;

public class LevelPieceScript : MonoBehaviour {
	private float speed = 60f;
	[SerializeField]
	private GameObject[] possibleParts;

	// Use this for initialization
	void Start () {
		foreach(Transform child in transform){
			if (child.CompareTag("Part Placeholder")) {
				GameObject nextPartPrefab = possibleParts[Random.Range(0, possibleParts.Length)];
				if (Random.Range(0, 3) == 2) {
					nextPartPrefab = ShipScript.instance.findNextPart(ShipScript.instance.randomPlayerInventory());
				}

				GameObject part = Instantiate(nextPartPrefab, child.position, Quaternion.identity) as GameObject;
				part.transform.SetParent(transform);
				part.GetComponent<Rigidbody2D>().isKinematic = true;

				Destroy(child.gameObject);
			}
		}

		GetComponent<Rigidbody2D> ().AddForce (Vector2.down * speed);
	}

	public object[] getPaths() {
		ArrayList paths = new ArrayList ();
		foreach(Transform child in transform){
			if (child.CompareTag("Path")) {
				paths.Add(child.gameObject);
			}
		}
		return paths.ToArray ();
	}

	public float[] getNearestTwoPathXs(Vector3 position) {
		float[] pathXs = {position.x, position.x};

		foreach(GameObject path in getPaths()) {
			if (path.transform.position.x > position.x && (pathXs[1] == position.x || path.transform.position.x <= pathXs[1])) {
				pathXs[1] = path.transform.position.x;
			}
			if (path.transform.position.x < position.x && (pathXs[0] == position.x || path.transform.position.x >= pathXs[0])) {
				pathXs[0] = path.transform.position.x;
			}
		}

		return pathXs;
	}
}
