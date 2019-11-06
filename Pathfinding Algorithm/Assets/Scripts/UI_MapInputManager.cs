using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (G_Map))]
public class UI_MapInputManager : MonoBehaviour {
	public float moveSpeed = 50f;
	public float moveMouseSpeed = 1f;
	public float zoomSpeed = 2f;
	[Space]
	public UI_TilePanel tilePanel;

	G_Map map;
	new Collider collider;

	Camera mainCam;

	void Start () {
		collider = GetComponent<Collider> ();
		map = GetComponent<G_Map> ();

		mainCam = Camera.main;
	}

	void Update () {
		// Forbid inputs if mouse cursor over UI elements
		if (EventSystem.current.IsPointerOverGameObject ())
			return;

		// Forbid inputs if tile panel is opened
		if (tilePanel.gameObject.activeInHierarchy)
			return;

		// Get mouse position on the object...
		Vector3 mousePosition = Input.mousePosition;
		Ray ray = mainCam.ScreenPointToRay (mousePosition);
		RaycastHit hitInfo;
		// and select tile
		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			D_Tile tile = map.GetTileByWorldCoords (hitInfo.point);
			if (tile != null)
				map.SelectTile (tile);
		} else {
			map.ClearTileSelection ();
		}


		// [Input axis]
		// Move screen by keyboard
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		if (horizontal != 0f || vertical != 0f) {
			mainCam.transform.Translate (horizontal * moveSpeed * Time.deltaTime, 0f, vertical * moveSpeed * Time.deltaTime, Space.World);
		}

		// [Mouse wheel]
		// Zoom by mouse
		if (Input.GetAxis ("Mouse ScrollWheel") != 0f) {
			Vector2 scrollVector2 = Input.mouseScrollDelta;

			// "+" sign - move "object" towards camera
			// "-" sign - move camera towards "object"
			mainCam.fieldOfView = Mathf.Clamp (mainCam.fieldOfView - scrollVector2.y * zoomSpeed, 10f, 90f);
		}

		// [LMB]
		// Change currently selected tile
		if (Input.GetMouseButton (0)) {
			map.ChangeCurrentTile ();
		}

		// [RMB]
		// Show tile panel
		if (Input.GetMouseButtonDown (1)) {
			if (map.GetSelectedTile () != null)
				tilePanel.gameObject.SetActive (true);
		}

		// [MMB]
		// Move screen by mouse
		/// HACK Bad behaviour on mouse dragging the view
		if (Input.GetMouseButton (2)) {
			float mouseX = Input.GetAxisRaw ("Mouse X");
			float mouseY = Input.GetAxisRaw ("Mouse Y");

			if (mouseX != 0f || mouseY != 0f) {
				float screenResolutionFactor = 750f / mainCam.pixelWidth;
				// "+" sign - move camera
				// "-" sign - move "object"
				mainCam.transform.Translate (-mouseX * moveMouseSpeed * screenResolutionFactor,
											0f,
											-mouseY * moveMouseSpeed * screenResolutionFactor,
											Space.World);
			}
		}
	}
}
