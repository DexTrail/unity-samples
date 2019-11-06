using System;

public class D_Tile {
	public struct Position {
		public int x;
		public int y;
	}

	public Position position;
	public TileTtype type = TileTtype.White;
	public float entryCost = 1f;
	public bool unreachable = false;

	public static int tileTtype_Lengh { get { return _tileTtype_Lengh; } }

	public enum TileTtype {
		Grey,
		Red,
		Black,
		White,
		Green,
		Yellow,
		Orange,
		Blue
	}

	private static int _tileTtype_Lengh;

	static D_Tile () {
		// Number of elements in enum
		_tileTtype_Lengh = Enum.GetValues (typeof (TileTtype)).Length;
	}

	public D_Tile (int x, int y, D_Tile tile) : this (x, y, tile.type, tile.entryCost, tile.unreachable) { }

	public D_Tile (int x, int y, TileTtype type, float entryCost, bool unreachable = false) : this (x, y, type) {
		this.entryCost = entryCost;
		this.unreachable = unreachable;
	}

	public D_Tile (int x, int y, TileTtype type) : this (x, y) {
		this.type = type;
	}

	private D_Tile (int x, int y) {
		this.position.x = x;
		this.position.y = y;
	}

	public D_Tile () { }
}
