using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectController : MonoBehaviour {

    public static ArchitectController Instance { get; protected set; }
    public GameObject AllWall { get; protected set; }
    public GameObject AllFurniture { get; protected set; }

    public Dictionary<Architecture, GameObject> archGameObjectMap { get; protected set; }

    public NeighbourCheck neighbourCheck { get; protected set; }


    void OnEnable()
    {
        Instance = this;

        AllWall = new GameObject();
        AllWall.transform.name = "Wall";

        AllFurniture = new GameObject();
        AllFurniture.transform.name = "Furniture";

        archGameObjectMap = new Dictionary<Architecture, GameObject>();
    }


    void Start ()
    {
        neighbourCheck = new NeighbourCheck();
    }
	

	void Update ()
    {
		
	}


    public void CreateArchAtTile(string categroy, string name, string type, Tile t)
    {
        Architecture arch_data = CreateArchPre(categroy, name, type, t);
        GameObject arch_obj = archGameObjectMap[arch_data];

        arch_data.tile = t;
        t.arch = arch_data;

        t.isWalkable = true;

        arch_obj.name += "_x_" + t.x + "_y_" + t.y;

        if(arch_data.category == "Structure")
        {
            neighbourCheck.StructureSpriteUpdate(t);
        }
    }


    public Architecture CreateArchPre(string categroy, string name, string type, Tile t)
    {
        Architecture arch_data = new Architecture(categroy, name, type, t);
        GameObject arch_obj = new GameObject();
        arch_data.stats = ArchStats.building;

        archGameObjectMap.Add(arch_data, arch_obj);

        arch_obj.name = categroy + type;
        arch_obj.transform.position = new Vector3(t.x, t.y, 0f);

        SpriteRenderer sr = arch_obj.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, 0.25f);

        switch (arch_data.category)
        {
            case "Structure":
                sr.sprite = WorldController.Instance.spriteloader.archSprite[categroy + neighbourCheck.StructureSpriteSet(arch_data)];
                sr.sortingLayerName = "Structure";
                arch_obj.transform.SetParent(AllWall.transform);
                break;

            case "Furniture":
                sr.sprite = WorldController.Instance.spriteloader.archSprite[categroy + name + type];
                sr.sortingLayerName = "Furniture";
                arch_obj.transform.SetParent(AllFurniture.transform);
                break;

            default:
                break;
        }

        return arch_data;
    }


    public void ArchWorkFinished(Tile t)
    {        
        if (t.arch != null && t.arch.stats == ArchStats.building)
        {
            if(t.arch.name == "Wall")
            {
                t.isWalkable = false;
            }

            t.arch.stats = ArchStats.done;
            SpriteRenderer sr = archGameObjectMap[t.arch].GetComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, 1f);

            RoomController.Instance.SplitRoom(t);
        }

        if (t.arch != null && t.arch.stats == ArchStats.destroying)
        {
            ItemController.Instance.CreateItemAtTile("Material", t.arch.type, t, 1);
            DestoryArch(t);

            RoomController.Instance.MergerRoom(t);
        }
    }


    public void DestoryArch(Tile t)
    {
        Destroy(archGameObjectMap[t.arch]);
        archGameObjectMap.Remove(t.arch);

        t.arch = null;
        t.isWalkable = true;

        neighbourCheck.StructureSpriteUpdate(t);
    }


    public void SetArchStatsDestroying(Architecture arch)
    {
        arch.stats = ArchStats.destroying;

        GameObject Icon = new GameObject();
        Icon.transform.name = "Icon";
        Icon.transform.position = archGameObjectMap[arch].transform.position;
        Icon.transform.SetParent(archGameObjectMap[arch].transform);

        SpriteRenderer sr = Icon.AddComponent<SpriteRenderer>();
        sr.sprite = WorldController.Instance.spriteloader.otherSprite["Destroy"];
        sr.sortingLayerName = "UI";
        sr.color = new Color(1f, 1f, 1f, 0.75f);
    }


    public void SetArchStatsDone(Architecture arch)
    {
        arch.stats = ArchStats.done;

        if(archGameObjectMap[arch].transform.Find("Icon").gameObject != null)
        {
            Destroy(archGameObjectMap[arch].transform.Find("Icon").gameObject);
        }
    } 
}
