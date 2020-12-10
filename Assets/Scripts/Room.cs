using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public List<Cell> Cells = new List<Cell>();
    public readonly int RoomNumber;
    public Room(int number)
    {
        RoomNumber = number;
    }
}
