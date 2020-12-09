using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public float xPos, Ypos;
    public float Value;
    public Room ParentRoom;
    
    private readonly Cell[,] _matrix;

    public Cell(Vector2Int vector2Int, float f, float[,] matrix)
    {
        throw new System.NotImplementedException();
    }
}
