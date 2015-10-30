using UnityEngine;
using System.Collections;

public class PartScript : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	public string partIdentifier = "";
	[SerializeField]
	public float maxEnergy = 100f;

	private float energy = 0;

	// Use this for initialization
	void Start () {
		energy = maxEnergy;
	}
	
	// Update is called once per frame
	void Update () {
		Color color = spriteRenderer.color;
		color.g = energy / maxEnergy;
		color.b = energy / maxEnergy;
		spriteRenderer.color = color;
	}

	public float getEnergy() {
		return energy;
	}

	public void setEnergy(float newEnergy) {
		energy = newEnergy;
	}
}
