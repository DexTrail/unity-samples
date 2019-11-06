using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (TileMap))]
public class TileMapMouse : MonoBehaviour {
	public Transform selectionCube;

	MeshRenderer meshRenderer;
	new Collider collider;
	TileMap tileMap;
	Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
		//meshRenderer = GetComponent<MeshRenderer> ();
		collider = GetComponent<Collider> ();
		tileMap = GetComponent<TileMap> ();

		cameraOffset = Camera.main.transform.position - selectionCube.position;

		selectionCube.localScale = new Vector3 (tileMap.tileSize, 1f, tileMap.tileSize);
		selectionCube.rotation = transform.rotation;
		selectionCube.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			Vector3 hitPoint = transform.InverseTransformPoint (hitInfo.point);
			int x = Mathf.FloorToInt (hitPoint.x / tileMap.tileSize);
			int z = Mathf.FloorToInt (hitPoint.z / tileMap.tileSize);
			//Debug.Log ("Tile: " + x + ", " + z + "\n" + hitInfo.point);

			selectionCube.transform.position = transform.TransformPoint ((x + .5f) * tileMap.tileSize, 0, (z + .5f) * tileMap.tileSize);
			selectionCube.gameObject.SetActive (true);

			if (Input.GetMouseButton (1))
				Camera.main.transform.position = selectionCube.transform.position + cameraOffset;
		} else {
			selectionCube.gameObject.SetActive (false);
		}


		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			Camera.main.transform.Translate (new Vector3 (0, 0, 5f), Space.Self);
			cameraOffset = Camera.main.transform.position - selectionCube.position;
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			Camera.main.transform.Translate (new Vector3 (0, 0, -5f), Space.Self);
			cameraOffset = Camera.main.transform.position - selectionCube.position;
		}
	}
}
