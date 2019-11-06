using UnityEngine;

public class Block : MonoBehaviour {

	public float gravityFactor = 20f;

	void Start () {
		GetComponent<Rigidbody2D> ().gravityScale += Time.timeSinceLevelLoad / gravityFactor;
	}

	void Update () {
		if (transform.position.y < -2f)
			Destroy (gameObject);
	}
}
