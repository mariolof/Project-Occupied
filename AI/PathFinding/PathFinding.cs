using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding {

    public List<Node> currentPath = new List<Node>();

    Astar Astar;
    Grid gridMap;
    Node start;
    Node end;


    public PathFinding(Tile targetTile, GameObject obj, World world)
    {
        if(targetTile.isWalkable == false)
        {
            return;
        }

        gridMap = new Grid(world);

        Tile tileUnderObj = WorldController.Instance.GetTileAtPos(obj.transform.position);
        start = gridMap.nodes[tileUnderObj.x, tileUnderObj.y];

        end = gridMap.nodes[targetTile.x, targetTile.y];

        Astar = new Astar(start, end, gridMap);
        currentPath = Astar.path;
    }
}
