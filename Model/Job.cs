using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job {

    public JobType type;
    public int jobWorkLoad;

    public Tile tile;
    public Human worker;
    public Item item;
    public JobQueue Queue;

    public bool isJobAssigned;
    public bool hasWorker;


    public Job(JobType Type, Tile JobTile, int JobWorkLoad)
    {
        type = Type;
        jobWorkLoad = JobWorkLoad;

        tile = JobTile;
        worker = null;
        item = null;
        Queue = null;

        isJobAssigned = false;
        hasWorker = false;
    }
}
