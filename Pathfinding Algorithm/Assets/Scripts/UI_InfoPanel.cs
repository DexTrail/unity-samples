using UnityEngine;
using UnityEngine.UI;

public class UI_InfoPanel : MonoBehaviour {
	public G_Map map;
	public Text statsText;

	D_Tile selectedTile = null;

	string[,] scoreTextFields = new string[4, 2];

	void Start () {
		SetStatsText ();
	}

	void LateUpdate () {
		SetStatsText ();
	}

	void SetStatsText () {
		D_Tile tile = map.GetSelectedTile ();

		if (tile != selectedTile) {
			selectedTile = tile;

			if (selectedTile != null) {
				statsText.text = string.Format ("Tile: {0}, {1}\n", selectedTile.position.x, selectedTile.position.y);
				statsText.text += string.Format ("Type: {0}\n", selectedTile.type);

				if (!selectedTile.unreachable)
					statsText.text += string.Format ("Entry cost: {0:F1}", selectedTile.entryCost);
				else
					statsText.text += "Unreachable";

			} else {
				for (int i = 0; i < scoreTextFields.GetLength (0); i++) {
					statsText.text = "Tile:\n";
					statsText.text += "Type:\n";
					statsText.text += "Entry cost:";
				}
			}
		}
	}
}
