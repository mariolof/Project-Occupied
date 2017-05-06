using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public string type;
    public float speedRatio;
    public int x;
    public int y;

    public World world;
    public Area area;
    public Room room;

    public Architecture arch;
    public Item item;
    public Plant plant;

    public Job jobOnTile;

    public bool isWalkable;


    public Tile (string Type, World World, int X, int Y, float SpeedRatio)
    {
        type = Type;
        speedRatio = SpeedRatio;
        x = X;
        y = Y;

        world = World;
        area = null;
        room = null;

        arch = null;
        item = null;
        plant = null;

        jobOnTile = null;

        isWalkable = true;
    }
}
