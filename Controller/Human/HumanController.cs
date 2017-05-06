using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {

    public static HumanController Instance { get; protected set; }
    public GameObject AllHuman;

    public List<Human> humanList { get; protected set; }
    public List<Human> heroList { get; protected set; }
    public List<Human> villagerList { get; protected set; }

    public List<Human> selectedHumanList { get; protected set; }
    public List<Human> workerList { get; protected set; }

    public Dictionary<Human, GameObject> humanGameObjectMap { get; protected set; }


    void OnEnable ()
    {
        Instance = this;

        AllHuman = new GameObject();
        AllHuman.transform.name = "Human";

        humanGameObjectMap = new Dictionary<Human, GameObject>();

        humanList = new List<Human>();
        heroList = new List<Human>();
        villagerList = new List<Human>();

        selectedHumanList = new List<Human>();
        workerList = new List<Human>();
    }


    void Start()
    {
        CreateHuman(50, 50, "Mario", "Hero", "Hero", Faction.player);
        CreateHuman(50, 49, "Olof", "Hero", "Hero", Faction.player);

        CreateHuman(47, 50, "Lucas", "Regular", "Regular", Faction.village_0);
        CreateHuman(48, 50, "Tom", "Regular", "Regular", Faction.village_0);
        CreateHuman(47, 49, "Betty", "Regular", "Regular", Faction.village_0);
        CreateHuman(48, 49, "Martin", "Regular", "Regular", Faction.village_0);
        CreateHuman(47, 48, "Lily", "Regular", "Regular", Faction.village_0);
        CreateHuman(48, 48, "Zeus", "Regular", "Regular", Faction.village_0);
        CreateHuman(47, 47, "Sam", "Regular", "Regular", Faction.village_0);
        CreateHuman(48, 47, "Kenny", "Regular", "Regular", Faction.village_0);
        CreateHuman(47, 46, "Huang", "Regular", "Regular", Faction.village_0);
        CreateHuman(48, 46, "Kevin", "Regular", "Regular", Faction.village_0);
    }


	void Update ()
    {
        BehaviourCheck();       
        
        MoveController.Instance.Wonder();
        MoveController.Instance.CheckInPosition();
        MoveController.Instance.Move(2f);
	}


    public void CreateHuman(float x, float y, string n, string fT, string bT, Faction f)
    {
        GameObject human_obj = new GameObject();
        Human human_data = new Human(n, fT, bT, f);

        humanGameObjectMap.Add(human_data, human_obj);
        human_obj.name = "Human_" + human_data.name;
        human_obj.transform.position = new Vector3(x, y, 0f);
        human_obj.transform.SetParent(AllHuman.transform);

        human_data.schedule = TimeController.Instance.scheduleList[0];
        human_data.jobType = JobType.carry;
        HumanFactionCheck(human_data);

        SpriteRenderer sr = human_obj.AddComponent<SpriteRenderer>();
        sr.sprite = WorldController.Instance.spriteloader.humanSprite[human_data.bodyType];
        sr.sortingLayerName = "Human";

        GameObject selectIcon = new GameObject();
        selectIcon.SetActive(false);
        selectIcon.transform.name = "Icon";
        selectIcon.transform.position = human_obj.transform.position;
        selectIcon.transform.SetParent(human_obj.transform);
        SpriteRenderer sr_Icon = selectIcon.AddComponent<SpriteRenderer>();
        sr_Icon.sprite = Resources.Load<Sprite>("Sprite/UI/Icon");
        sr_Icon.sortingLayerName = "UI";
    }


    public void BehaviourCheck()
    {
        for (int i = 0; i < villagerList.Count; i++)
        {
            Human villager = villagerList[i];

            switch (villager.schedule.scheduleMap[TimeController.Instance.hour])
            {
                case ScheduleType.Work:
                    WorkTime(villager);
                    break;
                case ScheduleType.Free:
                    FreeTime(villager);
                    break;
                case ScheduleType.Rest:
                    RestTime(villager);
                    break;
                default:
                    break;
            }
        }
    }


    public void WorkTime(Human villager)
    {
        if(villager.isWorking == false && villager.currentJobQueue == null)
        {
            switch (villager.jobType)
            {
                case JobType.build:
                    JobBuildController.Instance.CreatJobQueue(villager);
                    break;
                case JobType.carry:
                    JobCarryController.Instance.CreatJobQueue(villager);
                    break;
                case JobType.farm:
                    JobFarmController.Instance.CreatJobQueue(villager);
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
    }


    public void FreeTime(Human villager)
    {
        if (villager.isWorking == false && villager.currentJobQueue == null && villager.need == null)
        {
            switch (villager.jobType)
            {
                case JobType.build:
                    JobBuildController.Instance.CreatJobQueue(villager);
                    break;
                case JobType.carry:
                    JobCarryController.Instance.CreatJobQueue(villager);
                    break;
                case JobType.farm:
                    JobFarmController.Instance.CreatJobQueue(villager);
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
    }


    public void RestTime(Human villager)
    {
        if (villager.isWorking == false && villager.currentJobQueue == null && villager.need == null)
        {

        }
    }


    public void SwitchHumanSelected(Human human_data, bool s)
    {
        GameObject human_obj = humanGameObjectMap[human_data];

        if (s == true)
        {
            human_obj.transform.FindChild("Icon").gameObject.SetActive(true);
            human_data.isSelceted = true;

            if (selectedHumanList.Contains(human_data) == false)
            {
                selectedHumanList.Add(human_data);
            }
        }
        else
        {
            human_obj.transform.FindChild("Icon").gameObject.SetActive(false);
            human_data.isSelceted = false;

            if (selectedHumanList.Contains(human_data) == true)
            {
                selectedHumanList.Remove(human_data);
            }
        }
    }


    public void ClearSelectHumanList()
    {
        for (int i = 0; i < selectedHumanList.Count; i++)
        {
            Human human_data = selectedHumanList[i];

            human_data.isSelceted = false;
            GameObject human_obj = humanGameObjectMap[human_data];
            human_obj.transform.FindChild("Icon").gameObject.SetActive(false);
        }

        selectedHumanList.Clear();
    }


    void HumanFactionCheck(Human human_data)
    {
        switch (human_data.faction)
        {
            case Faction.player:
                humanList.Add(human_data);
                workerList.Add(human_data);
                heroList.Add(human_data);
                break;

            case Faction.village_0:
                humanList.Add(human_data);
                workerList.Add(human_data);
                villagerList.Add(human_data);
                VillagerPanelController.Instance.AddVillagerInfo(human_data);
                break;

            case Faction.invader:
                break;

            case Faction.guerilla:
                break;

            case Faction.puppet:
                break;

            case Faction.town_1:
                break;

            case Faction.town_2:
                break;

            case Faction.village_1:
                break;

            case Faction.village_2:
                break;

            case Faction.village_3:
                break;

            case Faction.village_4:
                break;

            case Faction.village_5:
                break;

            default:
                humanList.Add(human_data);
                break;
        }
    }
}
