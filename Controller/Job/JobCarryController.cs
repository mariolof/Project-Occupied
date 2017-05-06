using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobCarryController : MonoBehaviour {

    public static JobCarryController Instance { get; protected set; }

    public List<Job> pendingJobList { get; protected set; }
    public List<JobQueue> jobQueueList { get; protected set; }
    public List<JobQueue> assignedJobQueueList { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        pendingJobList = new List<Job>();
        jobQueueList = new List<JobQueue>();
        assignedJobQueueList = new List<JobQueue>();
	}
	

	void Update ()
    {
		
	}


    public void AddJob(Job pickUp)
    {
        pickUp.item = pickUp.tile.item;
        pickUp.item.registeredJob = pickUp;

        pendingJobList.Add(pickUp); //Debug.Log("pendingCarryJobList.Count" + pendingJobList.Count);
    }


    public void CreatJobQueue(Human h_worker)
    {
        for (int i = 0; i < pendingJobList.Count; i++)
        {
            Job pickUp = pendingJobList[i];
            Job place = FindStorageTile(pickUp);

            if (pickUp.item.tile.area != null && pickUp.item.tile.area.type == "Storage")
            {
                pendingJobList.Remove(pickUp);
                pickUp.item.registeredJob = null;
            }
            else if (place != null)
            {
                JobQueue jq_carry = new JobQueue();
                jq_carry.Add(pickUp);
                jq_carry.Add(place);

                place.tile.jobOnTile = place;

                h_worker.currentJobQueue = jq_carry;
                jq_carry.worker = h_worker;

                pickUp.worker = h_worker;
                place.worker = h_worker;

                pendingJobList.Remove(pickUp);
                jobQueueList.Add(jq_carry);
                assignedJobQueueList.Add(jq_carry);
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
                    j.worker = jq.worker;
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
                    if (jq.Count == 2) //j = PickUp
                    {
                        GameObject worker_obj = HumanController.Instance.humanGameObjectMap[j.worker];
                        GameObject item_obj = ItemController.Instance.itemGameobjectMap[j.item];

                        if(jq[1].tile.item == null)
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
                        else
                        {
                            int diff = jq[1].tile.item.maxStack - jq[1].tile.item.currentStack;

                            if (j.item.currentStack <= diff)
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
                            else
                            {
                                j.item.currentStack -= diff;
                                ItemController.Instance.itemGameobjectMap[j.item].transform.GetComponentInChildren<Text>().text = j.item.currentStack.ToString();

                                Item newItem_data = ItemController.Instance.CreateItem(j.item.category, j.item.type, item_obj.transform.position, diff);
                                GameObject newItem_obj = ItemController.Instance.itemGameobjectMap[newItem_data];
                                newItem_obj.transform.SetParent(worker_obj.transform);
                                newItem_obj.GetComponent<SpriteRenderer>().sortingLayerName = "HumanItem";

                                j.worker.itemInHand = newItem_data;
                                newItem_data.carrier = j.worker;

                                newItem_data.registeredJob = jq[1];
                                jq[1].item = newItem_data;

                                j.item.registeredJob = null;
                            }
                        }
                    }

                    if (jq.Count == 1) //j = Place
                    {
                        GameObject worker_obj = HumanController.Instance.humanGameObjectMap[j.worker];
                        GameObject item_obj = ItemController.Instance.itemGameobjectMap[j.item];

                        if(j.tile.item == null)
                        {
                            item_obj.transform.SetParent(ItemController.Instance.AllItem.transform);
                            item_obj.GetComponent<SpriteRenderer>().sortingLayerName = "Item";

                            j.worker.itemInHand = null;
                            j.item.carrier = null;

                            j.tile.item = j.item;
                            j.item.tile = j.tile;

                            j.item.registeredJob = null;

                            j.tile.jobOnTile = null;
                        }
                        else
                        {
                            j.tile.item.currentStack += j.worker.itemInHand.currentStack;
                            ItemController.Instance.itemGameobjectMap[j.tile.item].transform.GetComponentInChildren<Text>().text = j.tile.item.currentStack.ToString();

                            j.worker.itemInHand = null;

                            j.tile.jobOnTile = null;

                            ItemController.Instance.DestoryItem(j.item);
                        }
                    }

                    j.worker.isWorking = false;
                    j.worker.currentJob = null; //Debug.Log(jq[0].type.ToString() + "JobDone" + (jq.Count - 1));

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


    Job FindStorageTile(Job pickUp)
    {
        Tile storageTile = null;

        for (int i = 0; i < AreaController.Instance.StorageLsit.Count; i++)
        {
            Area a = AreaController.Instance.StorageLsit[i];

            for (int j = 0; j < a.tileInArea.Count; j++)
            {
                Tile t = a.tileInArea[j];

                if (t.jobOnTile == null && t.area.type == "Storage")
                {
                    if (t.item != null && t.item.type == pickUp.item.type && t.item.currentStack < t.item.maxStack)
                    {
                        storageTile = t;
                        break;
                    }
                    else if (t.item == null)
                    {
                        storageTile = t;
                        break;
                    }
                }
            }

            if(storageTile != null)
            {
                break;
            }
        }

        if(storageTile == null)
        {
            return null;
        }
        else
        {
            Job place = new Job(JobType.carry, storageTile, 5);
            return place;
        }
    }
}
