using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobFarmController : MonoBehaviour
{

    public static JobFarmController Instance { get; protected set; }

    public List<Job> pendingJobList { get; protected set; }
    public List<JobQueue> jobQueueList { get; protected set; }
    public List<JobQueue> assignedJobQueueList { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start()
    {
        pendingJobList = new List<Job>();
        jobQueueList = new List<JobQueue>();
        assignedJobQueueList = new List<JobQueue>();
    }


    void Update()
    {

    }


    public void AddJob(Job farm)
    {
        farm.tile.jobOnTile = farm;

        pendingJobList.Add(farm); Debug.Log("pendingFarmJobList.Count" + pendingJobList.Count);
    }


    public void CreatJobQueue(Human h_worker)
    {
        for (int i = 0; i < pendingJobList.Count; i++)
        {
            Job farm = pendingJobList[i];

            if (farm.tile.plant == null)
            {
                if (farm.tile.area == null || farm.tile.area.type != "Farmland")
                {
                    pendingJobList.Remove(farm); 
                    farm.tile.jobOnTile = null;
                }
                else
                {
                    JobQueue jq_farm = new JobQueue();
                    jq_farm.Add(farm);

                    h_worker.currentJobQueue = jq_farm;
                    jq_farm.worker = h_worker;

                    farm.worker = h_worker;

                    pendingJobList.Remove(farm);
                    jobQueueList.Add(jq_farm);
                    assignedJobQueueList.Add(jq_farm);
                    break;
                }
            }
            else
            {
                if (farm.tile.plant.catagory == "Crop" && farm.tile.plant.currentStage == farm.tile.plant.maxStage)
                {
                    JobQueue jq_harvest = new JobQueue();
                    jq_harvest.Add(farm);

                    h_worker.currentJobQueue = jq_harvest;
                    jq_harvest.worker = h_worker;

                    farm.worker = h_worker;

                    pendingJobList.Remove(farm);
                    jobQueueList.Add(jq_harvest);
                    assignedJobQueueList.Add(jq_harvest);
                    break;
                }
                else
                {
                    pendingJobList.Remove(farm);
                    farm.tile.jobOnTile = null;
                }
            }
        }
    }


    public void ExecuteJobQueue()
    {
        for (int i = 0; i < assignedJobQueueList.Count; i++)
        {
            JobQueue jq = assignedJobQueueList[i];

            if (jq.Count != 0)
            {
                Job j = jq[0];

                if (jq.worker.isWorking == false && jq.worker.currentJob == null)
                {
                    MoveController.Instance.SetPath(j.tile, jq.worker);
                    jq.worker.isWorking = true;
                    jq.worker.currentJob = j;
                }
            }
        }
    }


    public void CheckHasWorker()
    {
        for (int i = 0; i < assignedJobQueueList.Count; i++)
        {
            JobQueue jq = assignedJobQueueList[i];

            if (jq.Count != 0)
            {
                Job j = jq[0];

                Vector3 currentWorkerPos = HumanController.Instance.humanGameObjectMap[j.worker].transform.position;
                Vector3 jobPos = new Vector3(j.tile.x, j.tile.y, 0f);

                if (j.worker.isWorking == true && currentWorkerPos == jobPos)
                {
                    j.hasWorker = true;
                }
                else
                {
                    j.hasWorker = false;
                }
            }
        }
    }


    public void CheckProgress()
    {
        for (int i = 0; i < assignedJobQueueList.Count; i++)
        {
            JobQueue jq = assignedJobQueueList[i];

            if (jq.Count != 0)
            {
                Job j = jq[0];

                if (j.hasWorker == true && j.jobWorkLoad > 0)
                {
                    j.jobWorkLoad -= Mathf.CeilToInt(j.worker.workSpeed * Time.timeScale);
                }

                if (j.jobWorkLoad <= 0)
                {
                    if (j.tile.plant == null)
                    {
                        PlantController.Instance.CreatePlantAtTile("Crop", "Rice", j.tile);
                    }
                    else
                    {
                        PlantController.Instance.HarvestPlant(j.tile);
                    }

                    j.tile.jobOnTile = null;

                    j.worker.isWorking = false; Debug.Log(jq[0].type.ToString() + "JobDone" + (jq.Count - 1));
                    j.worker.currentJob = null;

                    jq.RemoveAt(0);
                }
            }

            if (jq.Count == 0)
            {
                jq.worker.currentJobQueue = null;
                jq.worker = null;

                jobQueueList.Remove(jq);
                assignedJobQueueList.Remove(jq);
            }
        }
    }
}
