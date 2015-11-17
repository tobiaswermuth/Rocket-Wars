using UnityEngine;
using System.Collections;

public class ChangeGameStateScript : MonoBehaviour {
	public void backToMenu() {
		Application.LoadLevel("Start");
	}
	
	public void startGame() {
		Application.LoadLevel("Game");
	}
	
	public void endGame() {
		Application.LoadLevel("End");
	}
}
