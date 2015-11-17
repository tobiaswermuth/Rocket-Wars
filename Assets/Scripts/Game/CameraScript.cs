using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	[SerializeField]
	private GameObject target;

	private float yOffset;

	// Use this for initialization
	void Start () {
		yOffset = transform.position.y - target.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		position.y = target.transform.position.y + yOffset;
		transform.position = position;
	}
}
