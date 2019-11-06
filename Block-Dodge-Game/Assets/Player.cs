﻿using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 15f;
	public float mapWidth = 5f;

	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		float x = Input.GetAxis ("Horizontal") * speed * Time.fixedDeltaTime;

		Vector2 newPosition = rb.position + x * Vector2.right;
		newPosition.x = Mathf.Clamp (newPosition.x, -mapWidth, mapWidth);

		rb.MovePosition (newPosition);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		FindObjectOfType<GameManager> ().EndGame ();
	}
}