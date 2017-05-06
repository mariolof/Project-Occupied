using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public List<Tile> tileInRoom;
    public List<Tile> entrence;

    public bool hasRoof;

    public Room()
    {
        tileInRoom = new List<Tile>();
        entrence = new List<Tile>();

        hasRoof = false;
    }
}
