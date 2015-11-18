using UnityEngine;
using System.Collections;

public class TsunamiScript : MonoBehaviour {
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Rigidbody2D targetBody;
	[SerializeField]
	private Rigidbody2D myRigidbody;
	
	[SerializeField]
	float touchDistance;
	[SerializeField]
	float minTouchDistance;
	[SerializeField]
	float maxDistance;
	[SerializeField]
	float pushForce;
	
	[SerializeField]
	float speed;
	
	// Use this for initialization
	void Start () {
		myRigidbody.AddForce(Vector2.up * speed);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = target.position.y - transform.position.y;
		
		if (distance < minTouchDistance) {
			Vector2 position = target.position;
			position.y = transform.position.y + minTouchDistance;
			target.position = position;
		}
		
		if (distance < touchDistance) {
			targetBody.AddForce(Vector2.up * pushForce);
		}
		
		if (distance > maxDistance) {
			Vector2 position = transform.position;
			position.y = target.position.y - maxDistance;
			transform.position = position;
		} 
	}
}
