using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchedulePanelController : MonoBehaviour {

    public static SchedulePanelController Instance { get; protected set; }
    public GameObject schedulePanel;
    public GameObject scheduleInfo;
    public Button scheduleButton;

    Dictionary<Schedule, GameObject> scheduleMap;
    Dictionary<Button, int> buttonMap;

    GameObject scheduleView;

    ScheduleType currentType;


    void OnEnable()
    {
        Instance = this;

        scheduleMap = new Dictionary<Schedule, GameObject>();
        buttonMap = new Dictionary<Button, int>();

        scheduleView = schedulePanel.transform.Find("ScheduleView").gameObject;
    }


    void Start ()
    {
        currentType = ScheduleType.Free;
    }
	

	void Update ()
    {
		
	}


    public void AddScheduleInfo(Schedule schedule)
    {
        if (scheduleMap.ContainsKey(schedule) == false)
        {
            GameObject info = Instantiate(scheduleInfo, scheduleView.transform);
            scheduleMap.Add(schedule, info);

            for (int i = 0; i < 24; i++)
            {
                Button button = Instantiate(scheduleButton, info.transform);
                button.transform.name = i.ToString();
                button.transform.Find("Text").GetComponent<Text>().text = schedule.scheduleMap[i].ToString();
                button.onClick.AddListener(delegate { ChangeScheduleType(schedule, button); });
                buttonMap.Add(button, i);
            }
        }
    }


    public void SelectType(int n)
    {
        if (n == 0)
        {
            currentType = ScheduleType.Work;
        }

        if (n == 1)
        {
            currentType = ScheduleType.Free;
        }

        if (n == 2)
        {
            currentType = ScheduleType.Rest;
        }
    }


    void ChangeScheduleType(Schedule schedule, Button button)
    {
        int n = buttonMap[button];
        schedule.scheduleMap[n] = currentType;
        button.transform.Find("Text").GetComponent<Text>().text = currentType.ToString();
    }
}
