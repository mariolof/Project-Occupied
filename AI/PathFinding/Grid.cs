using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    public Node[,] nodes;

    public World world;

    public int width;
    public int length;


    public Grid(World World)
    {
        world = World; 

        width = world.width;
        length = world.length;

        nodes = new Node[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                nodes[x, y] = new Node(x, y, this);
                nodes[x, y].isWalkable = world.tiles[x, y].isWalkable;
            }
        }
    }
}
