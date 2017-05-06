using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDetect {

    public void FindStartTile(Tile tile, List<Tile> startTileList)
    {
        List<Vector2> neighbourWallVector = new List<Vector2>();

        if (tileHasBlock(WorldController.Instance.GetNeighbourN(tile)) == true)
        {
            neighbourWallVector.Add(new Vector2(0, 1));
        }

        if (tileHasBlock(WorldController.Instance.GetNeighbourE(tile)) == true)
        {
            neighbourWallVector.Add(new Vector2(1, 0));
        }

        if (tileHasBlock(WorldController.Instance.GetNeighbourS(tile)) == true)
        {
            neighbourWallVector.Add(new Vector2(0, -1));
        }

        if (tileHasBlock(WorldController.Instance.GetNeighbourW(tile)) == true)
        {
            neighbourWallVector.Add(new Vector2(-1, 0));
        }

        if (neighbourWallVector.Count == 4)
        {
            List<Tile> cornorTileList = new List<Tile>();

            cornorTileList.Add(WorldController.Instance.GetNeighbourNE(tile));
            cornorTileList.Add(WorldController.Instance.GetNeighbourSE(tile));
            cornorTileList.Add(WorldController.Instance.GetNeighbourSW(tile));
            cornorTileList.Add(WorldController.Instance.GetNeighbourNW(tile));

            for (int i = 0; i < cornorTileList.Count; i++)
            {
                Tile tempTile = cornorTileList[i];

                if (tileHasBlock(tempTile) == false)
                {
                    startTileList.Add(tempTile);
                }
            }
        }
        else if (neighbourWallVector.Count == 3)
        {
            int diffX = Mathf.FloorToInt(neighbourWallVector[0].x + neighbourWallVector[1].x + neighbourWallVector[2].x);
            int diffY = Mathf.FloorToInt(neighbourWallVector[0].y + neighbourWallVector[1].y + neighbourWallVector[2].y);

            startTileList.Add(WorldController.Instance.GetTileAt(tile.x - diffX, tile.y - diffY));

            if (diffX == 0)
            {
                if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x - 1, tile.y + diffY)) == false)
                {
                    startTileList.Add(WorldController.Instance.GetTileAt(tile.x - 1, tile.y + diffY));
                }

                if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x + 1, tile.y + diffY)) == false)
                {
                    startTileList.Add(WorldController.Instance.GetTileAt(tile.x + 1, tile.y + diffY));
                }
            }

            if (diffY == 0)
            {
                if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y - 1)) == false)
                {
                    startTileList.Add(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y - 1));
                }

                if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y + 1)) == false)
                {
                    startTileList.Add(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y + 1));
                }
            }            
        }
        else if (neighbourWallVector.Count == 2)
        {
            int diffX = Mathf.FloorToInt(neighbourWallVector[0].x + neighbourWallVector[1].x);
            int diffY = Mathf.FloorToInt(neighbourWallVector[0].y + neighbourWallVector[1].y);

            if (diffX == 0 && diffY == 0) // straight
            {
                if (neighbourWallVector[0].x == 0 && neighbourWallVector[1].x == 0) // NS
                {
                    if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x - 1, tile.y)) == false)
                    {
                        startTileList.Add(WorldController.Instance.GetTileAt(tile.x - 1, tile.y));
                    }

                    if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x + 1, tile.y)) == false)
                    {
                        startTileList.Add(WorldController.Instance.GetTileAt(tile.x + 1, tile.y));
                    }
                }

                if (neighbourWallVector[0].y == 0 && neighbourWallVector[1].y == 0) // EW
                {
                    if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x, tile.y - 1)) == false)
                    {
                        startTileList.Add(WorldController.Instance.GetTileAt(tile.x, tile.y - 1));
                    }

                    if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x, tile.y + 1)) == false)
                    {
                        startTileList.Add(WorldController.Instance.GetTileAt(tile.x, tile.y + 1));
                    }
                }
            }

            if (diffX != 0 && diffY != 0) // cornor
            {
                startTileList.Add(WorldController.Instance.GetTileAt(tile.x, tile.y - diffY));

                if (tileHasBlock(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y + diffY)) == false)
                {
                    startTileList.Add(WorldController.Instance.GetTileAt(tile.x + diffX, tile.y + diffY));
                }
            }
        }
        else
        {
            Debug.Log("no room was changed");
        }
    }


    public void DetectClosedArea(Tile startTile, List<Tile> tileWithinList)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();
        List<Tile> cornorList = new List<Tile>();

        string direction = "N";
        bool isOutdoor = false;

        openList.Add(startTile);

        while(openList.Count > 0 && isOutdoor == false)
        {
            Tile t = openList[openList.Count - 1];

            Tile n = WorldController.Instance.GetNeighbourN(t);
            Tile e = WorldController.Instance.GetNeighbourE(t);
            Tile s = WorldController.Instance.GetNeighbourS(t);
            Tile w = WorldController.Instance.GetNeighbourW(t);

            if (n == null || e == null || s == null || w == null)
            {
                isOutdoor = true;
                continue;
            }

            Dictionary<Tile, string> tileDirection = new Dictionary<Tile, string>();
            tileDirection.Add(n, "N");
            tileDirection.Add(e, "E");
            tileDirection.Add(s, "S");
            tileDirection.Add(w, "W");

            List<Tile> unCheckedList = new List<Tile>();

            switch (direction)
            {
                case "N":
                    unCheckedList.Add(n);
                    unCheckedList.Add(e);
                    unCheckedList.Add(s);
                    unCheckedList.Add(w);
                    break;

                case "E":
                    unCheckedList.Add(e);
                    unCheckedList.Add(s);
                    unCheckedList.Add(w);
                    unCheckedList.Add(n);
                    break;

                case "S":
                    unCheckedList.Add(s);
                    unCheckedList.Add(w);
                    unCheckedList.Add(n);
                    unCheckedList.Add(e);
                    break;

                case "W":
                    unCheckedList.Add(w);
                    unCheckedList.Add(n);
                    unCheckedList.Add(e);
                    unCheckedList.Add(s);
                    break;

                default:
                    break;
            }

            if (unCheckedList.Count == 4)
            {
                bool isDeadEnd = true;

                for (int i = 0; i < 4; i++)
                {
                    if (tileHasBlock(unCheckedList[i]) == false && openList.Contains(unCheckedList[i]) == false && closeList.Contains(unCheckedList[i]) == false)
                    {
                        openList.Add(unCheckedList[i]);
                        direction = tileDirection[unCheckedList[i]];
                        isDeadEnd = false;
                        break;
                    }
                    else 
                    {
                        if (tileHasBlock(unCheckedList[i]) == true)
                        {
                            Tile tempT0 = GetNeighbourInList(i, unCheckedList)[0];
                            Tile tempT1 = GetNeighbourInList(i, unCheckedList)[1];

                            if (tileHasBlock(tempT0) == true)
                            {
                                int diffX = tempT0.x + unCheckedList[i].x - t.x - t.x;
                                int diffY = tempT0.y + unCheckedList[i].y - t.y - t.y;

                                Tile cornorTile = WorldController.Instance.GetTileAt(t.x + diffX, t.y + diffY);

                                if (tileHasBlock(cornorTile) == false && cornorList.Contains(cornorTile) == false)
                                {
                                    cornorList.Add(cornorTile);
                                }
                            }

                            if (tileHasBlock(tempT1) == true)
                            {
                                int diffX = tempT1.x + unCheckedList[i].x - t.x - t.x;
                                int diffY = tempT1.y + unCheckedList[i].y - t.y - t.y;

                                Tile cornorTile = WorldController.Instance.GetTileAt(t.x + diffX, t.y + diffY);

                                if (tileHasBlock(cornorTile) == false && cornorList.Contains(cornorTile) == false)
                                {
                                    cornorList.Add(cornorTile);
                                }
                            }
                        }
                    }
                }

                if (isDeadEnd == true)
                {
                    openList.Remove(t);
                    closeList.Add(t);
                }
            }

            if (openList.Count == 0)
            {
                for (int i = 0; i < cornorList.Count; i++)
                {
                    Tile cornorTile = cornorList[i];

                    if (openList.Contains(cornorTile) == false && closeList.Contains(cornorTile) == false)
                    {
                        openList.Add(cornorTile);
                    }
                }
            }
        }

        if (isOutdoor != true)
        {
            for (int i = 0; i < closeList.Count; i++)
            {
                tileWithinList.Add(closeList[i]);
            }
        }
    }


    bool tileHasBlock(Tile t)
    {
        if (t != null && t.arch != null && (t.arch.name == "Wall" || t.arch.name == "Door") && t.arch.stats != ArchStats.building)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    List<Tile> GetNeighbourInList(int index, List<Tile> targetList)
    {
        List<Tile> neighbourList = new List<Tile>();

        if (targetList.Count < 3)
        {
            return null;
        }
        else
        {
            if (index == 0)
            {
                neighbourList.Add(targetList[1]);
                neighbourList.Add(targetList[targetList.Count - 1]);
            }
            else if (index == targetList.Count - 1)
            {
                neighbourList.Add(targetList[targetList.Count - 2]);
                neighbourList.Add(targetList[0]);
            }
            else
            {
                neighbourList.Add(targetList[index - 1]);
                neighbourList.Add(targetList[index + 1]);
            }

            return neighbourList;
        }
    }
}
