using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobBuildController : MonoBehaviour {

    public static JobBuildController Instance { get; protected set; }

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

    
    public void AddJob(Job build)
    {
        build.tile.jobOnTile = build;

        pendingJobList.Add(build); //Debug.Log("pendingBuildJobList.Count" + pendingJobList.Count);
    }


    public void CancelJob(Job job)
    {
        if (pendingJobList.Contains(job))
        {
            pendingJobList.Remove(job); //Debug.Log("pendingBuildJobList.Count" + pendingJobList.Count);
        }
    }


    public void CreatJobQueue(Human h_worker)
    {
        for (int i = 0; i < pendingJobList.Count; i++)
        {
            Job build = pendingJobList[i];

            if (build.tile.arch.stats == ArchStats.building)
            {
                Job find = FindMaterial(build);

                if (find != null)
                {
                    JobQueue jq_build = new JobQueue();
                    jq_build.Add(find);
                    jq_build.Add(build);

                    find.item = find.tile.item;
                    find.item.registeredJob = find;

                    h_worker.currentJobQueue = jq_build;
                    jq_build.worker = h_worker;

                    find.worker = h_worker;
                    build.worker = h_worker;

                    pendingJobList.Remove(build);
                    jobQueueList.Add(jq_build);
                    assignedJobQueueList.Add(jq_build);
                    break;
                }
            }

            if (build.tile.arch.stats == ArchStats.destroying)
            {
                JobQueue jq_build = new JobQueue();
                jq_build.Add(build);

                h_worker.currentJobQueue = jq_build;
                jq_build.worker = h_worker;

                build.worker = h_worker;

                pendingJobList.Remove(build);
                jobQueueList.Add(jq_build);
                assignedJobQueueList.Add(jq_build);
                break;
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
                    if (jq.Count == 2) //j = find;
                    {
                        GameObject worker_obj = HumanController.Instance.humanGameObjectMap[j.worker];
                        GameObject item_obj = ItemController.Instance.itemGameobjectMap[j.item];

                        if (j.item.currentStack > 1)
                        {
                            j.item.currentStack -= 1;
                            ItemController.Instance.itemGameobjectMap[j.item].transform.GetComponentInChildren<Text>().text = j.item.currentStack.ToString();

                            Item newItem_data = ItemController.Instance.CreateItem(j.item.category, j.item.type, item_obj.transform.position, 1);
                            GameObject newItem_obj = ItemController.Instance.itemGameobjectMap[newItem_data];
                            newItem_obj.transform.SetParent(worker_obj.transform);
                            newItem_obj.GetComponent<SpriteRenderer>().sortingLayerName = "HumanItem";

                            j.worker.itemInHand = newItem_data;
                            newItem_data.carrier = j.worker;

                            newItem_data.registeredJob = jq[1];
                            jq[1].item = newItem_data;

                            j.item.registeredJob = null;
                        }
                        else
                        {
                            item_obj.transform.SetParent(worker_obj.transform);
                            item_obj.GetComponent<SpriteRenderer>().sortingLayerName = "HumanItem";

                            j.worker.itemInHand = j.item;
                            j.item.carrier = j.worker;

                            j.tile.item = null;
                            j.item.tile = null;

                            j.item.registeredJob = jq[1];
                            jq[1].item = j.item;
                        }
                    }

                    if (jq.Count == 1) //j = build;
                    {
                        j.worker.itemInHand = null;

                        j.tile.jobOnTile = null;

                        if(j.item != null)
                        {
                            ItemController.Instance.DestoryItem(j.item);
                        }

                        ArchitectController.Instance.ArchWorkFinished(j.tile);
                    }

                    j.worker.isWorking = false; //Debug.Log(jq[0].type.ToString() + "JobDone" + (jq.Count - 1));
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


    Job FindMaterial(Job build)
    {
        Architecture arch_data = build.tile.arch;
        Item material = null;

        for (int i = 0; i < ItemController.Instance.materialList.Count; i++)
        {
            Item m = ItemController.Instance.materialList[i];

            if (m.type == arch_data.type && m.registeredJob == null && m.currentStack > 0)
            {
                material = m;
                break;
            }
        }

        if(material == null)
        {
            return null;
        }
        else
        {
            Job find = new Job(JobType.build, material.tile, 5);
            return find;
        }
    }
}
