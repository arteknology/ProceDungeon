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
// ReSharper disable All

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
    public float Xoffset;
    public float Yoffset;

    private Cell[,] _matrix;
    private int maxAutoUpdateSize = 100;
    private List<Room> rooms = new List<Room>();
    private float[,] Network;
    private Graph.Edge[] _edges;
    private bool Blocks = false;
    
    /*private void FixedUpdate()
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
    }*/

    [ContextMenu("Generate")]
    public void Generate()
    {
        Blocks = false;
        ClearMap();
        BlocsGeneration();
        if (Blocks)
        {
            CreateRoom();
            CreateNetwork();
            CreateMST();
            CreateCorridor();
        }
        FillTiles();
    }


    private void BlocsGeneration() {
        float[,] perlinMatrix = new float[MapXSize,MapYSize];
        perlinMatrix = NoiseUtils.PerlinCave(perlinMatrix, Zoom, Xoffset, Yoffset);
        _matrix = new Cell[MapXSize, MapYSize];
        for (int i = 0; i < MapXSize; i++) {
            for (int j = 0; j < MapYSize; j++) {
                float value = perlinMatrix[i, j];
                _matrix[i,j] = new Cell(new Vector2Int(i,j), value, _matrix);
            }
        }

        Blocks = true;
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
        for (int i = 0; i < rooms.Count; i++) {
            for (int j = 0; j < rooms.Count; j++)
            {
                Network[i, j] = Vector2Int.Distance(rooms[i].CenterCell.Position, rooms[j].CenterCell.Position);
            }
        }
    }

    private void CreateMST()
    {
        int V = rooms.Count;
        int E = Network.Length;
        Graph graph = new Graph(V, E);
        
        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                graph.edge[i + j].src = i;
                graph.edge[i + j].dest = j;
                graph.edge[i + j].weight = (int) Network[i, j];
            }
        }
        _edges = graph.KruskalMST();
    }

    private void CreateCorridor()
    {
        foreach (Graph.Edge edge in _edges)
        {
         Cell srcCell = rooms[edge.src].CenterCell;
         Cell destCell = rooms[edge.dest].CenterCell;

         int distX = destCell.Position.x - srcCell.Position.x;
         int distY = destCell.Position.y - srcCell.Position.y;

         if (distX > 0)
         {
             for (int i = srcCell.Position.x; i < destCell.Position.x; i++)
             {
                 _matrix[i, srcCell.Position.y].Value = -1;
             }
         }
         else
         {
             for (int i = srcCell.Position.x; i > destCell.Position.x; i--)
             {
                 _matrix[i, srcCell.Position.y].Value = -1;
             }
         }

         if (distY > 0)
         {
             for (int j = srcCell.Position.y; j < destCell.Position.y; j++)
             {
                 _matrix[destCell.Position.x, j].Value = -1;
             }
         }
         else
         {
             for (int j = srcCell.Position.y; j > destCell.Position.y; j--)
             {
                 _matrix[destCell.Position.x, j].Value = -1;
             }
         }
        }
    }

    private void FillTiles()
    {
        foreach (Cell cell in _matrix)
        {
            Vector3Int cellPos = new Vector3Int(cell.Position.x, cell.Position.y, 0);
            if (cell.Value < 0.5)
            {
                GroundMap.SetTile(cellPos,GroundTile);
            }

            if (cell.Value >= 0.5)
            {
                WallMap.SetTile(cellPos,WallTile);
            }
        }
    
    }
    
}

