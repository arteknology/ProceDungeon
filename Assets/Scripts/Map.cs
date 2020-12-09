using System;
using System.Collections.Generic;
using System.Data;
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
    public string Seed;
    public bool AutoUpdate;

    [Header("References")] 
    public Camera Camera;
    public Tilemap WallMap;
    public Tilemap GroundMap;

    private float[,] _matrix;
    private int maxAutoUpdateSize;

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

    public void Generate() {
        ClearMap();
        SetupCamera();
        BlocsGeneration();
    }

    public void SetupCamera() {
        Camera.transform.position = new Vector3((float) MapXSize / 2, Camera.transform.position.y, (float) MapYSize / 2);
    }
    
    public void BlocsGeneration() {
        float[,] perlinMatrix = new float[MapXSize,MapYSize];
        perlinMatrix = NoiseUtils.PerlinCave(perlinMatrix, Zoom, 0, 0);
        _matrix = new Cell[MapXSize, MapYSize];
        for (int i = 0; i < MapXSize; i++) {
            for (int j = 0; j < MapYSize; j++) {
                float value = _matrix[i, j];
                GroundMap.SetTile(new Vector3Int(i, j, 0),GroundTile);
                if(perlinMatrix[i, j] <= 0.5) continue;
                WallMap.SetTile(new Vector3Int(i, j, 0),WallTile);
                _matrix[i,j] = new Cell(new Vector2Int(i,j), perlinMatrix[i,j], _matrix);
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
            
        }
    }
}
