using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public static MouseController Instance { get; protected set; }

    public GameObject Icon;
    public GameObject SelectRec;

    public Vector3 mousePosition { get; protected set; }
    public Tile tileUnderMouse { get; protected set; }

    public bool isBuildMode { get; protected set; }
    public BuildMode currentBuildMode { get; protected set; }

    public string currentBuildCategory { get; protected set; }
    public string currentBuildName { get; protected set; }

    public string structureType { get; protected set; }
    public string furnitureType { get; protected set; }
    public string areaType { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        isBuildMode = false;

        currentBuildCategory = null;
        currentBuildMode = BuildMode.floor;
    }
	

	void Update ()
    {
		mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        tileUnderMouse = WorldController.Instance.GetTileAtPos(mousePosition);


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ExitBuildMode();
        }


        if (isBuildMode == false)
        {
            FreeModeController.Instance.CursorDrag();
            FreeModeController.Instance.ClickSetTarget();
            FreeModeController.Instance.BuildJobCancel();
        }

        if (isBuildMode == true)
        {
            BuildModeController.Instance.BuildJobCreate(currentBuildMode);
        }
    }


    public void SetBuildName(string name)
    {
        currentBuildName = name;
    }


    public void ExitBuildMode()
    {
        isBuildMode = false;
        SelectRec.SetActive(false);
        SelectRec.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        BuildModeController.Instance.ResetFurniturePreview();
    }


    public void OnDestroyMode()
    {
        isBuildMode = true;
        currentBuildMode = BuildMode.destroy;
        SelectRec.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 0.75f);
        HumanController.Instance.ClearSelectHumanList();
    }

    public void OnAreaMode(string type)
    {
        isBuildMode = true;
        areaType = type;
        currentBuildMode = BuildMode.area;
        SelectRec.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 1f, 0.75f);
        HumanController.Instance.ClearSelectHumanList();
    }

    public void OnFloorMode()
    {
        isBuildMode = true;
        currentBuildMode = BuildMode.floor;
        SelectRec.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1f, 0.25f, 0.75f);
        HumanController.Instance.ClearSelectHumanList();
    }

    public void OnStructureMode(string type)
    {
        isBuildMode = true;
        structureType = type;
        currentBuildCategory = "Structure";
        currentBuildMode = BuildMode.structure;
        SelectRec.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1f, 0.25f, 0.75f);
        HumanController.Instance.ClearSelectHumanList();
    }

    public void OnFurnitureMode(string type)
    {
        isBuildMode = true;
        furnitureType = type;
        currentBuildCategory = "Furniture";
        currentBuildMode = BuildMode.furniture;
        BuildModeController.Instance.ResetFurniturePreview();
        HumanController.Instance.ClearSelectHumanList();
    }
}
