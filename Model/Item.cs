using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public string category;
    public string type;

    public Tile tile;
    public Human carrier;
    public Job registeredJob;
    public Need registeredNeed;

    public bool isStackable;
    public int maxStack;
    public int currentStack;


    public Item(string Category, string Type, int n)
    {
        category = Category;
        type = Type;

        tile = null;
        carrier = null;
        registeredJob = null;
        registeredNeed = null;

        isStackable = true;
        maxStack = 100;
        currentStack = n;
    }
}
