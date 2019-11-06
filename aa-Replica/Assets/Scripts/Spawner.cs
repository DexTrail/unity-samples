﻿using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject pinPrefab;

	void Update () {
		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Jump") || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began))
			SpawnPin ();
	}

	void SpawnPin () {
		Instantiate (pinPrefab, transform.position, transform.rotation);
	}
}
