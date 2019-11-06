using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshCollider))]
public class G_Map : MonoBehaviour {
	[Header ("Map size")]
	public int size_x = 100;
	public int size_z = 50;
	public float tileSize = 1.0f;

	[Header ("Map texture")]
	public Texture2D texTerrainTiles;
	public FilterMode filterMode = FilterMode.Bilinear;
	public D_Tile.TileTtype highlightTex = D_Tile.TileTtype.White;

	[Header ("Default tile")]
	public D_Tile.TileTtype defTileType = D_Tile.TileTtype.Grey;
	public float defEntryCost = 1f;
	public bool defUnreachable = false;

	[HideInInspector]
	public D_Tile inputTile;

	int texTileResolution;
	float texTileProportion;
	Mesh mesh;

	D_Map tdMap;
	D_Tile selectedTile = null;

	public void Awake () {
		texTileResolution = texTerrainTiles.height;
		texTileProportion = (float)texTileResolution / texTerrainTiles.width;

		mesh = new Mesh ();

		tdMap = new D_Map (size_x, size_z, new D_Tile (0, 0, defTileType, defEntryCost, defUnreachable));
		if (tdMap == null)
			throw new System.Exception ("Unable to create tdMap.");

		BuildMesh ();

		inputTile = tdMap.defaultTile;
	}


	void BuildMesh () {
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numTiles = size_x * size_z;
		int numPoints = vsize_x * vsize_z;
		int numVerts = numTiles * 4;
		int numTris = numTiles * 2;

		// Generate the mesh data
		Vector3[] points = new Vector3[numPoints];
		Vector3[] vertices = new Vector3[numVerts];
		int[] triangles = new int[numTris * 3];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		int x, z;
		// Generate raw mesh
		for (z = 0; z < vsize_z; z++)
			for (x = 0; x < vsize_x; x++) {
				int index = z * vsize_x + x;

				points[index] = new Vector3 (x * tileSize, 0, z * tileSize);
			}

		// Generate render mesh
		for (z = 0; z < size_z; z++)
			for (x = 0; x < size_x; x++) {
				int tileOffset = z * size_x + x;
				int pointOffset = z * vsize_x + x;
				int vertOffset = tileOffset * 4;
				int triOffset = tileOffset * 6;

				// Vertices
				vertices[vertOffset + 0] = points[pointOffset];
				vertices[vertOffset + 1] = points[pointOffset + vsize_x];
				vertices[vertOffset + 2] = points[pointOffset + vsize_x + 1];
				vertices[vertOffset + 3] = points[pointOffset + 1];

				// Triangles
				triangles[triOffset + 0] = vertOffset + 0;
				triangles[triOffset + 1] = vertOffset + 1;
				triangles[triOffset + 2] = vertOffset + 2;

				triangles[triOffset + 3] = vertOffset + 0;
				triangles[triOffset + 4] = vertOffset + 2;
				triangles[triOffset + 5] = vertOffset + 3;

				// Normals
				normals[vertOffset + 0] = Vector3.up;
				normals[vertOffset + 1] = Vector3.up;
				normals[vertOffset + 2] = Vector3.up;
				normals[vertOffset + 3] = Vector3.up;
			}

		uv = FillTexture ();

		// Create mesh
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		// Assign mesh
		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		MeshCollider mesh_collider = GetComponent<MeshCollider> ();

		mesh_filter.mesh = mesh;
		mesh_renderer.sharedMaterial.mainTexture = texTerrainTiles;
		mesh_collider.sharedMesh = mesh;

		// Assign texture
		texTerrainTiles.filterMode = filterMode;
		texTerrainTiles.Apply ();
	}

	// Change currently selected tile parameters to input tile parameters
	public void ChangeCurrentTile () {
		if (selectedTile == null)
			return;
		selectedTile.type = inputTile.type;
		selectedTile.entryCost = inputTile.entryCost;
		selectedTile.unreachable = inputTile.unreachable;
	}

	public void ClearTileSelection () {
		if (selectedTile != null)
			PaintTile (selectedTile, selectedTile.type);
		selectedTile = null;
	}

	Vector2[] FillTexture () {
		if (texTerrainTiles.width / texTileResolution != D_Tile.tileTtype_Lengh)
			throw new System.Exception ("Incorrect size of tile texture.");

		Vector2[] uv = new Vector2[size_x * size_z * 4];

		for (int z = 0; z < size_z; z++)
			for (int x = 0; x < size_x; x++) {
				int tileOffset = z * size_x + x;
				int vertOffset = tileOffset * 4;
				int texOffset = (int)tdMap.GetTile (x, z).type;

				// UVs
				uv[vertOffset + 0] = new Vector2 (texOffset * texTileProportion, 0);
				uv[vertOffset + 1] = new Vector2 (texOffset * texTileProportion, 1);
				uv[vertOffset + 2] = new Vector2 ((texOffset + 1) * texTileProportion, 1);
				uv[vertOffset + 3] = new Vector2 ((texOffset + 1) * texTileProportion, 0);
			}

		return uv;
	}

	public D_Tile GetSelectedTile () { return selectedTile; }

	public D_Tile GetTileByWorldCoords (Vector3 worldCoords) {
		Vector3 localCoords = gameObject.transform.InverseTransformPoint (worldCoords);
		int tileX = Mathf.FloorToInt (localCoords.x / tileSize);
		int tileY = Mathf.FloorToInt (localCoords.z / tileSize);

		return tdMap.GetTile (tileX, tileY);
	}

	void PaintTile (D_Tile tile, D_Tile.TileTtype paintTex) {
		Vector2[] uv = mesh.uv;

		int vertOffset = (tile.position.y * size_x + tile.position.x) * 4;
		int texOffset = (int)paintTex;

		// UVs
		uv[vertOffset + 0] = new Vector2 (texOffset * texTileProportion, 0);
		uv[vertOffset + 1] = new Vector2 (texOffset * texTileProportion, 1);
		uv[vertOffset + 2] = new Vector2 ((texOffset + 1) * texTileProportion, 1);
		uv[vertOffset + 3] = new Vector2 ((texOffset + 1) * texTileProportion, 0);

		mesh.uv = uv;
	}

	public void SelectTile (D_Tile tile) {
		if (selectedTile != null)
			PaintTile (selectedTile, selectedTile.type);
		PaintTile (tile, highlightTex);
		selectedTile = tile;
	}
}
