using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour {

    public static PlantController Instance { get; protected set; }
    public GameObject AllPlant { get; protected set; }

    public List<Plant> cropList { get; protected set; }

    public Dictionary<Plant, GameObject> plantGameObjectMap { get; protected set; }


    void OnEnable()
    {
        Instance = this;

        AllPlant = new GameObject();
        AllPlant.transform.name = " AllPlant";
    }


    void Start ()
    {
        plantGameObjectMap = new Dictionary<Plant, GameObject>();

        cropList = new List<Plant>();
    }
	

	void Update ()
    {

    }


    public void CreatePlantAtTile(string category, string name, Tile t)
    {
        Plant plant_data = CreatePlant(category, name, t);

        plant_data.tile = t;
        t.plant = plant_data;
    }


    public Plant CreatePlant(string category, string name, Tile t)
    {
        Plant plant_data = new Plant(category, name);
        GameObject plant_obj = new GameObject();
        plant_data.currentStage = 0;

        cropList.Add(plant_data);
        plantGameObjectMap.Add(plant_data, plant_obj);

        plant_obj.transform.name = category + name;
        plant_obj.transform.position = new Vector3(t.x, t.y, 0f);

        SpriteRenderer sr = plant_obj.AddComponent<SpriteRenderer>();

        switch (plant_data.catagory)
        {
            case "Crop":
                sr.sprite = WorldController.Instance.spriteloader.plantSprite[plant_data.catagory + plant_data.name + plant_data.currentStage.ToString()];
                sr.sortingLayerName = "Plant";
                plant_obj.transform.SetParent(AllPlant.transform);
                break;

            case "Wild":
                break;

            default:
                break;
        }

        return plant_data;
    }


    public void PlantGrow()
    {
        for (int i = 0; i < cropList.Count; i++)
        {
            Plant crop = cropList[i];

            if (crop.currentGrowHour < crop.growHour)
            {
                crop.currentGrowHour += 1;
            }
            else if (crop.currentGrowHour >= crop.growHour && crop.currentStage < crop.maxStage)
            {
                crop.currentStage += 1;
                SpriteRenderer sr = plantGameObjectMap[crop].GetComponent<SpriteRenderer>();
                sr.sprite = WorldController.Instance.spriteloader.plantSprite[crop.catagory + crop.name + crop.currentStage];

                crop.currentGrowHour = 0;
            }
            else if (crop.tile.jobOnTile == null && crop.currentStage == crop.maxStage)
            {
                Debug.Log("ready to havest");
                JobController.Instance.CreateJob(JobType.farm, crop.tile, 25);
            }
        }
    }


    public void HarvestPlant(Tile tile)
    {
        if (tile.plant != null)
        {
            Destroy(plantGameObjectMap[tile.plant]);
            plantGameObjectMap.Remove(tile.plant);
            cropList.Remove(tile.plant);

            ItemController.Instance.CreateItemAtTile("Food", tile.plant.name, tile, 5);

            tile.plant = null;
        }
    }


    public void EmptyFarmLandCheck()
    {
        for (int i = 0; i < AreaController.Instance.FarmlandLsit.Count; i++)
        {
            Area farmland = AreaController.Instance.FarmlandLsit[i];

            for (int j = 0; j < farmland.tileInArea.Count; j++)
            {
                Tile t = farmland.tileInArea[j];

                if (t.jobOnTile == null && t.plant == null)
                {
                    JobController.Instance.CreateJob(JobType.farm, t, 25);
                }
            }
        }
    }
}
