using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public List<Cell> Cells = new List<Cell>();

    private Vector2Int Center
    {
        get
        {
            Vector2Int sum = Cells.Aggregate(Vector2Int.zero, (current, cell) => current + cell.Position);
            return new Vector2Int(Mathf.RoundToInt((float)sum.x / Cells.Count), Mathf.RoundToInt((float)sum.y/Cells.Count));
        }
    }

    public Cell CenterCell
    {
        get
        {
            Vector2Int center = Center;
            Cell centerCell = null;
            float shorterDistance = Mathf.Infinity;

            foreach (var cell in Cells.Where(cell => Vector2Int.Distance(center, cell.Position) < shorterDistance))
            {
                shorterDistance = Vector2Int.Distance(center, cell.Position);
                centerCell = cell;
            }
            return centerCell;
        }
    }
}
