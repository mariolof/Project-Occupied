using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour {

    public GameObject TileInfoPanel;
    public GameObject TimePanel;
    public GameObject SelectHumanPanel;
    public GameObject PauseText;

    public Tile tileUnderMouse { get; protected set; }

    public string tileInfo { get; protected set; }
    public string tileRoomInfo { get; protected set; }
    public string tileAreaInfo { get; protected set; }
    public string tileArchInfo { get; protected set; }
    public string tileItemInfo { get; protected set; }
    public string tilePlantInfo { get; protected set; }


    void Start ()
    {
		
	}


	void Update ()
    {
        ShowTileInfo();
        ShowTimeInfo();
        ShowHumanInfo();

        PausedText();
    }


    public void ShowTileInfo()
    {
        tileUnderMouse = MouseController.Instance.tileUnderMouse;

        if(tileUnderMouse != null)
        {
            tileInfo = " " + tileUnderMouse.type + " x:" + tileUnderMouse.x + " y:" + tileUnderMouse.y;
            TileInfoPanel.transform.Find("Type").GetComponent<Text>().text = tileInfo;

            if (tileUnderMouse.room == null)
            {
                tileRoomInfo = " Outdoor";
            }
            else
            {
                tileRoomInfo = " Room:" + RoomController.Instance.roomList.IndexOf(tileUnderMouse.room).ToString();
            }
            TileInfoPanel.transform.Find("Room").GetComponent<Text>().text = tileRoomInfo;

            if (tileUnderMouse.area == null)
            {
                tileAreaInfo = " Not in Area";
            }
            else
            {
                tileAreaInfo = " Area:" + AreaController.Instance.AreaList.IndexOf(tileUnderMouse.area).ToString() + " " + tileUnderMouse.area.type;
            }
            TileInfoPanel.transform.Find("Area").GetComponent<Text>().text = tileAreaInfo;

            if (tileUnderMouse.arch == null)
            {
                tileArchInfo = " No Arch on Tile";
            }
            else
            {
                tileArchInfo = " " + tileUnderMouse.arch.name + " " + tileUnderMouse.arch.type;
            }
            TileInfoPanel.transform.Find("Arch").GetComponent<Text>().text = tileArchInfo;

            if (tileUnderMouse.item == null)
            {
                tileItemInfo = " No Item On tile";
            }
            else
            {
                tileItemInfo = " " + tileUnderMouse.item.type + " " + tileUnderMouse.item.currentStack.ToString();
            }
            TileInfoPanel.transform.Find("Item").GetComponent<Text>().text = tileItemInfo;

            if (tileUnderMouse.plant == null)
            {
                tilePlantInfo = " No Plant On tile";
            }
            else
            {
                tilePlantInfo = " " + tileUnderMouse.plant.name + " stage " + tileUnderMouse.plant.currentStage.ToString();
            }
            TileInfoPanel.transform.Find("Plant").GetComponent<Text>().text = tilePlantInfo;
        }
    }


    public void ShowTimeInfo()
    {
        TimePanel.transform.Find("Canlendar").GetComponent<Text>().text = TimeController.Instance.year.ToString() + ". " + TimeController.Instance.month.ToString() + ". " + TimeController.Instance.date.ToString();
        TimePanel.transform.Find("Time").GetComponent<Text>().text = "Current Hour: " + Mathf.FloorToInt(TimeController.Instance.hour).ToString();
        TimePanel.transform.Find("Schedule").GetComponent<Text>().text = TimeController.Instance.scheduleList[0].scheduleMap[TimeController.Instance.hour].ToString();
        TimePanel.transform.Find("GameSpeed").GetComponent<Text>().text = "Speed: " + Time.timeScale.ToString();
    }


    public void ShowHumanInfo()
    {
        if(HumanController.Instance.selectedHumanList.Count == 1)
        {
            SelectHumanPanel.transform.Find("Name").GetComponent<Text>().text = "Name_" + HumanController.Instance.selectedHumanList[0].name;
            SelectHumanPanel.transform.Find("IsWorking").GetComponent<Text>().text = "IsWorking_" + HumanController.Instance.selectedHumanList[0].isWorking.ToString();
            SelectHumanPanel.transform.Find("JobType").GetComponent<Text>().text = "JobType_" + HumanController.Instance.selectedHumanList[0].jobType.ToString();
            SelectHumanPanel.transform.Find("Need").GetComponent<Text>().text = "Need_" + (HumanController.Instance.selectedHumanList[0].need != null).ToString();
            SelectHumanPanel.transform.Find("JobQueue").GetComponent<Text>().text = "JobQueue_" + (HumanController.Instance.selectedHumanList[0].currentJobQueue != null).ToString();
            SelectHumanPanel.transform.Find("Schedule").GetComponent<Text>().text = "Schedule_" + TimeController.Instance.scheduleList.IndexOf(HumanController.Instance.selectedHumanList[0].schedule).ToString();

            if (HumanController.Instance.selectedHumanList[0].targetTile != null)
            {
                SelectHumanPanel.transform.Find("Target").GetComponent<Text>().text = "Target_x" + HumanController.Instance.selectedHumanList[0].targetTile.x.ToString() + "_y" + HumanController.Instance.selectedHumanList[0].targetTile.y.ToString();
            }
            else
            {
                SelectHumanPanel.transform.Find("Target").GetComponent<Text>().text = "No Target";
            }
            
            if (HumanController.Instance.selectedHumanList[0].pathAI.currentPath != null)
            {
                SelectHumanPanel.transform.Find("PathAI").GetComponent<Text>().text = "PathAI_Num_" + HumanController.Instance.selectedHumanList[0].pathAI.currentPath.Count.ToString();
            }
            else
            {
                SelectHumanPanel.transform.Find("PathAI").GetComponent<Text>().text = "PathAI_Num_0";
            }            
        }
        else if(HumanController.Instance.selectedHumanList.Count != 0)
        {
            SelectHumanPanel.transform.Find("Name").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("IsWorking").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("JobType").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("Need").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("JobQueue").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("Schedule").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("Target").GetComponent<Text>().text = "multi_selected";
            SelectHumanPanel.transform.Find("PathAI").GetComponent<Text>().text = "multi_selected";
        }
        else
        {
            SelectHumanPanel.transform.Find("Name").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("IsWorking").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("JobType").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("Need").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("JobQueue").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("Schedule").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("Target").GetComponent<Text>().text = "";
            SelectHumanPanel.transform.Find("PathAI").GetComponent<Text>().text = "";
        }
    }


    public void PausedText()
    {
        if (Time.timeScale == 0)
        {
            PauseText.SetActive(true);
        }
        else
        {
            PauseText.SetActive(false);
        }
    }
}
