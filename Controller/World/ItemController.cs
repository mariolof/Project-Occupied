using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

    public static ItemController Instance { get; protected set; }
    public GameObject stackNumberUI;
    public GameObject AllItem;

    public List<Item> itemList { get; protected set; }
    public List<Item> toStorageList { get; protected set; }
    public List<Item> materialList { get; protected set; }
    public List<Item> foodList { get; protected set; }

    public Dictionary<Item, GameObject> itemGameobjectMap { get; protected set; }


    void OnEnable()
    {
        Instance = this;

        AllItem = new GameObject();
        AllItem.transform.name = "Item";
    }


    void Start ()
    {
        itemGameobjectMap = new Dictionary<Item, GameObject>();

        itemList = new List<Item>();
        toStorageList = new List<Item>();
        materialList = new List<Item>();
        foodList = new List<Item>();

        CreateItemAtTile("Material", "Stone", WorldController.Instance.GetTileAt(52, 50), 20);
        CreateItemAtTile("Material", "Stone", WorldController.Instance.GetTileAt(52, 49), 20);
        CreateItemAtTile("Material", "Stone", WorldController.Instance.GetTileAt(52, 48), 20);
        CreateItemAtTile("Material", "Stone", WorldController.Instance.GetTileAt(52, 47), 20);
        CreateItemAtTile("Material", "Stone", WorldController.Instance.GetTileAt(52, 46), 20);

        CreateItemAtTile("Material", "Dirt", WorldController.Instance.GetTileAt(53, 50), 20);
        CreateItemAtTile("Material", "Dirt", WorldController.Instance.GetTileAt(53, 49), 20);
        CreateItemAtTile("Material", "Dirt", WorldController.Instance.GetTileAt(53, 48), 20);
        CreateItemAtTile("Material", "Dirt", WorldController.Instance.GetTileAt(53, 47), 20);
        CreateItemAtTile("Material", "Dirt", WorldController.Instance.GetTileAt(53, 46), 20);

        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(50, 55), 10);
        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(50, 56), 10);
        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(51, 55), 10);
        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(51, 56), 10);
        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(52, 55), 10);
        CreateItemAtTile("Food", "Rice", WorldController.Instance.GetTileAt(52, 56), 10);
    }
	

	void Update ()
    {

    }


    public void CreateItemAtTile(string category, string type, Tile t, int n)
    {
        Vector3 pos = new Vector3(t.x, t.y, 0f);
        Item item_data = CreateItem(category, type, pos, n);

        item_data.tile = t;
        t.item = item_data;
    }


    public Item CreateItem(string category, string type, Vector3 pos, int n)
    {
        GameObject item_obj = new GameObject(); 
        Item item_data = new Item(category, type, n);

        itemList.Add(item_data);
        itemGameobjectMap.Add(item_data, item_obj);

        item_obj.name = category + type;
        item_obj.transform.position = pos;
        item_obj.transform.SetParent(AllItem.transform);

        SpriteRenderer sr = item_obj.AddComponent<SpriteRenderer>();

        switch (item_data.category)
        {
            case "Material":
                sr.sprite = WorldController.Instance.spriteloader.itemSprite[item_data.category + item_data.type];
                sr.sortingLayerName = "Item";
                item_data.isStackable = true;
                item_data.maxStack = 25;
                materialList.Add(item_data);
                break;

            case "Food":
                sr.sprite = WorldController.Instance.spriteloader.itemSprite[item_data.category + item_data.type];
                sr.sortingLayerName = "Item";
                item_data.isStackable = true;
                item_data.maxStack = 10;
                foodList.Add(item_data);
                break;

            default:
                Debug.Log("Unknow Item");
                break;
        }

        if(item_data.isStackable == true)
        {
            GameObject numUI_obj = Instantiate(stackNumberUI);
            numUI_obj.transform.SetParent(item_obj.transform);
            numUI_obj.transform.localPosition = new Vector3(0f, 0f, 0f);
            numUI_obj.transform.Find("Text").GetComponent<Text>().text = item_data.currentStack.ToString();
        }

        return item_data;
    }


    public void DestoryItem(Item item_data)
    {
        Destroy(itemGameobjectMap[item_data]);
        itemList.Remove(item_data);
        itemGameobjectMap.Remove(item_data);

        if (toStorageList.Contains(item_data))
        {
            toStorageList.Remove(item_data);
        }

        if (materialList.Contains(item_data))
        {
            materialList.Remove(item_data);
        }

        if (foodList.Contains(item_data))
        {
            foodList.Remove(item_data);
        }
    }


    public void ItemCarryQuest()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];

            if (item.tile != null)
            {
                if ((item.tile.area == null || item.tile.area.type != "Storage") && item.registeredJob == null)
                {
                    JobController.Instance.CreateJob(JobType.carry, item.tile, 5);
                }
            }
        }
    }
}
