using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    public static MoveController Instance { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}


    public void CheckInPosition()
    {
        for (int i = 0; i < HumanController.Instance.humanList.Count; i++)
        {
            Human human_data = HumanController.Instance.humanList[i];

            if (human_data.targetTile != null)
            {
                GameObject human_obj = HumanController.Instance.humanGameObjectMap[human_data];
                Vector3 targetTilePos = new Vector3(human_data.targetTile.x, human_data.targetTile.y, 0f);

                if (human_obj.transform.position == targetTilePos)
                {
                    human_data.targetTile = null; //Debug.Log(human_data.name + " in position");
                }
            }
        }
    }


    public void SetPath(Tile newTargetTile, Human human_data)
    {
        if (newTargetTile != human_data.targetTile)
        {
            human_data.targetTile = newTargetTile;
            human_data.pathAI = new PathFinding(newTargetTile, HumanController.Instance.humanGameObjectMap[human_data], WorldController.Instance.currentWorld);
        }
    }


    public void Move(float speed)
    {
        for (int i = 0; i < HumanController.Instance.humanList.Count; i++)
        {
            Human human_data = HumanController.Instance.humanList[i];

            if (human_data.targetTile != null)
            {
                if (human_data.pathAI.currentPath == null)
                {
                    return;
                }
                else
                {
                    int n = human_data.pathAI.currentPath.Count;

                    if (n > 0)
                    {
                        Node waypoint = human_data.pathAI.currentPath[n - 1];

                        Tile tempTarget = WorldController.Instance.GetTileAt(waypoint.x, waypoint.y);

                        Vector3 startPos = HumanController.Instance.humanGameObjectMap[human_data].transform.position;
                        Vector3 endPos = new Vector3(tempTarget.x, tempTarget.y, 0f);


                        if (startPos != endPos)
                        {
                            if (human_data.isWorking == true || human_data.need != null)
                            {
                                HumanController.Instance.humanGameObjectMap[human_data].transform.position = Vector3.MoveTowards(startPos, endPos, speed * Time.deltaTime);
                            }
                            else
                            {
                                HumanController.Instance.humanGameObjectMap[human_data].transform.position = Vector3.MoveTowards(startPos, endPos, speed * Time.deltaTime * 0.2f);
                            }

                        }
                        else
                        {
                            human_data.pathAI.currentPath.RemoveAt(n - 1);
                        }
                    }
                }
            }
        }
    }


    public void Wonder()
    {
        for (int i = 0; i < HumanController.Instance.humanList.Count; i++)
        {
            Human human_data = HumanController.Instance.humanList[i];

            if (human_data.targetTile == null && human_data.isWorking == false && human_data.currentJobQueue == null && human_data.need == null && human_data.schedule.scheduleMap[TimeController.Instance.hour] != ScheduleType.Rest)
            {
                int x = Random.Range(-2, 3);
                int y = Random.Range(-2, 3);

                Vector3 currntPos = HumanController.Instance.humanGameObjectMap[human_data].transform.position;
                Tile targetTile = WorldController.Instance.GetTileAt(Mathf.FloorToInt(currntPos.x) + x, Mathf.FloorToInt(currntPos.y) + y);

                if (targetTile != null && targetTile.isWalkable == true)
                {
                    SetPath(targetTile, human_data);
                }
            }
        }
    }
}
