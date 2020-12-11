using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public Vector2Int Position;
    public float Value;
    public Room ParentRoom;
    
    private readonly Cell[,] _matrix;

    public Cell(Vector2Int vector2Int, float f, Cell[,] matrix)
    {
        Position = vector2Int;
        Value = f;
        _matrix = matrix;
        ParentRoom = null;
    }

    public void CheckNeighbours(Room room)
    {
        ParentRoom = room;
        ParentRoom.Cells.Add(this);
        List<Cell> neighbourCells = new List<Cell>();

        if (_matrix.GetLength(0) > Position.x + 1)
        {
            neighbourCells.Add(_matrix[Position.x + 1, Position.y]);
        }
        if (_matrix.GetLength(0) > Position.x - 1)
        {
            neighbourCells.Add(_matrix[Position.x - 1, Position.y]);
        }
        if (_matrix.GetLength(1) > Position.y + 1)
        {
            neighbourCells.Add(_matrix[Position.x, Position.y + 1]);
        }
        if (_matrix.GetLength(1) > Position.y - 1)
        {
            neighbourCells.Add(_matrix[Position.x, Position.y -1]);
        }
        
        foreach (var neighbourCell in neighbourCells.Where(neighbourCell => !(neighbourCell.Value > 0.5)).Where(neighbourCell => neighbourCell.ParentRoom == null))
        {
            neighbourCell.CheckNeighbours(ParentRoom);
        }
    }
}
