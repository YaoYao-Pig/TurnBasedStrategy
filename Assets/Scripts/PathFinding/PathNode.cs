using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable=true;
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }


    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetfCost()
    {
        return fCost;
    }
    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    public void SetGCost(int g)
    {
        gCost = g;
    }
    public void SetHCost(int h)
    {
        hCost = h;
    }
    public void CaculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void SetCameFromPathNode(PathNode cameFromPathNode)
    {
        this.cameFromPathNode = cameFromPathNode;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
