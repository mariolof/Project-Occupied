using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Grid grid;

    public int x;
    public int y;

    public int g;
    public int h;

    public int f;

    public bool isWalkable;

    public Node fatherNode;


    public Node(int X, int Y, Grid Grid)
    {
        x = X;
        y = Y;
        grid = Grid;

        g = 0;
        h = 0;
        f = g + h;

        isWalkable = true;
        fatherNode = null;
    }
}
