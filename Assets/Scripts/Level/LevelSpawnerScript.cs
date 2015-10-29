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
		nextLevelObject = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!lastLevelObject || 
		    transform.position.y - (lastLevelObject.transform.position.y + lastLevelObject.GetComponent<BoxCollider2D>().size.y) > levelDistance) {

			Vector3 nextPosition = transform.position;
			nextPosition.y += nextLevelObject.GetComponent<BoxCollider2D>().size.y / 2;

			lastLevelObject = Instantiate(nextLevelObject, nextPosition, Quaternion.identity) as GameObject;
			lastLevelObject.transform.SetParent(transform.parent);
			lastLevelObject.transform.localScale = transform.parent.localScale;

			nextLevelObject = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
		}
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
