using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorChange : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public Tile freeTile;
    public Tile occupiedTile;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        foreach (Vector3 occupiedPos in OccupiedPositionsHandler.Instance.occupiedPositions)
        {
            Vector3Int gridPos = tilemap.WorldToCell(occupiedPos);
            tilemap.SetTileFlags(gridPos, TileFlags.None);
            tilemap.SetTile(gridPos, occupiedTile);
        }
    }

}
