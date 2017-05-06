using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar {

    public List<Node> openList = new List<Node>();
    public List<Node> closeList = new List<Node>();
    public List<Node> path = new List<Node>();

    Grid gridMap;

    Node startNode;
    Node targetNode;

    public Astar(Node Start, Node End, Grid GridMap)
    {
        gridMap = GridMap;
        startNode = gridMap.nodes[Start.x, Start.y];
        targetNode = gridMap.nodes[End.x, End.y];
        
        openList.Clear();
        closeList.Clear();
        path.Clear();

        Node currentNode = null;
        openList.Add(startNode);

        while (openList.Count != 0 && closeList.Contains(targetNode) == false)
        {
            currentNode = openList[0]; //Debug.Log("openlist_" + openList.Count);

            openList.RemoveAt(0);
            closeList.Add(currentNode); //Debug.Log("closelist_" + closeList.Count);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (currentNode.x + i >= 0 && currentNode.x + i < gridMap.width && currentNode.y + j >= 0 && currentNode.y + j < gridMap.length)
                    {
                        Node node = gridMap.nodes[currentNode.x + i, currentNode.y + j];

                        if (closeList.Contains(node) == false && node.isWalkable == true)
                        {
                            if (openList.Contains(node) == false)
                            {
                                node.h = Manhattan(node, targetNode);

                                int temp_G = GetG(i, j, currentNode);
                                node.g = temp_G;

                                node.f = node.g + node.h;

                                node.fatherNode = currentNode;
                                openList.Add(node);
                            }

                            if (openList.Contains(node) == true)
                            {
                                int temp_G = GetG(i, j, currentNode);

                                if(temp_G < node.g)
                                {
                                    node.g = temp_G;
                                    node.fatherNode = currentNode;
                                }

                                node.f = node.g + node.h;
                            }
                        }
                    }
                }
            }
        }


        if (targetNode.fatherNode != null)
        {
            Node waypoint = targetNode;

            while (waypoint != startNode)
            {
                path.Add(waypoint);
                waypoint = waypoint.fatherNode;
            }

            if(path == null)
            {
                Debug.Log("can't find the path");
            }
        }
    }


    int GetG(int i, int j, Node currentNode)
    {
        int temp_g = 0;

        if (i * j == 0)
        {
            temp_g = currentNode.g + 10;
        }
        else
        {
            temp_g = currentNode.g + 15;
        }

        return temp_g;
    }

 
    int Manhattan(Node node, Node targetNode)
    {
        int h = (Mathf.Abs(node.x - targetNode.x) + Mathf.Abs(node.y - targetNode.y)) * 5;
        return h;
    }


    Node FindBestNode(List<Node> list)
    {
        foreach (Node node in list)
        {
            if (node.f < list[0].f)
            {

            } 
        }

        return list[0];
    }
}
