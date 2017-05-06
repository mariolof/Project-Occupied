using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JobController : MonoBehaviour {

    public static JobController Instance { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start()
    {

    }


    void Update()
    {
        ExecuteJobQueue();
        CheckWorkerPos();
        CheckProgress();
    }


    public void CreateJob(JobType jT, Tile t, int WL)
    {
        Job job = new Job(jT, t, WL);

        switch (job.type)
        {
            case JobType.build:
                JobBuildController.Instance.AddJob(job);
                break;

            case JobType.carry:
                JobCarryController.Instance.AddJob(job);
                break;

            case JobType.farm:
                JobFarmController.Instance.AddJob(job);
                break;

            case JobType.cook:
                break;

            case JobType.care:
                break;

            case JobType.produce:
                break;

            case JobType.gather:
                break;

            case JobType.guard:
                break;

            case JobType.maintain:
                break;

            default:
                break;
        }
    }


    public void CancelJob(Job job)
    {
        switch (job.type)
        {
            case JobType.build:
                JobBuildController.Instance.CancelJob(job);
                break;

            case JobType.carry:
                break;

            case JobType.farm:
                break;

            case JobType.cook:
                break;

            case JobType.care:
                break;

            case JobType.produce:
                break;

            case JobType.gather:
                break;

            case JobType.guard:
                break;

            case JobType.maintain:
                break;

            default:
                break;
        }
    }


    public void UnAssignJobQueue()
    {
        
    }


    public void ExecuteJobQueue()
    {
        JobBuildController.Instance.ExecuteJobQueue();
        JobCarryController.Instance.ExecuteJobQueue();
        JobFarmController.Instance.ExecuteJobQueue();
    }


    public void CheckWorkerPos()
    {
        JobBuildController.Instance.CheckHasWorker();
        JobCarryController.Instance.CheckHasWorker();
        JobFarmController.Instance.CheckHasWorker();
    }


    public void CheckProgress()
    {
        JobBuildController.Instance.CheckProgress();
        JobCarryController.Instance.CheckProgress();
        JobFarmController.Instance.CheckProgress();
    }
}
