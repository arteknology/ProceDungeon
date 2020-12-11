using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
    public int Xoffset;
    public int Yoffset;

    private Cell[,] _matrix;
    private int maxAutoUpdateSize = 100;
    private List<Room> rooms = new List<Room>();
    private float[,] Network;

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
        CreateNetwork();
    }


    private void BlocsGeneration() {
        float[,] perlinMatrix = new float[MapXSize,MapYSize];
        perlinMatrix = NoiseUtils.PerlinCave(perlinMatrix, Zoom, Xoffset, Yoffset);
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

    private void ClearMap() {
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

    private void CreateNetwork()
    {
        Network = new float[rooms.Count, rooms.Count];
        for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = 0; j < rooms.Count; j++)
                {

                }
            }
        }
}

