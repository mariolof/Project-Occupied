using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedController : MonoBehaviour {

    public static NeedController Instance { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
		
	}
	

	void Update ()
    {
        ExecuteNeed();
    }


    public void ReduceHunger()
    {
        for (int i = 0; i < HumanController.Instance.villagerList.Count; i++)
        {
            Human villager = HumanController.Instance.villagerList[i];
            int num = Random.Range(3, 5);
            villager.hunger -= num;

            if (villager.hunger < 0)
            {
                villager.hunger = 0;
            }
            else if (villager.hunger < 25)
            {
                if (villager.need == null)
                {
                    CreateNeed(NeedType.Hunger, villager);
                }
            }
            else if (villager.hunger < 50)
            {
                if (villager.need == null && villager.currentJobQueue == null && villager.schedule.scheduleMap[TimeController.Instance.hour] != ScheduleType.Rest)
                {
                    CreateNeed(NeedType.Hunger, villager);
                }
            }

            VillagerPanelController.Instance.UpdateHunger(villager);
        }
    }


    public void CreateNeed(NeedType type, Human h)
    {
        Need need = new Need(type, h);
        h.need = need;
    }


    public void ExecuteNeed()
    {
        for (int i = 0; i < HumanController.Instance.villagerList.Count; i++)
        {
            Human h = HumanController.Instance.villagerList[i];
            
            if(h.need != null && h.isWorking == false)
            {
                if (h.need.tile == null)
                {
                    switch (h.need.type)
                    {
                        case NeedType.Hunger:
                            FindFood(h);
                            break;
                        case NeedType.Energy:
                            break;
                        case NeedType.Happiness:
                            break;
                        default:
                            break;
                    }
                }
                else if (HumanController.Instance.humanGameObjectMap[h].transform.position == new Vector3(h.need.tile.x, h.need.tile.y, 0f))
                {
                    switch (h.need.type)
                    {
                        case NeedType.Hunger:
                            EatFood(h);
                            break;
                        case NeedType.Energy:
                            break;
                        case NeedType.Happiness:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }


    void FindFood(Human h)
    {
        for (int i = 0; i < ItemController.Instance.foodList.Count; i++)
        {
            Item food = ItemController.Instance.foodList[i];

            if (food.registeredJob == null && food.registeredNeed == null && food.carrier == null && food.tile != null)
            {
                h.need.tile = food.tile;
                food.registeredNeed = h.need;
                MoveController.Instance.SetPath(h.need.tile, h);
                break;
            }
        }
    }


    void EatFood(Human h)
    {
        Debug.Log(h.name + " Found Food");

        h.need.tile.item.registeredNeed = null;
        h.need.tile.item.currentStack -= 1;

        if (h.need.tile.item.currentStack <= 0)
        {
            ItemController.Instance.DestoryItem(h.need.tile.item);
            h.need.tile.item = null;
        }
        else
        {
            ItemController.Instance.itemGameobjectMap[h.need.tile.item].transform.GetComponentInChildren<Text>().text = h.need.tile.item.currentStack.ToString();
        }

        h.hunger += 50;
        h.need = null;
    }
}
