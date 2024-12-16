using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class GridSystemHex<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;
    private const float HEX_VERTICAL_OFFSET_MULITIPLIER = 0.75f;
    public GridSystemHex(int _width,int _height,float _cellSize,Func<GridSystemHex<TGridObject>,GridPosition,TGridObject> createGridObject)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;
        gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; ++x)
        {
            for(int z = 0; z < height; ++z)
            {
                gridObjectArray[x,z]= createGridObject(this, new GridPosition(x, z));
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, 0) * cellSize +
            new Vector3(0,0,gridPosition.z) * cellSize* HEX_VERTICAL_OFFSET_MULITIPLIER +
             (((gridPosition.z % 2)==1)?new Vector3(1,0,0)*(cellSize/2):Vector3.zero);
    }
    public GridPosition GetGridPosition(Vector3 _worldPosition)
    {
        GridPosition roughGridPosition = new GridPosition(
                Mathf.RoundToInt(_worldPosition.x/cellSize),
                Mathf.RoundToInt(_worldPosition.z/cellSize/ HEX_VERTICAL_OFFSET_MULITIPLIER)
            );
        bool oddRow = roughGridPosition.z % 2 == 1;
        List<GridPosition> neighbourGridPositionList = new List<GridPosition>
        {
            roughGridPosition+new GridPosition(-1,0),
            roughGridPosition+new GridPosition(1,0),
            roughGridPosition+new GridPosition(0,1),
            roughGridPosition+new GridPosition(0,-1),
            roughGridPosition+new GridPosition(oddRow?+1:-1,1),
            roughGridPosition+new GridPosition(oddRow?+1:-1,-1),
        };
        float closetDistance = Vector3.Distance(LevelGrid.Instance.GetWorldPosition(roughGridPosition), _worldPosition);
        GridPosition closetGridPosition=roughGridPosition;
        foreach(GridPosition neighbourGridPosition in neighbourGridPositionList)
        {
            if (!LevelGrid.Instance.IsValidGridPosition(neighbourGridPosition)) continue;
            float testDistance = Vector3.Distance(LevelGrid.Instance.GetWorldPosition(neighbourGridPosition), _worldPosition);
            if (closetDistance > testDistance)
            {
                closetDistance = testDistance;
                closetGridPosition = neighbourGridPosition;
            }

        }
        return closetGridPosition;
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < height; ++z)
            {
                GridPosition pos = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(pos), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(pos));
               

            }
        }
    }


    public TGridObject GetGridObject(GridPosition _gridPosition)
    {
        return gridObjectArray[_gridPosition.x, _gridPosition.z];
    }


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0 &&
            gridPosition.x < width && gridPosition.z < height;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}

