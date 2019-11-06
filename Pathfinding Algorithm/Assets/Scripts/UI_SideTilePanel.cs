using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SideTilePanel : MonoBehaviour {
	public G_Map map;
	[Header ("Panel elements")]
	public Image tileImage;
	public Dropdown typeDropdown;
	public InputField entryCostInputField;
	public Toggle unreachableToggle;

	D_Tile inputTile = null;
	Sprite[] sprite;

	void Start () {
		inputTile = map.inputTile;

		// Initializing tile image sprites...
		Texture2D texTerrainTiles = map.texTerrainTiles;
		int texTileResolution = texTerrainTiles.height;
		int texTileSpriteNum = texTerrainTiles.width / texTerrainTiles.height;

		sprite = new Sprite[texTileSpriteNum];
		for (int i = 0; i < texTileSpriteNum; i++) {
			Rect rect = new Rect (texTileResolution * i, 0f, texTileResolution, texTileResolution);
			sprite[i] = Sprite.Create (texTerrainTiles, rect, new Vector2 (.5f, .5f));
		}
		// ...

		// Initializing typeDropdown
		List<string> typeList = new List<string> ();
		typeList.AddRange (Enum.GetNames (typeof (D_Tile.TileTtype)).Cast<string> ());
		typeDropdown.ClearOptions ();
		typeDropdown.AddOptions (typeList);

		// Initializing elements with default tile parameters...
		int type = (int)inputTile.type;
		bool unreachable = inputTile.unreachable;

		tileImage.sprite = sprite[type];
		typeDropdown.value = type;
		entryCostInputField.text = inputTile.entryCost.ToString ("F1");
		entryCostInputField.interactable = !unreachable;
		unreachableToggle.isOn = unreachable;
		// ...
	}

	public void TypeChanged (int value) {
		inputTile.type = (D_Tile.TileTtype)value;
		tileImage.sprite = sprite[value];
	}

	public void ValidateCost (bool onEnd = false) {
		entryCostInputField.text = entryCostInputField.text.TrimStart (("-").ToCharArray ());

		float f;
		bool parsed = float.TryParse (entryCostInputField.text, out f);
		if (!parsed && onEnd == true) {
			entryCostInputField.text = inputTile.entryCost.ToString ("F1");
			return;
		}

		if (f > 9999.9f)
			entryCostInputField.text = (9999.9f).ToString ();

		if (onEnd)
			inputTile.entryCost = Mathf.Round (f * 10) / 10;
	}

	public void ValidateUnreachable (bool state) {
		inputTile.unreachable = state;
		entryCostInputField.interactable = !state;
	}
}
