using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorChange : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public Tile occupiedTile;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        OccupiedPositionsHandler.Instance.OnOccupiedChanged += ChangeColor;
    }

    private void OnDisable() => OccupiedPositionsHandler.Instance.OnOccupiedChanged -= ChangeColor;

    private void ChangeColor(Vector3 pos)
    {
        HashSet<Vector3> positions = OccupiedPositionsHandler.Instance.occupiedPositions;
        Vector3Int gridPos = tilemap.WorldToCell(pos);
        tilemap.SetTileFlags(gridPos, TileFlags.None);
        if (positions.Contains(pos))
        {
            tilemap.SetTile(gridPos, occupiedTile);
        }
        else tilemap.SetTile(gridPos, null);

    }

    private void Update()
    {
        foreach (Vector3 occupiedPos in OccupiedPositionsHandler.Instance.occupiedPositions)
        {
            Vector3Int gridPos = tilemap.WorldToCell(occupiedPos);
            tilemap.SetTileFlags(gridPos, TileFlags.None);
            if (OccupiedPositionsHandler.Instance.IsPositionOccupied(occupiedPos))
            {
                tilemap.SetTile(gridPos, occupiedTile);
            }
        }
    }

}
