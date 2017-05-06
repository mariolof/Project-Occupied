using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreeModeController : MonoBehaviour {

    public static FreeModeController Instance { get; protected set; }

    public List<Tile> tileHasJob { get; protected set; }
    public Vector3 dragStartPos { get; protected set; }
    public Vector3 mousePos { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        tileHasJob = new List<Tile>();
    }
	

	void Update ()
    {
        mousePos = MouseController.Instance.mousePosition;
    }


    public void CursorDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            MouseController.Instance.SelectRec.SetActive(false);
            tileHasJob.Clear();
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = mousePos;
            tileHasJob.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            MouseController.Instance.SelectRec.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            float diffX = dragStartPos.x - mousePos.x;
            float diffY = dragStartPos.y - mousePos.y;

            if (diffX > 0)
            {
                diffX = -diffX;
            }

            if (diffY > 0)
            {
                diffY = -diffY;
            }

            float midX = (mousePos.x + dragStartPos.x) * 0.5f;
            float midY = (mousePos.y + dragStartPos.y) * 0.5f;

            MouseController.Instance.SelectRec.SetActive(true);
            MouseController.Instance.SelectRec.transform.position = new Vector3(midX, midY, 0f);
            MouseController.Instance.SelectRec.transform.localScale = new Vector3(diffX, diffY, 1f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseController.Instance.SelectRec.SetActive(false);

            float diffX = Mathf.Abs(dragStartPos.x - mousePos.x) / 2;
            float diffY = Mathf.Abs(dragStartPos.y - mousePos.y) / 2;
            float midX = (mousePos.x + dragStartPos.x) / 2;
            float midY = (mousePos.y + dragStartPos.y) / 2;

            for (int i = 0; i < HumanController.Instance.villagerList.Count; i++)
            {
                Human human_data = HumanController.Instance.villagerList[i];

                GameObject human_obj = HumanController.Instance.humanGameObjectMap[human_data];
                Vector3 pos = human_obj.transform.position;

                if (Mathf.Abs(pos.x - midX) > diffX || Mathf.Abs(pos.y - midY) > diffY)
                {
                    HumanController.Instance.SwitchHumanSelected(human_data, false);
                }
                else
                {
                    HumanController.Instance.SwitchHumanSelected(human_data, true);
                }
            }
        }
    }


    public void BuildJobCancel()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            MouseController.Instance.SelectRec.SetActive(false);
            tileHasJob.Clear();
            return;
        }


        if (Input.GetMouseButtonDown(1))
        {
            dragStartPos = mousePos;
            tileHasJob.Clear();
        }

        if (Input.GetMouseButton(1))
        {
            MouseController.Instance.SelectRec.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 1f, 1f);

            float diffX = dragStartPos.x - mousePos.x;
            float diffY = dragStartPos.y - mousePos.y;

            if (diffX > 0)
            {
                diffX = -diffX;
            }

            if (diffY > 0)
            {
                diffY = -diffY;
            }

            float midX = (mousePos.x + dragStartPos.x) * 0.5f;
            float midY = (mousePos.y + dragStartPos.y) * 0.5f;

            MouseController.Instance.SelectRec.SetActive(true);
            MouseController.Instance.SelectRec.transform.position = new Vector3(midX, midY, 0f);
            MouseController.Instance.SelectRec.transform.localScale = new Vector3(diffX, diffY, 1f);
        }

        if (Input.GetMouseButtonUp(1))
        {
            MouseController.Instance.SelectRec.SetActive(false);

            int startTileX = WorldController.Instance.GetTileAtPos(dragStartPos).x;
            int startTileY = WorldController.Instance.GetTileAtPos(dragStartPos).y;

            int endTileX = WorldController.Instance.GetTileAtPos(mousePos).x;
            int endTileY = WorldController.Instance.GetTileAtPos(mousePos).y;

            int diffX = startTileX - endTileX;
            int diffY = startTileY - endTileY;


            if (diffX >= 0 && diffY >= 0)
            {
                for (int x = 0; x <= diffX; x++)
                {
                    for (int y = 0; y <= diffY; y++)
                    {
                        Tile tempTile = WorldController.Instance.GetTileAt(startTileX - x, startTileY - y);

                        if (tempTile.jobOnTile !=null)
                        {
                            tileHasJob.Add(tempTile);
                        }
                    }
                }
            }

            if (diffX >= 0 && diffY < 0)
            {
                for (int x = 0; x <= diffX; x++)
                {
                    for (int y = diffY; y <= 0; y++)
                    {
                        Tile tempTile = WorldController.Instance.GetTileAt(startTileX - x, startTileY - y);

                        if (tempTile.jobOnTile != null)
                        {
                            tileHasJob.Add(tempTile);
                        }
                    }
                }
            }

            if (diffX < 0 && diffY >= 0)
            {
                for (int x = diffX; x <= 0; x++)
                {
                    for (int y = 0; y <= diffY; y++)
                    {
                        Tile tempTile = WorldController.Instance.GetTileAt(startTileX - x, startTileY - y);

                        if (tempTile.jobOnTile != null)
                        {
                            tileHasJob.Add(tempTile);
                        }
                    }
                }
            }

            if (diffX < 0 && diffY < 0)
            {
                for (int x = diffX; x <= 0; x++)
                {
                    for (int y = diffY; y <= 0; y++)
                    {
                        Tile tempTile = WorldController.Instance.GetTileAt(startTileX - x, startTileY - y);

                        if (tempTile.jobOnTile != null)
                        {
                            tileHasJob.Add(tempTile);
                        }
                    }
                }
            }


            if(tileHasJob.Count != 0)
            {
                for (int i = 0; i < tileHasJob.Count; i++)
                {
                    Tile tile = tileHasJob[i];

                    if (tile.arch != null && tile.arch.stats == ArchStats.building && tile.jobOnTile.worker == null)
                    {
                        ArchitectController.Instance.DestoryArch(tile);
                        JobController.Instance.CancelJob(tile.jobOnTile);
                    }

                    if(tile.arch != null && tile.arch.stats == ArchStats.destroying && tile.jobOnTile.worker == null)
                    {
                        ArchitectController.Instance.SetArchStatsDone(tile.arch);
                        JobController.Instance.CancelJob(tile.jobOnTile);
                    }
                }
            }
        }
    }


    public void ClickSetTarget()
    {
        if (Input.GetMouseButtonUp(1))
        {
            for (int i = 0; i < HumanController.Instance.selectedHumanList.Count; i++)
            {
                Human human_data = HumanController.Instance.selectedHumanList[i];
                Tile targetTile = WorldController.Instance.GetTileAtPos(mousePos);

                if (human_data.underControl == true && targetTile != null && targetTile.isWalkable == true)
                {
                    MoveController.Instance.SetPath(targetTile, human_data);
                    human_data.isWorking = false;

                    if(human_data.currentJob != null)
                    {
                        human_data.currentJob.isJobAssigned = false;
                        human_data.currentJob = null;
                    }
                }
            }
        }
    }
}
