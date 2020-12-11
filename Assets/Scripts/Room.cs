using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public List<Cell> Cells = new List<Cell>();

    public Vector2Int Center
    {
        get
        {
            Vector2Int sum = Vector2Int.zero;
            foreach (Cell cell in Cells)
            {
                sum += cell.Position;
            }
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

            foreach (Cell cell in Cells)
            {
                if (centerCell == null || Vector2Int.Distance(center, cell.Position) < shorterDistance)
                {
                    shorterDistance = Vector2Int.Distance(center, cell.Position);
                    centerCell = cell;
                }
            }
            return centerCell;
        }
    }
}
