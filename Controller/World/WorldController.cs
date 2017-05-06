using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public static WorldController Instance { get; protected set; }

    public World currentWorld { get; protected set; }
    public Dictionary<Tile, GameObject> tileGameObjectMap { get; protected set; }

    public SpriteLoader spriteloader { get; protected set; }


    void OnEnable()
    {
        Instance = this;

        tileGameObjectMap = new Dictionary<Tile, GameObject>();

        spriteloader = new SpriteLoader();
        spriteloader.loadTileSprite();
        spriteloader.loadHumanSprite();
        spriteloader.loadArchitectureSprite();
        spriteloader.loadItemSprite();
        spriteloader.loadPlantSprite();
        spriteloader.loadOtherSprite();

        CreateWorld();
    }


    void Start ()
    {

    }
	

	void Update ()
    {
        AreaController.Instance.ClaerInvalidArea();
        AreaController.Instance.ClearIsolatedTile();

        RoomController.Instance.ClearEmptyRoom();

        ItemController.Instance.ItemCarryQuest();

        PlantController.Instance.EmptyFarmLandCheck();
    }


    public void CreateWorld()
    {
        currentWorld = new World(100, 100);
                
        for (int x = 0; x < currentWorld.width; x++)
        {
            for (int y = 0; y < currentWorld.length; y++)
            {
                Tile tile_data = GetTileAt(x, y);
                GameObject tile_obj = new GameObject();

                tileGameObjectMap.Add(tile_data, tile_obj);

                tile_obj.name = "tile_x" + x + "_y" + y;
                tile_obj.transform.SetParent(transform, true);
                tile_obj.transform.position = new Vector3(tile_data.x, tile_data.y, 0f);

                int num_r = Random.Range(0, 15);               

                SpriteRenderer sr = tile_obj.AddComponent<SpriteRenderer>();
                sr.sprite = spriteloader.tileSprite[tile_data.type + "_" + num_r];
                sr.sortingLayerName = "Ground";
            }
        }
    }


    public Tile GetTileAt(int X, int Y)
    {
        if (X > currentWorld.width - 1 || X < 0 || Y > currentWorld.length - 1 || Y < 0)
        {
            //Debug.Log("tile is out of range");
            return null;
        }

        return currentWorld.tiles[X, Y];
    }


    public Tile GetTileAtPos(Vector3 Pos)
    {
        int x = Mathf.FloorToInt(Pos.x + 0.5f);
        int y = Mathf.FloorToInt(Pos.y + 0.5f);

        return GetTileAt(x, y);
    }


    public Tile GetNeighbourN(Tile t)
    {
        Tile neighbour = GetTileAt(t.x, t.y + 1);
        return neighbour;
    }

    public Tile GetNeighbourS(Tile t)
    {
        Tile neighbour = GetTileAt(t.x, t.y - 1);
        return neighbour;
    }

    public Tile GetNeighbourE(Tile t)
    {
        Tile neighbour = GetTileAt(t.x + 1, t.y);
        return neighbour;
    }

    public Tile GetNeighbourW(Tile t)
    {
        Tile neighbour = GetTileAt(t.x - 1, t.y);
        return neighbour;
    }

    public Tile GetNeighbourNE(Tile t)
    {
        Tile neighbour = GetTileAt(t.x + 1, t.y + 1);
        return neighbour;
    }

    public Tile GetNeighbourNW(Tile t)
    {
        Tile neighbour = GetTileAt(t.x - 1, t.y + 1);
        return neighbour;
    }

    public Tile GetNeighbourSE(Tile t)
    {
        Tile neighbour = GetTileAt(t.x + 1, t.y - 1);
        return neighbour;
    }

    public Tile GetNeighbourSW(Tile t)
    {
        Tile neighbour = GetTileAt(t.x - 1, t.y - 1);
        return neighbour;
    }
}
