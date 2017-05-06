using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourCheck {


    public void StructureSpriteUpdate(Tile tile)
    {
        Tile nT;
        int x = tile.x;
        int y = tile.y;

        nT = WorldController.Instance.GetTileAt(x, y + 1);
        if (nT != null && nT.arch != null && nT.arch.category == "Structure")
        {
            GameObject arch = ArchitectController.Instance.archGameObjectMap[nT.arch];
            arch.GetComponent<SpriteRenderer>().sprite = WorldController.Instance.spriteloader.archSprite["Structure" + StructureSpriteSet(nT.arch)];
        }

        nT = WorldController.Instance.GetTileAt(x, y - 1);
        if (nT != null && nT.arch != null && nT.arch.category == "Structure")
        {
            GameObject arch = ArchitectController.Instance.archGameObjectMap[nT.arch];
            arch.GetComponent<SpriteRenderer>().sprite = WorldController.Instance.spriteloader.archSprite["Structure" + StructureSpriteSet(nT.arch)];
        }

        nT = WorldController.Instance.GetTileAt(x + 1, y);
        if (nT != null && nT.arch != null && nT.arch.category == "Structure")
        {
            GameObject arch = ArchitectController.Instance.archGameObjectMap[nT.arch];
            arch.GetComponent<SpriteRenderer>().sprite = WorldController.Instance.spriteloader.archSprite["Structure" + StructureSpriteSet(nT.arch)];
        }

        nT = WorldController.Instance.GetTileAt(x - 1, y);
        if (nT != null && nT.arch != null && nT.arch.category == "Structure")
        {
            GameObject arch = ArchitectController.Instance.archGameObjectMap[nT.arch];
            arch.GetComponent<SpriteRenderer>().sprite = WorldController.Instance.spriteloader.archSprite["Structure" + StructureSpriteSet(nT.arch)];
        }
    }


    public string StructureSpriteSet(Architecture arch_data)
    {
        Tile nT;

        string spriteName = arch_data.name + arch_data.type;
        int x = arch_data.tile.x;
        int y = arch_data.tile.y;

        nT = WorldController.Instance.GetTileAt(x, y + 1);
        if(nT != null && nT.arch != null && nT.arch.name == arch_data.name)
        {
            spriteName += "N";
        }

        nT = WorldController.Instance.GetTileAt(x, y - 1);
        if (nT != null && nT.arch != null && nT.arch.name == arch_data.name)
        {
            spriteName += "S";
        }

        nT = WorldController.Instance.GetTileAt(x + 1, y);
        if (nT != null && nT.arch != null && nT.arch.name == arch_data.name)
        {
            spriteName += "E";
        }

        nT = WorldController.Instance.GetTileAt(x - 1, y);
        if (nT != null && nT.arch != null && nT.arch.name == arch_data.name)
        {
            spriteName += "W";
        }

        return spriteName;
    }


    public void AreaCombine(Tile tile)
    {
        Tile nT = null;
        int x = tile.x;
        int y = tile.y;

        nT = WorldController.Instance.GetTileAt(x, y + 1);
        if (nT != null && nT.area != null && nT.area.type == tile.area.type)
        {
            List<Tile> tempList = new List<Tile>();

            for (int i = 0; i < nT.area.tileInArea.Count; i++)
            {
                tempList.Add(nT.area.tileInArea[i]);
            }

            nT.area.tileInArea.Clear();

            for (int i = 0; i < tempList.Count; i++)
            {
                Tile t = tempList[i];
                tile.area.tileInArea.Add(t);
                t.area = tile.area;
            }
        }

        nT = WorldController.Instance.GetTileAt(x, y - 1);
        if (nT != null && nT.area != null && nT.area.type == tile.area.type)
        {
            List<Tile> tempList = new List<Tile>();

            for (int i = 0; i < nT.area.tileInArea.Count; i++)
            {
                tempList.Add(nT.area.tileInArea[i]);
            }

            nT.area.tileInArea.Clear();

            for (int i = 0; i < tempList.Count; i++)
            {
                Tile t = tempList[i];
                tile.area.tileInArea.Add(t);
                t.area = tile.area;
            }
        }

        nT = WorldController.Instance.GetTileAt(x + 1, y);
        if (nT != null && nT.area != null && nT.area.type == tile.area.type)
        {
            List<Tile> tempList = new List<Tile>();

            for (int i = 0; i < nT.area.tileInArea.Count; i++)
            {
                tempList.Add(nT.area.tileInArea[i]);
            }

            nT.area.tileInArea.Clear();

            for (int i = 0; i < tempList.Count; i++)
            {
                Tile t = tempList[i];
                tile.area.tileInArea.Add(t);
                t.area = tile.area;
            }
        }

        nT = WorldController.Instance.GetTileAt(x - 1, y);
        if (nT != null && nT.area != null && nT.area.type == tile.area.type)
        {
            List<Tile> tempList = new List<Tile>();

            for (int i = 0; i < nT.area.tileInArea.Count; i++)
            {
                tempList.Add(nT.area.tileInArea[i]);
            }

            nT.area.tileInArea.Clear();

            for (int i = 0; i < tempList.Count; i++)
            {
                Tile t = tempList[i];
                tile.area.tileInArea.Add(t);
                t.area = tile.area;
            }
        }
    }
}
