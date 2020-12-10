using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Random = System.Random;

public class Map : MonoBehaviour {

    public enum Generation {
        PerlinNoise,
        RandomNoise
    }
    
    [Header("Data")]
    public Tile GroundTile;
    public RuleTile WallTile;
    
    [Header("Parameters")] 
    public int MapXSize, MapYSize;
    [Range(0,1f)] public float Zoom;
    public bool AutoUpdate;

    [Header("References")] 
    public Camera Camera;
    public Tilemap WallMap;
    public Tilemap GroundMap;

    private Cell[,] _matrix;
    private int maxAutoUpdateSize = 100;
    private List<Room> rooms = new List<Room>();
    private List<Vector2Int> roomCenters = new List<Vector2Int>();


    private void FixedUpdate()
    {
        if (!AutoUpdate) return;
        if (MapXSize + MapYSize > maxAutoUpdateSize)
        {
            Debug.LogWarning("Too biiiiiiiiiiiiiiig");
        }
        else
        {
            Generate();
        }
    }

    [ContextMenu("Generate")]
    public void Generate() {
        ClearMap();
        BlocsGeneration();
        CreateRoom();
        ColorateRoom();
        //FindRoomCenter();
    }
    
    
    public void BlocsGeneration() {
        float[,] perlinMatrix = new float[MapXSize,MapYSize];
        perlinMatrix = NoiseUtils.PerlinCave(perlinMatrix, Zoom, 0, 0);
        _matrix = new Cell[MapXSize, MapYSize];
        for (int i = 0; i < MapXSize; i++) {
            for (int j = 0; j < MapYSize; j++) {
                float value = perlinMatrix[i, j];
                _matrix[i,j] = new Cell(new Vector2Int(i,j), value, _matrix);
                GroundMap.SetTile(new Vector3Int(i, j, 0),GroundTile);
                if(perlinMatrix[i, j] <= 0.5) continue;
                WallMap.SetTile(new Vector3Int(i, j, 0),WallTile);
            }
        }
    }
    
    public void ClearMap() {
        GroundMap.ClearAllTiles();
        WallMap.ClearAllTiles();
    }

    private void CreateRoom()
    {
        foreach (Cell cell in _matrix)
        {
            if (cell.ParentRoom != null) continue;
            if (cell.Value > 0.5) continue;
            Room currentRoom = new Room();
            cell.CheckNeighbours(currentRoom);
            rooms.Add(currentRoom);
        }
    }

    private void ColorateRoom()
    {
        foreach (Room room in rooms)
        {
            //Debug.Log(room.Cells.Count);
            foreach (Cell cell in room.Cells)
            {
                Vector3Int cellPos = new Vector3Int(cell.Position.x, cell.Position.y, 0);
                GroundMap.SetTile(cellPos, null);
            }
        }
    }

    private void FindRoomCenter()
    {
        foreach (Room room in rooms)
        {
            room.Center();
            roomCenters.Add(room.center);

            Vector3Int centerOfRoom = new Vector3Int(room.center.x, room.center.y, 0);
            GroundMap.SetTile(centerOfRoom, null);
        }
    }

}
