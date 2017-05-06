using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour {

    public static AreaController Instance { get; protected set; }

    public List<Area> AreaList { get; protected set; }
    public List<Area> StorageLsit { get; protected set; }
    public List<Area> FarmlandLsit { get; protected set; }

    public NeighbourCheck neighbourCheck { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        AreaList = new List<Area>();
        StorageLsit = new List<Area>();
        FarmlandLsit = new List<Area>();

        neighbourCheck = new NeighbourCheck();
    }
	

	void Update ()
    {

    }


    public void CreateArea(string aT, List<Tile> tileList)
    {
        Area area = new Area(aT);
        AreaList.Add(area);

        switch (area.type)
        {
            case "Storage":
                StorageLsit.Add(area);
                break;

            case "Farmland":
                FarmlandLsit.Add(area);
                break;

            default:
                break;
        }

        for (int i = 0; i < tileList.Count; i++)
        {
            Tile t = tileList[i];

            if (t.arch ==null && t.area == null)
            {
                area.tileInArea.Add(t);
                t.area = area;

                GameObject t_obj = WorldController.Instance.tileGameObjectMap[t];

                switch (area.type)
                {
                    case "Storage":
                        t_obj.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 1f, 1f);
                        break;

                    case "Farmland":
                        t_obj.transform.GetComponent<SpriteRenderer>().color = new Color(0.9f, 1f, 0.9f, 1f);
                        break;

                    default:
                        break;
                }
            }

            neighbourCheck.AreaCombine(t);
        }
    }


    public void RemoveTileFromArea(Tile t)
    {
        if (t.area != null)
        {
            GameObject t_obj = WorldController.Instance.tileGameObjectMap[t];
            t_obj.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            t.area.tileInArea.Remove(t);
            t.area = null;
        }
    }


    public void ClaerInvalidArea()
    {
        for (int i = 0; i < AreaList.Count; i++)
        {
            Area a = AreaList[i];
            if (a.tileInArea.Count == 0)
            {
                AreaList.Remove(a);

                switch (a.type)
                {
                    case "Storage":
                        StorageLsit.Remove(a);
                        break;

                    case "Farmland":
                        FarmlandLsit.Remove(a);
                        break;

                    default:
                        break;
                }
            }
        }
    }


    public void ClearIsolatedTile()
    {
        for (int i = 0; i < AreaList.Count; i++)
        {
            Area a = AreaList[i];
            List<Tile> tempTileInArea = new List<Tile>();

            for (int j = 0; j < a.tileInArea.Count; j++)
            {
                Tile t = a.tileInArea[j];
                tempTileInArea.Add(t);
            }

            List<List<Tile>> subArea = new List<List<Tile>>();

            while (tempTileInArea.Count > 0)
            {
                Tile tile = tempTileInArea[0];

                List<Tile> tileInSubArea = new List<Tile>();
                List<Tile> checkList = new List<Tile>();

                tileInSubArea.Add(tile);
                checkList.Add(tile);
                tempTileInArea.Remove(tile);

                while (checkList.Count > 0)
                {
                    Tile t = checkList[0];

                    Tile nT = null;
                    int x = t.x;
                    int y = t.y;

                    nT = WorldController.Instance.GetTileAt(x, y + 1);
                    if (nT != null && nT.area != null && nT.area == t.area && tileInSubArea.Contains(nT) == false)
                    {
                        tileInSubArea.Add(nT);
                        checkList.Add(nT);
                        tempTileInArea.Remove(nT);
                    }

                    nT = WorldController.Instance.GetTileAt(x, y - 1);
                    if (nT != null && nT.area != null && nT.area == t.area && tileInSubArea.Contains(nT) == false)
                    {
                        tileInSubArea.Add(nT);
                        checkList.Add(nT);
                        tempTileInArea.Remove(nT);
                    }

                    nT = WorldController.Instance.GetTileAt(x + 1, y);
                    if (nT != null && nT.area != null && nT.area == t.area && tileInSubArea.Contains(nT) == false)
                    {
                        tileInSubArea.Add(nT);
                        checkList.Add(nT);
                        tempTileInArea.Remove(nT);
                    }

                    nT = WorldController.Instance.GetTileAt(x - 1, y);
                    if (nT != null && nT.area != null && nT.area == t.area && tileInSubArea.Contains(nT) == false)
                    {
                        tileInSubArea.Add(nT);
                        checkList.Add(nT);
                        tempTileInArea.Remove(nT);
                    }

                    checkList.Remove(t);
                }

                subArea.Add(tileInSubArea);
            }

            if (tempTileInArea.Count == 0)
            {
                int maxListIndex = 0;

                for (int j = 0; j < subArea.Count; j++)
                {
                    if (subArea[j].Count > subArea[maxListIndex].Count)
                    {
                        maxListIndex = j;
                    }
                }

                for (int j = a.tileInArea.Count; j > 0; j--)
                {
                    Tile t = a.tileInArea[j - 1];

                    if (subArea[maxListIndex].Contains(t) == false)
                    {
                        RemoveTileFromArea(t);
                    }
                }
            }
        }
    }
}
