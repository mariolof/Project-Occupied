using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour {

    public static BuildModeController Instance { get; protected set; }

    public List<Tile> tileIsSelected { get; protected set; }

    public Vector3 mousePos { get; protected set; }
    public Tile dragStartTile { get; protected set; }
    public Tile dragcurrntTile { get; protected set; }

    public Architecture furniturePreview_data { get; protected set; }
    public GameObject furniturePreview_obj { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        tileIsSelected = new List<Tile>();
        furniturePreview_data = null;
    }
	

	void Update ()
    {
        mousePos = MouseController.Instance.mousePosition;
    }


    public void ResetFurniturePreview()
    {
        if (furniturePreview_data != null)
        {
            Destroy(furniturePreview_obj);
            ArchitectController.Instance.archGameObjectMap.Remove(furniturePreview_data);
            furniturePreview_data = null;
        }
    }


    public void BuildJobCreate(BuildMode buildMode)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            MouseController.Instance.SelectRec.SetActive(false);
            ResetFurniturePreview();
            return;
        }


        if (buildMode != BuildMode.furniture)
        {
            MouseController.Instance.SelectRec.SetActive(true);
            MouseController.Instance.SelectRec.transform.position = new Vector3(Mathf.Floor(mousePos.x + 0.5f), Mathf.Floor(mousePos.y + 0.5f), 0f);
            MouseController.Instance.SelectRec.transform.localScale = new Vector3(1, 1, 1f);
        }
        else if (buildMode == BuildMode.furniture && Input.GetMouseButton(1) == false)
        {
            if (furniturePreview_data == null)
            {
                furniturePreview_data = ArchitectController.Instance.CreateArchPre(MouseController.Instance.currentBuildCategory, MouseController.Instance.currentBuildName, MouseController.Instance.furnitureType, MouseController.Instance.tileUnderMouse);
                furniturePreview_obj = ArchitectController.Instance.archGameObjectMap[furniturePreview_data];
                furniturePreview_data.tile = null;
            }

            furniturePreview_obj.transform.position = new Vector3(MouseController.Instance.tileUnderMouse.x, MouseController.Instance.tileUnderMouse.y, 0f);
            dragcurrntTile = MouseController.Instance.tileUnderMouse;
        }


        if (Input.GetMouseButton(1))
        {
            MouseController.Instance.SelectRec.SetActive(false);
            ResetFurniturePreview();
            tileIsSelected.Clear();
            dragStartTile = null;
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            tileIsSelected.Clear();
            dragStartTile = MouseController.Instance.tileUnderMouse;
        }

        if (Input.GetMouseButton(0) && buildMode != BuildMode.furniture)
        {
            if(dragStartTile != null)
            {
                dragcurrntTile = MouseController.Instance.tileUnderMouse;

                int diffxAbs = Mathf.Abs(dragStartTile.x - dragcurrntTile.x);
                int diffyAbs = Mathf.Abs(dragStartTile.y - dragcurrntTile.y);

                float midX = (dragStartTile.x + dragcurrntTile.x) * 0.5f;
                float midY = (dragStartTile.y + dragcurrntTile.y) * 0.5f;

                if (buildMode == BuildMode.structure)
                {
                    if (diffxAbs >= diffyAbs)
                    {
                        MouseController.Instance.SelectRec.SetActive(true);
                        MouseController.Instance.SelectRec.transform.position = new Vector3(midX, dragStartTile.y, 0f);
                        MouseController.Instance.SelectRec.transform.localScale = new Vector3(diffxAbs + 1, 1, 1f);
                    }

                    if (diffxAbs < diffyAbs)
                    {
                        MouseController.Instance.SelectRec.SetActive(true);
                        MouseController.Instance.SelectRec.transform.position = new Vector3(dragStartTile.x, midY, 0f);
                        MouseController.Instance.SelectRec.transform.localScale = new Vector3(1, diffyAbs + 1, 1f);
                    }
                }
                
                if (buildMode == BuildMode.floor || buildMode == BuildMode.area || buildMode == BuildMode.destroy)
                {
                    MouseController.Instance.SelectRec.SetActive(true);
                    MouseController.Instance.SelectRec.transform.position = new Vector3(midX, midY, 0f);
                    MouseController.Instance.SelectRec.transform.localScale = new Vector3(diffxAbs + 1, diffyAbs + 1, 1f);
                }
            }
        } 

        if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(1) == false)
        {
            if (dragStartTile != null)
            {
                MouseController.Instance.SelectRec.SetActive(false);

                int diffX = dragStartTile.x - dragcurrntTile.x;
                int diffY = dragStartTile.y - dragcurrntTile.y;

                int diffxAbs = Mathf.Abs(diffX);
                int diffyAbs = Mathf.Abs(diffY);

                switch (buildMode)
                {
                    case BuildMode.floor:
                        break;

                    case BuildMode.structure:
                        WallBuildMode(diffX, diffY, diffxAbs, diffyAbs);
                        break;

                    case BuildMode.furniture:
                        FurnitureBuildMode();
                        break;

                    case BuildMode.area:
                        AreaBuildMode(diffX, diffY);
                        break;

                    case BuildMode.destroy:
                        DestroyBuildMode(diffX, diffY);
                        break;

                    default:
                        break;
                }
            }
        }
    }


    void FurnitureBuildMode()
    {
        Tile t = MouseController.Instance.tileUnderMouse;

        if(t !=  null && t.item == null)
        {
            if(t.arch == null)
            {
                ArchitectController.Instance.CreateArchAtTile(MouseController.Instance.currentBuildCategory, MouseController.Instance.currentBuildName, MouseController.Instance.furnitureType, t);
                JobController.Instance.CreateJob(JobType.build, t, 50);
            }
            else if(t.arch.name == "Wall" && MouseController.Instance.currentBuildName == "Door")
            {

            }
            else
            {
                Debug.Log("tile is full");
            }
        }
    }


    void WallBuildMode(int diffX, int diffY, int diffxAbs, int diffyAbs)
    {
        if (diffxAbs >= diffyAbs)
        {
            if (diffX < 0)
            {
                for (int x = dragStartTile.x; x <= dragcurrntTile.x; x++)
                {
                    tileIsSelected.Add(WorldController.Instance.GetTileAt(x, dragStartTile.y));
                }
            }

            if (diffX >= 0)
            {
                for (int x = dragcurrntTile.x; x <= dragStartTile.x; x++)
                {
                    tileIsSelected.Add(WorldController.Instance.GetTileAt(x, dragStartTile.y));
                }
            }
        }

        if (diffxAbs < diffyAbs)
        {
            if (diffY < 0)
            {
                for (int y = dragStartTile.y; y <= dragcurrntTile.y; y++)
                {
                    tileIsSelected.Add(WorldController.Instance.GetTileAt(dragStartTile.x, y));
                }
            }

            if (diffY >= 0)
            {
                for (int y = dragcurrntTile.y; y <= dragStartTile.y; y++)
                {
                    tileIsSelected.Add(WorldController.Instance.GetTileAt(dragStartTile.x, y));
                }
            }
        }


        if (tileIsSelected.Count != 0)
        {
            for (int i = 0; i < tileIsSelected.Count; i++)
            {
                Tile t = tileIsSelected[i];

                if (t.arch == null && t.item == null)
                {
                    if (t.area != null)
                    {
                        AreaController.Instance.RemoveTileFromArea(t);
                    }

                    ArchitectController.Instance.CreateArchAtTile(MouseController.Instance.currentBuildCategory, MouseController.Instance.currentBuildName, MouseController.Instance.structureType, t);
                    JobController.Instance.CreateJob(JobType.build, t, 100);
                }
                else
                {
                    Debug.Log("tile is full");
                }
            }
        }
    }


    void DestroyBuildMode(int diffX, int diffY)
    {
        if (diffX >= 0 && diffY >= 0)
        {
            for (int x = 0; x <= diffX; x++)
            {
                for (int y = 0; y <= diffY; y++)
                {
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);

                    if (tempTile.arch != null && tempTile.arch.stats == ArchStats.done)
                    {
                        tileIsSelected.Add(tempTile);
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
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);

                    if (tempTile.arch != null && tempTile.arch.stats == ArchStats.done)
                    {
                        tileIsSelected.Add(tempTile);
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
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);

                    if (tempTile.arch != null && tempTile.arch.stats == ArchStats.done)
                    {
                        tileIsSelected.Add(tempTile);
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
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);

                    if (tempTile.arch != null && tempTile.arch.stats == ArchStats.done)
                    {
                        tileIsSelected.Add(tempTile);
                    }
                }
            }
        }


        if (tileIsSelected.Count != 0)
        {
            for (int i = 0; i < tileIsSelected.Count; i++)
            {
                Tile t = tileIsSelected[i];

                ArchitectController.Instance.SetArchStatsDestroying(t.arch);
                JobController.Instance.CreateJob(JobType.build, t, 50);
                t.isWalkable = true;
            }
        }
    } 


    void AreaBuildMode(int diffX, int diffY)
    {
        if (diffX >= 0 && diffY >= 0)
        {
            for (int x = 0; x <= diffX; x++)
            {
                for (int y = 0; y <= diffY; y++)
                {
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);
                    tileIsSelected.Add(tempTile);
                }
            }
        }

        if (diffX >= 0 && diffY < 0)
        {
            for (int x = 0; x <= diffX; x++)
            {
                for (int y = diffY; y <= 0; y++)
                {
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);
                    tileIsSelected.Add(tempTile);
                }
            }
        }

        if (diffX < 0 && diffY >= 0)
        {
            for (int x = diffX; x <= 0; x++)
            {
                for (int y = 0; y <= diffY; y++)
                {
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);
                    tileIsSelected.Add(tempTile);
                }
            }
        }

        if (diffX < 0 && diffY < 0)
        {
            for (int x = diffX; x <= 0; x++)
            {
                for (int y = diffY; y <= 0; y++)
                {
                    Tile tempTile = WorldController.Instance.GetTileAt(dragStartTile.x - x, dragStartTile.y - y);
                    tileIsSelected.Add(tempTile);
                }
            }
        }


        if (tileIsSelected.Count != 0)
        {
            if(MouseController.Instance.areaType == "Clear")
            {
                for (int i = 0; i < tileIsSelected.Count; i++)
                {
                    Tile t = tileIsSelected[i];
                    AreaController.Instance.RemoveTileFromArea(t);
                }
            }
            else
            {
                AreaController.Instance.CreateArea(MouseController.Instance.areaType, tileIsSelected);
            }
        }
    }
}
