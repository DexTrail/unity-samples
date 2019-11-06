using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_TilePanel : MonoBehaviour {
	public G_Map map;
	[Header ("Panel elements")]
	public Text headerText;
	public Dropdown typeDropdown;
	public InputField entryCostInputField;
	public Toggle unreachableToggle;
	[Space]
	public float panelOffset = 5f;

	D_Tile selectedTile = null;
	RectTransform rectTransform;
	Camera mainCam;

	// Blocks input when editing child element
	bool blockInput = false;

	private void Awake () {
		rectTransform = GetComponent<RectTransform> ();

		mainCam = Camera.main;

		// Initializing typeDropdown
		List<string> typeList = new List<string> ();
		typeList.AddRange (Enum.GetNames (typeof (D_Tile.TileTtype)).Cast<string> ());
		typeDropdown.ClearOptions ();
		typeDropdown.AddOptions (typeList);
	}

	void Update () {
		// [Keyboard: Enter, NumEnter]
		if ((Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter)) && !blockInput)
			ClosePanel (true);

		// [Keyboard: Escape]
		if (Input.GetKeyDown (KeyCode.Escape) && !blockInput)
			ClosePanel (false);

		// Block input to prevent handle Enter and Escape pressings
		// when editing InputField or DropDown list is opened
		blockInput = entryCostInputField.isFocused || typeDropdown.transform.childCount > 3;
	}

	public void ClosePanel (bool aplySettings = false) {
		if (aplySettings) {
			selectedTile.type = (D_Tile.TileTtype)typeDropdown.value;
			selectedTile.entryCost = Mathf.Round (float.Parse (entryCostInputField.text) * 10) / 10;
			selectedTile.unreachable = unreachableToggle.isOn;
		}

		gameObject.SetActive (false);
	}

	public void ValidateCost (bool onEnd = false) {
		entryCostInputField.text = entryCostInputField.text.TrimStart (("-").ToCharArray ());

		float f;
		bool parsed = float.TryParse (entryCostInputField.text, out f);
		if (!parsed && onEnd == true) {
			entryCostInputField.text = selectedTile.entryCost.ToString ("F1");
			return;
		}

		if (f > 9999.9f)
			entryCostInputField.text = (9999.9f).ToString ();
	}

	public void ValidateUnreachable (bool state) {
		entryCostInputField.interactable = !state;
	}

	void OnEnable () {
		selectedTile = map.GetSelectedTile ();
		if (selectedTile == null) {
			gameObject.SetActive (false);
			return;
		}

		// Initializing elements with selectedTile parameters...
		headerText.text = string.Format ("Tile {0}, {1}", selectedTile.position.x, selectedTile.position.y);

		typeDropdown.value = (int)selectedTile.type;

		entryCostInputField.text = selectedTile.entryCost.ToString ("F1");

		bool unreachable = selectedTile.unreachable;
		entryCostInputField.interactable = !unreachable;

		unreachableToggle.isOn = unreachable;
		// ...Initializing elements with selectedTile parameters

		// UNDONE Placement
		Vector3 mousePosition = Input.mousePosition;

		//* Initializing variables for positioning...
		
		// Array of corner points of rectTransform in world coordinates (clockwise from bottom-left)
		// Works incorrect in Awake ()
		Vector3[] panelCorners = new Vector3[4];
		rectTransform.GetWorldCorners (panelCorners);

		// Half-diagonal
		Vector3 panelHalfSize = (panelCorners[2] - panelCorners[0]) / 2f;
		// Panel size + offset
		Vector2 spaceNeeded = new Vector2 (panelHalfSize.x * 2f + panelOffset, panelHalfSize.y * 2f + panelOffset);
		// Half-diagonal + offset
		Vector2 panelCenterPoint = new Vector2 (panelHalfSize.x + panelOffset, panelHalfSize.y + panelOffset);
		//* ...Initializing variables for positioning

		Vector3 finalPos = new Vector3 (0, 0, 0);
		if (mousePosition.y >= spaceNeeded.y)
			if (mousePosition.x <= mainCam.pixelWidth - spaceNeeded.x) {
				Debug.Log ("Bottom-right");
				finalPos.x = mousePosition.x + panelCenterPoint.x;
				finalPos.y = mousePosition.y - panelCenterPoint.y;
			} else if (mousePosition.x > spaceNeeded.x) {
				Debug.Log ("Bottom-left");
				finalPos.x = mousePosition.x - panelCenterPoint.x;
				finalPos.y = mousePosition.y - panelCenterPoint.y;
			} else
				// IMPLEMENT
				Debug.Log ("Screen too small (1)");
		else if (mousePosition.y < mainCam.pixelHeight - spaceNeeded.y)
			if (mousePosition.x <= mainCam.pixelWidth - spaceNeeded.x) {
				Debug.Log ("Top-right");
				finalPos.x = mousePosition.x + panelCenterPoint.x;
				finalPos.y = mousePosition.y + panelCenterPoint.y;
			} else if (mousePosition.x > spaceNeeded.x) {
				Debug.Log ("Top-left");
				finalPos.x = mousePosition.x - panelCenterPoint.x;
				finalPos.y = mousePosition.y + panelCenterPoint.y;
			} else
				// IMPLEMENT
				Debug.Log ("Screen too small (2)");
		else {
			// IMPLEMENT
			Vector2 v0 = new Vector2 (spaceNeeded.x, spaceNeeded.y);
			Vector2 v1 = new Vector2 (mainCam.pixelWidth - spaceNeeded.x, mainCam.pixelHeight - spaceNeeded.y);
			Debug.Log ("Screen too small (3)" +
						"\n" + v0.ToString () + " - " + v1.ToString ());
			//Debug.DrawLine (new Vector3 (v0.x, v0.y), new Vector3 (v1.x, v1.y), Color.red, 60f);

			//
		}

		transform.position = finalPos;
		//transform.position = mousePosition;
		//transform.position = new Vector3 ((float)mainCam.pixelWidth / 2f, (float)mainCam.pixelHeight / 2f);


		//Debug.DrawLine (transform.position - new Vector3 (panelCenterPoint.x, panelCenterPoint.y),
		//				transform.position + new Vector3 (panelCenterPoint.x, panelCenterPoint.y),
		//				Color.blue, 60f);
		//Debug.DrawLine (new Vector3 (transform.position.x - rectTransform.rect.width, transform.position.y - rectTransform.rect.height, 0f),
		//				new Vector3 (transform.position.x + rectTransform.rect.width, transform.position.y + rectTransform.rect.height, 0f),
		//				Color.yellow, 60f);
		//Debug.DrawLine (panelCorners[0], panelCorners[2], Color.yellow, 60f);

		Debug.Log ("mainCam: <b>" + mainCam.pixelWidth + ", " + mainCam.pixelHeight +
			"</b>  || mousePosition: <color=blue>" + mousePosition.x + ", " + mousePosition.y + "</color>\n" +
			"rectTransform size: <color=green>" + rectTransform.rect.width + ", " + rectTransform.rect.height + "</color>" +
			"  ||  transform.position: <color=blue>" + transform.position.ToString () + "</color>" +
			"  ||  localScale: ");

		Debug.Log ("OnEnable return");
	}
}
