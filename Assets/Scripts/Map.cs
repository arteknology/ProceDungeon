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
    }
    
    
    public void BlocsGeneration() {
        float[,] perlinMatrix = new float[MapXSize,MapYSize];
        perlinMatrix = NoiseUtils.PerlinCave(perlinMatrix, Zoom, 0, 0);
        _matrix = new Cell[MapXSize, MapYSize];
        for (int i = 0; i < MapXSize; i++) {
            for (int j = 0; j < MapYSize; j++) {
                float value = perlinMatrix[i, j];
                GroundMap.SetTile(new Vector3Int(i, j, 0),GroundTile);
                _matrix[i,j] = new Cell(new Vector2Int(i,j), value, _matrix);
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
        bool first = true;
        Vector3Int startCellPos = new Vector3Int();

        int iteration = 0;
        List<Room> rooms = new List<Room>();
        int roomNumber = 0;


        foreach (Cell cell in _matrix)
        {
            if (cell.Value <= 0.5 && iteration < _matrix.Length)
            {
                Debug.Log("GROUND / X pos = "+ cell.XPos + " Y Pos = " + cell.Ypos);

                //GET FIRST GROUND TILE / CREATE FIRST ROOM / ADD IT AS PARENT OF THE CELL / PUT IT IN ROOMS LIST
                if (first)
                {
                    Cell startCell = cell;
                    startCellPos = new Vector3Int(startCell.XPos, startCell.Ypos, 0);
                    
                    Room firstRoom = new Room(roomNumber);
                    firstRoom.Cells.Add(startCell);
                    rooms.Add(firstRoom);
                    
                    first = false;
                }

                //DELETE FIRST GROUND TILE TO KNOW WHERE IT IS
                Tilemap ground = GroundMap.GetComponent<Tilemap>();
                ground.SetTile(startCellPos, null);

                //FindNeighbours(cell);
            }
        }
    }

    
    /*public void FindNeighbours(Cell cell)
    {
        Cell rightCell = cell;
        rightCell.XPos = rightCell.XPos + 1;
        
        Cell leftCell = cell;
        leftCell.XPos = leftCell.XPos - 1;
        
        Cell upCell = cell;
        upCell.Ypos = upCell.Ypos + 1;
        
        Cell downCell = cell;
        downCell.Ypos = downCell.Ypos - 1;
    }*/
}
