using UnityEngine;
using System.Collections;

public class PartScript : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	public string partIdentifier = "";

	public bool collected = false;

	void Update () {
		if (collected) {
			spriteRenderer.color = new Color(0, 1, 0);
		} else {
			spriteRenderer.color = new Color(1, 1, 1);
		}
	}
}
