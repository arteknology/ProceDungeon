using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public List<Cell> Cells = new List<Cell>();
    public Vector2Int center = Vector2Int.zero;

    public void Center()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            center += Cells[i].Position;
        }
        center = center / Cells.Count;
        //Debug.Log(center);
    }
}
