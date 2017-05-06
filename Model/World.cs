using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    public Tile[,] tiles;

    public int width;
    public int length;

    
    public World(int Width, int Length)
    {
        width = Width;
        length = Length;

        tiles = new Tile[Width, Length];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
            {
                tiles[x, y] = new Tile("Dirt", this, x, y, 1);
            }
        }

        Debug.Log("world created with " + (Width * Length) + " tiles");
    }
}
