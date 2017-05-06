using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need {

    public NeedType type;
    public Human human;
    public Tile tile;

    public bool isDoing;


    public Need(NeedType Type, Human Human)
    {
        type = Type;
        human = Human;
        tile = null;

        isDoing = false;
    }
}
