﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public int XPos, Ypos;
    public float Value;
    public Room ParentRoom;
    
    private readonly Cell[,] _matrix;

    public Cell(Vector2Int vector2Int, float f, Cell[,] matrix)
    {
        XPos = vector2Int.x;
        Ypos = vector2Int.y;
        Value = f;
        _matrix = matrix;
        ParentRoom = null;
    }
}
