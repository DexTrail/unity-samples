using UnityEngine;

public class BlockSpawner : MonoBehaviour {

	public GameObject blockPrefab;
	public Transform[] spawnPoints;

	public float timeBetweenWawes = 1f;

	private float timeToSpawn = 2f;

	void Update () {
		if (timeToSpawn <= Time.time) {
			SpawnBlocks ();
			timeToSpawn = Time.time + timeBetweenWawes;
		}
	}

	void SpawnBlocks () {
		int randomIndex = Random.Range (0, spawnPoints.Length);

		for (int i = 0; i < spawnPoints.Length; i++) {
			if (i != randomIndex) {
				Instantiate (blockPrefab, spawnPoints[i].position, Quaternion.identity);
			}
		}
	}
}
