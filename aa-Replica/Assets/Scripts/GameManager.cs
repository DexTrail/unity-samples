using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public Rotator rotator;
	public Spawner spawner;
	public Animator animator;

	private bool gameHasEnded = false;

	void Start () {
		Input.backButtonLeavesApp = true;
	}

	public void EndGame () {
		if (gameHasEnded)
			return;

		rotator.enabled = false;
		spawner.enabled = false;
		animator.SetTrigger ("EndGame");

		gameHasEnded = true;
	}

	public void RestartLevel () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
