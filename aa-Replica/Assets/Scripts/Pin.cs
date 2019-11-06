using UnityEngine;

public class Pin : MonoBehaviour {
	public float speed = 20f;
	public Rigidbody2D rb;

	private bool isPinned = false;

	void Update () {
		if (!isPinned)
			rb.MovePosition (rb.position + Vector2.up * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.tag == "Rotator") {
			transform.SetParent (collision.transform);
			Score.pinCount++;
			isPinned = true;
		} else if (collision.tag == "Pin") {
			FindObjectOfType<GameManager> ().EndGame ();
		}
	}
}
