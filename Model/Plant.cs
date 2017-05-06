using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant {

    public string catagory;
    public string name;
    public string growStage;

    //public bool isCorp;
    public Tile tile;

    public int growHour;
    public int currentGrowHour;
    public int maxStage;
    public int currentStage;


    public Plant(string Category, string Name)
    {
        catagory = Category;
        name = Name;
        growStage = "0";

        //isCorp = false;
        tile = null;

        growHour = 10;
        currentGrowHour = 0;
        maxStage = 2;
        currentStage = 0;
    }
}
