using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeGameStateScript : MonoBehaviour {
	public void backToMenu() {
		SceneManager.LoadScene("Start");
	}

	public void startGame() {
		SceneManager.LoadScene("Game");
	}

	public void endGame() {
		SceneManager.LoadScene("End");
	}
}
