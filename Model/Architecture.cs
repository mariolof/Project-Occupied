using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architecture {

    public string category;
    public string name;
    public string type;

    public int sizeX;
    public int sizeY;

    public Tile tile;
    public ArchStats stats;

    public float speedRatio;
    public int health;


    public Architecture(string Category, string Name, string Type, Tile Tile)
    {
        category = Category;
        name = Name;
        type = Type;

        sizeX = 1;
        sizeY = 1;

        tile = Tile;
        stats = ArchStats.building;

        speedRatio = 1;
        health = 100;
    }
}
