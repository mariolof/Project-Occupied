using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobQueue {

    public Human worker;
    public List<Job> jobs;


    public JobQueue()
    {
        jobs = new List<Job>();
        worker = null;
    }

    public int Count
    {
        get
        {
            return jobs.Count;
        }
    }

    public Job this[int index]
    {
        get
        {
            return jobs[index];
        }
    }

    public void Add(Job j)
    {
        jobs.Add(j);
    }

    public void RemoveAt(int index)
    {
        if (jobs == null || jobs.Count == 0)
        {
            return;
        }

        jobs.RemoveAt(index);
    }

    public void Clear()
    {
        jobs.Clear();
    }
}
