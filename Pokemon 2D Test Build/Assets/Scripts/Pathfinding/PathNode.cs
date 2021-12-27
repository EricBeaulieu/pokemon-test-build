using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x { get; private set; }
    public int y { get; private set; }

    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get; private set; }

    public PathNode cameFromNode = null;


    public PathNode(int xPos,int yPos)
    {
        x = xPos;
        y = yPos;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector2 ReturnPathFromCurrentNode()
    {
        if(cameFromNode == null)
        {
            return Vector2.zero;
        }
        return new Vector2(x - cameFromNode.x, y - cameFromNode.y);
    }
}
