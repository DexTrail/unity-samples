public class D_Map {
	public D_Tile[,] tiles;
	public int width { get { return _width; } }
	public int height { get { return _height; } }

	// Special tiles
	public D_Tile defaultTile { get { return _defaultTile; } }
	public D_Tile startTile { get { return _startTile; } set { _startTile = value; } }
	public D_Tile targetTile { get { return _targetTile; } set { _targetTile = value; } }

	private int _width;
	private int _height;

	private D_Tile _defaultTile = new D_Tile ();
	private D_Tile _startTile;
	private D_Tile _targetTile;

	public D_Map (int width, int height, D_Tile defaultTile) {
		_width = width;
		_height = height;

		if (defaultTile == null) {
			_defaultTile.type = D_Tile.TileTtype.Grey;
			_defaultTile.entryCost = 1f;
			_defaultTile.unreachable = false;
		} else {
			_defaultTile.type = defaultTile.type;
			_defaultTile.entryCost = defaultTile.entryCost;
			_defaultTile.unreachable = defaultTile.unreachable;
		}

		tiles = new D_Tile[_width, _height];

		GenerateMapData ();
	}

	public D_Map (int width, int height) : this (width, height, null) { }

	private void GenerateMapData () {

		for (int y = 0; y < _height; y++)
			for (int x = 0; x < _width; x++) {
				tiles[x, y] = new D_Tile (x, y, _defaultTile);
			}
	}

	public D_Tile GetTile (int x, int y) {
		if (x < 0 || x >= _width || y < 0 || y >= _height)
			return null;

		return tiles[x, y];
	}
}
