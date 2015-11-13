using UnityEngine;
using System.Collections;

public class LevelSpawnerScript : MonoBehaviour {
	[SerializeField]
	private float levelDistance = 1f;
	[SerializeField]
	private GameObject[] levelPrefabs;
	private GameObject nextLevelObject;
	private GameObject lastLevelObject;

	// Use this for initialization
	void Start () {
		createLevelPiece(levelPrefabs[Random.Range(0, levelPrefabs.Length)], transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!lastLevelObject || 
		    transform.position.y - (lastLevelObject.transform.position.y + lastLevelObject.GetComponent<BoxCollider2D>().size.y/2) > levelDistance) {
				
			Vector3 nextPosition;
			if (lastLevelObject) {
				nextPosition = lastLevelObject.transform.position;
				nextPosition.y += lastLevelObject.GetComponent<BoxCollider2D>().size.y/2;
				nextPosition.y += levelDistance;
			} else {
				nextPosition = transform.position;
			}
			
			nextPosition.y += nextLevelObject.GetComponent<BoxCollider2D>().size.y / 2;

			createLevelPiece(nextLevelObject, nextPosition);
		}
	}

	private void createLevelPiece(GameObject prefab, Vector3 position) {
		lastLevelObject = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		
		nextLevelObject = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
	}

	public GameObject getNearestLevelPiece(Vector3 position) {
		LevelPieceScript[] levelPieces = transform.parent.GetComponentsInChildren<LevelPieceScript>();
		GameObject nearestLevelPiece = levelPieces[levelPieces.Length - 1].gameObject;
		foreach (LevelPieceScript levelPiece in levelPieces) {
			if (levelPiece.transform.position.y > position.y - levelPiece.GetComponent<BoxCollider2D>().size.y / 2) {
				if (levelPiece.transform.position.y < nearestLevelPiece.transform.position.y) {
					nearestLevelPiece = levelPiece.gameObject;
				} 
			}
		}
		return nearestLevelPiece.gameObject;
	}
}
