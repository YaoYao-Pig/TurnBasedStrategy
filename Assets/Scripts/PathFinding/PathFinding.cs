using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    private int width;
    private int height;
    private float cellsize;
    private GridSystem<PathNode> gridSystem;
    [SerializeField] private Transform gridDebugPrefab;
    [SerializeField] private LayerMask obstacleLayerMask;
    public static PathFinding Instance;

    private readonly int MOVE_DIAGONAL_COST=14;
    private readonly int MOVE_STRAIGHT_COST = 10;
    public void Awake()
    {
        Instance = this;
/*        this.gridSystem = new GridSystem<PathNode>(10, 10, 2f,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugPrefab);*/
    }



    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition,out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>(); 

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for(int x = 0; x < gridSystem.GetWidth(); ++x)
        {
            for(int z = 0; z < gridSystem.GetWidth(); ++z)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CaculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CaculateDistance(startGridPosition,endGridPosition));
        startNode.ResetCameFromPathNode();

        while (openList.Count > 0)
        {
            PathNode currentPathNode = GetLowestFCostPathNode(openList);

            if(currentPathNode== endNode)
            {
                pathLength = endNode.GetfCost();
                return CaculatePath(endNode);
            }

            openList.Remove(currentPathNode);
            closeList.Add(currentPathNode);
            foreach(PathNode neigbourPathNode in GetNeigbourList(currentPathNode))
            {
                if (closeList.Contains(neigbourPathNode))
                {
                    continue;
                }

                if (!neigbourPathNode.IsWalkable())
                {
                    closeList.Add(neigbourPathNode);
                    continue;
                }
                int tentativeGCost = currentPathNode.GetGCost() + CaculateDistance(currentPathNode.GetGridPosition(), neigbourPathNode.GetGridPosition());


                //经由当前点到邻居节点，距离start的距离比邻居节点直接到start的距离更近
                //注意，这里都是引用，更新会影响到openList里面
                if (tentativeGCost < neigbourPathNode.GetGCost())
                {
                    neigbourPathNode.SetCameFromPathNode(currentPathNode);
                    neigbourPathNode.SetGCost(tentativeGCost);
                    neigbourPathNode.SetHCost(CaculateDistance(neigbourPathNode.GetGridPosition(), endGridPosition));
                    neigbourPathNode.CaculateFCost();
                    if (!openList.Contains(neigbourPathNode))
                    {
                        openList.Add(neigbourPathNode);
                    }
                }
            }
        }

        //No Path Find
        pathLength = 0;
        return null;

    }
    public int CaculateDistance(GridPosition gridPositionA,GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int totalDistance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(zDistance, xDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    public PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        foreach(PathNode pathNode in pathNodeList)
        {
            if (pathNode.GetfCost() < lowestFCostPathNode.GetfCost())
            {
                lowestFCostPathNode = pathNode;
            }
        }
        return lowestFCostPathNode;
    }


    private int[] neigbourXOffset = new int[8] { 1, -1, 1, -1 , 0, 0, 1, -1};
    private int[] neigbourZOffset = new int[8] { -1, 1, 1, -1 , 1, -1, 0, 0};

    public List<PathNode> GetNeigbourList(PathNode pathNode)
    {
        List<PathNode> neigbourList = new List<PathNode>();
        
        for(int offset = 0; offset < 8; ++offset)
        {
            int xOffset = neigbourXOffset[offset];
            int zOffset = neigbourZOffset[offset];
            GridPosition neigbourGridPosition = pathNode.GetGridPosition() + new GridPosition(xOffset, zOffset);
            if (CheckPositionValid(neigbourGridPosition))
            {
                neigbourList.Add(gridSystem.GetGridObject(neigbourGridPosition));
            }
        }
        return neigbourList;
    }

    private bool CheckPositionValid(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }

    private List<GridPosition> CaculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        List<GridPosition> gridPositionList = new List<GridPosition>();    
        PathNode currentNode = endNode;
        while (currentNode != null)
        {
            pathNodeList.Add(currentNode);
            gridPositionList.Add(currentNode.GetGridPosition());
            currentNode = currentNode.GetCameFromPathNode();
        }
        //pathNodeList.Reverse();
        gridPositionList.Reverse();
        return gridPositionList;
    
    }

    public void Setup(int width,int height,float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellSize;
        gridSystem = new GridSystem<PathNode>(width, height, cellsize,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugPrefab);
        
        for(int x = 0; x < width; ++x)
        {
            for(int z = 0; z < height; ++z)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                float raycastOffsetDistance = 5f;
                if(Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, 
                    Vector3.up, 
                    raycastOffsetDistance * 2,
                    obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    } 

    public PathNode GetNode(int x,int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {

        if (FindPath(startGridPosition, endGridPosition,out int pathLength) == null)
        {
            return false;
        }
        return true;
    }

    public int GetPathLenght(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);

        return pathLength;
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition,bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

}
