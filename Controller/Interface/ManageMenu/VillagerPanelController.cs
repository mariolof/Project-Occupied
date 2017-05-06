using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillagerPanelController : MonoBehaviour {

    public static VillagerPanelController Instance { get; protected set; }
    public GameObject villagerInfo;
    public GameObject villagerPanel;
    public Dropdown workTypeDropdown;
    public Dropdown scheduleDropdwon;

    Dictionary<Human, GameObject> villagerInfoMap;
    Dictionary<Human, Dropdown> workTypeDropdownMap;
    Dictionary<Human, Dropdown> scheduleDropdownMap;

    GameObject villagerView;

    Text total;
    Text build;
    Text carry;
    Text farm;
    Text cook;
    Text care;
    Text produce;
    Text gather;
    Text guard;
    Text maintain;

    float currentPosY;


    void OnEnable()
    {
        Instance = this;

        villagerInfoMap = new Dictionary<Human, GameObject>();
        workTypeDropdownMap = new Dictionary<Human, Dropdown>();
        scheduleDropdownMap = new Dictionary<Human, Dropdown>();

        villagerView = villagerPanel.transform.Find("VillagerView").Find("Viewport").Find("Content").gameObject;
    }


    void Start ()
    {
        currentPosY = 0f;

        InitVillagerSummary();
        UpdateVillagerSummary();
    }
	

	void Update ()
    {

    }


    public void AddVillagerInfo(Human villager)
    {
        if (workTypeDropdownMap.ContainsKey(villager) == false)
        {
            GameObject info = Instantiate(villagerInfo, villagerView.transform);
            Dropdown workTypeDD = Instantiate(workTypeDropdown, info.transform);
            Dropdown scheduleDD = Instantiate(scheduleDropdwon, info.transform);

            info.transform.Find("Name").GetComponent<Text>().text = villager.name;
            info.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, currentPosY - 15f, 0f);
            info.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 30f);
            villagerInfoMap.Add(villager, info);

            workTypeDD.value = (int)Enum.Parse(typeof(JobType), villager.jobType.ToString(), true);
            workTypeDD.onValueChanged.AddListener(delegate { ChangeWorkType(villager); });
            workTypeDD.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -15f, 0f);
            workTypeDD.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 30f);
            workTypeDropdownMap.Add(villager, workTypeDD);

            scheduleDD.value = TimeController.Instance.scheduleList.IndexOf(villager.schedule);
            scheduleDD.onValueChanged.AddListener(delegate { ChangeSchedule(villager); });
            scheduleDD.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -15f, 0f);
            scheduleDD.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 30f);
            scheduleDropdownMap.Add(villager, scheduleDD);

            villagerView.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, villagerView.GetComponent<RectTransform>().sizeDelta.y + 45f);
            currentPosY = currentPosY - 45f;

            UpdateHunger(villager);
        }
    }


    public void AddScheduleDropDownOption(Schedule schedule)
    {
        List<string> newSchedule = new List<string>();
        newSchedule.Add("schedule" + TimeController.Instance.scheduleList.IndexOf(schedule).ToString());

        for (int i = 0; i < HumanController.Instance.villagerList.Count; i++)
        {
            Dropdown DD = scheduleDropdownMap[HumanController.Instance.villagerList[i]];
            DD.AddOptions(newSchedule);
        }
    }


    public void UpdateHunger(Human villager)
    {
        villagerInfoMap[villager].transform.Find("Hunger").GetComponent<Text>().text = villager.hunger.ToString() + "/100";
    }


    void InitVillagerSummary()
    {
        total = villagerPanel.transform.Find("VillagerSummary").Find("Total").GetComponent<Text>();
        build = villagerPanel.transform.Find("VillagerSummary").Find("Build").GetComponent<Text>();
        carry = villagerPanel.transform.Find("VillagerSummary").Find("Carry").GetComponent<Text>();
        farm = villagerPanel.transform.Find("VillagerSummary").Find("Farm").GetComponent<Text>();
        cook = villagerPanel.transform.Find("VillagerSummary").Find("Cook").GetComponent<Text>();
        care = villagerPanel.transform.Find("VillagerSummary").Find("Care").GetComponent<Text>();
        produce = villagerPanel.transform.Find("VillagerSummary").Find("Produce").GetComponent<Text>();
        gather = villagerPanel.transform.Find("VillagerSummary").Find("Gather").GetComponent<Text>();
        guard = villagerPanel.transform.Find("VillagerSummary").Find("Guard").GetComponent<Text>();
        maintain = villagerPanel.transform.Find("VillagerSummary").Find("Maintain").GetComponent<Text>();
    }


    void ChangeWorkType(Human villager)
    {
        Dropdown Dropdown = workTypeDropdownMap[villager];

        switch (Dropdown.value)
        {
            case 0:
                villager.jobType = JobType.build;
                break;

            case 1:
                villager.jobType = JobType.carry;
                break;

            case 2:
                villager.jobType = JobType.farm;
                break;

            case 3:
                villager.jobType = JobType.cook;
                break;

            case 4:
                villager.jobType = JobType.care;
                break;

            case 5:
                villager.jobType = JobType.produce;
                break;

            case 6:
                villager.jobType = JobType.gather;
                break;

            case 7:
                villager.jobType = JobType.guard;
                break;

            case 8:
                villager.jobType = JobType.maintain;
                break;

            default:
                Debug.Log("Unknow JobType");
                break;
        }

        UpdateVillagerSummary();
    }


    void ChangeSchedule(Human villager)
    {
        Dropdown dropdown = scheduleDropdownMap[villager];

        switch (dropdown.value)
        {
            case 0:
                villager.schedule = TimeController.Instance.scheduleList[0];
                break;
            case 1:
                villager.schedule = TimeController.Instance.scheduleList[1];
                break;
            case 2:
                villager.schedule = TimeController.Instance.scheduleList[2];
                break;
            case 3:
                villager.schedule = TimeController.Instance.scheduleList[2];
                break;
            case 4:
                villager.schedule = TimeController.Instance.scheduleList[4];
                break;
            default:
                break;
        }
    }


    void UpdateVillagerSummary()
    {
        int totalCount = HumanController.Instance.villagerList.Count;
        int buildCount = 0;
        int carryCount = 0;
        int farmCount = 0;
        int cookCount = 0;
        int careCount = 0;
        int produceCount = 0;
        int guardCount = 0;
        int gatherCount = 0;
        int maintainCount = 0;

        for (int i = 0; i < HumanController.Instance.villagerList.Count; i++)
        {
            Human villager = HumanController.Instance.villagerList[i];
            switch (villager.jobType)
            {
                case JobType.build:
                    buildCount += 1;
                    break;

                case JobType.carry:
                    carryCount += 1;
                    break;

                case JobType.farm:
                    farmCount += 1;
                    break;

                case JobType.cook:
                    cookCount += 1;
                    break;

                case JobType.care:
                    careCount += 1;
                    break;

                case JobType.produce:
                    produceCount += 1;
                    break;

                case JobType.gather:
                    gatherCount += 1;
                    break;

                case JobType.guard:
                    guardCount += 1;
                    break;

                case JobType.maintain:
                    maintainCount += 1;
                    break;

                default:
                    break;
            }
        }

        total.text = "Total:" + totalCount.ToString();
        build.text = "Build:" + buildCount.ToString();
        carry.text = "Carry:" + carryCount.ToString();
        farm.text = "Farm:" + farmCount.ToString();
        cook.text = "Cook:" + cookCount.ToString();
        care.text = "Care:" + careCount.ToString();
        produce.text = "Produce:" + produceCount.ToString();
        gather.text = "Gather:" + gatherCount.ToString();
        guard.text = "Guard:" + guardCount.ToString();
        maintain.text = "Maintain:" + maintainCount.ToString();
    }
}
