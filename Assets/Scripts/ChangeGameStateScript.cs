using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeGameStateScript : MonoBehaviour {
	public void toLobby() {
		SceneManager.LoadScene("Lobby");
	}

	public void startGame() {
		SceneManager.LoadScene("Game");
	}
}
