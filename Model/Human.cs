using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human {

    public string name;
    public string faceType;
    public string bodyType;

    public Faction faction;

    public Schedule schedule;

    public JobType jobType;
    public JobQueue currentJobQueue;
    public Job currentJob;

    public Need need;

    public PathFinding pathAI;
    public Tile targetTile;

    public Item itemInHand;

    public bool underControl;
    public bool isSelceted;
    public bool isWorking;
    public bool isLocal;

    public int workSpeed;
    public int hunger;
    public int energy;
    public int happiness;


    public Human(string Name, string FaceType, string BodyType, Faction Faction)
    {
        name = Name;
        faceType = FaceType;
        bodyType = BodyType;

        faction = Faction;

        schedule = null;

        jobType = JobType.build;
        currentJobQueue = null;
        currentJob = null;

        need = null;

        pathAI = null;
        targetTile = null;

        itemInHand = null;

        underControl = false;
        isSelceted = false;
        isWorking = false;
        isLocal = true;

        workSpeed = 1;
        hunger = 100;
        energy = 100;
        happiness = 100;
    }
}
