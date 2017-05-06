using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area {

    public string type;
    public List<Tile> tileInArea;

    public bool needRoof;


    public Area(string Type)
    {
        type = Type;
        tileInArea = new List<Tile>();

        needRoof = false;
    }
}
