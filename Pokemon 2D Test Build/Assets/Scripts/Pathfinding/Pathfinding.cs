using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    List<PathNode> availablePathNodesInsideGrid;

    List<PathNode> openList;
    List<PathNode> closedList;

    public List<Vector2> FindPath(Vector2 start,Vector2 end)
    {
        if(start == end)
        {
            return null;
        }    

        Vector2 worldPosOffset = ArtificialGrid.GetWorldPositionOffset();
        PathNode startNode = new PathNode(Mathf.FloorToInt(start.x - worldPosOffset.x), Mathf.FloorToInt(start.y - worldPosOffset.y));
        PathNode endNode = new PathNode(Mathf.FloorToInt(end.x - worldPosOffset.x), Mathf.FloorToInt(end.y - worldPosOffset.y));

        availablePathNodesInsideGrid = new List<PathNode>();
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        bool[,] grid = ArtificialGrid.GetGrid;

        if(grid[startNode.x,startNode.y] == false || grid[endNode.x, endNode.y] == false)
        {
            Debug.Log("end or start position is Unwalkable");
            return null;
        }

        List<Entity> allActiveEntities = SceneSystem.currentLevelManager.GetAllEntities();
        allActiveEntities.Add(GameManager.instance.GetPlayerController);

        for (int i = 0; i < allActiveEntities.Count; i++)
        {
            Vector2Int position = new Vector2Int(Mathf.FloorToInt(allActiveEntities[i].transform.position.x - worldPosOffset.x), Mathf.FloorToInt(allActiveEntities[i].transform.position.y - worldPosOffset.y));
            if(position.x == startNode.x && position.y == startNode.y)
            {
                continue;
            }
            grid[position.x, position.y] = false;
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x,y] == false)
                {
                    continue;
                }

                if(x == endNode.x && y == endNode.y)
                {
                    endNode.gCost = int.MaxValue;
                    endNode.CalculateFCost();
                    availablePathNodesInsideGrid.Add(endNode);
                    continue;
                }

                PathNode pathNode = new PathNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                availablePathNodesInsideGrid.Add(pathNode);
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count >0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //reached final node
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeightbourNodes(currentNode))
            {
                if(closedList.Contains(neighbourNode) || openList.Contains(neighbourNode))
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                }

                openList.Add(neighbourNode);
            }
        }
        return null;
    }

    int CalculateDistanceCost(PathNode a,PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return xDistance + yDistance;
    }

    PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCost = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFCost.fCost)
            {
                lowestFCost = pathNodeList[i];
            }
        }
        return lowestFCost;
    }

    public List<Vector2> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();

        List<Vector2> pathMovement = new List<Vector2>();
        for (int i = 0; i < path.Count; i++)
        {
            pathMovement.Add(path[i].ReturnPathFromCurrentNode());
        }
        return pathMovement;
    }

    public PathNode GetNode(int x,int y)
    {
        return availablePathNodesInsideGrid.FirstOrDefault(node => node.x == x && node.y == y);
    }

    public List<PathNode> GetNeightbourNodes(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        //left
        PathNode tempNode = GetNode(currentNode.x - 1, currentNode.y);
        if (tempNode != null)
        {
            neighbours.Add(tempNode);
        }

        //Right
        tempNode = GetNode(currentNode.x + 1, currentNode.y);
        if (tempNode != null)
        {
            neighbours.Add(tempNode);
        }

        //Up
        tempNode = GetNode(currentNode.x, currentNode.y + 1);
        if (tempNode != null)
        {
            neighbours.Add(tempNode);
        }

        //Down
        tempNode = GetNode(currentNode.x, currentNode.y - 1);
        if (tempNode != null)
        {
            neighbours.Add(tempNode);
        }

        return neighbours;
    }
}
