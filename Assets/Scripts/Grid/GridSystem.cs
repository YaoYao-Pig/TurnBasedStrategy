using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;

    public GridSystem(int _width,int _height,float _cellSize)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;
        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; ++x)
        {
            for(int z = 0; z < height; ++z)
            {
                gridObjectArray[x,z]=new GridObject(this, new GridPosition(x, z));
            }
        }
    }


    public Vector3 GetWorldPosition(GridPosition _gridPosition)
    {
        return new Vector3(_gridPosition.x, 0, _gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 _worldPosition)
    {
        return new GridPosition(
                Mathf.RoundToInt(_worldPosition.x/cellSize),
                Mathf.RoundToInt(_worldPosition.z/cellSize)
            );
    }

    public void CreateDebugObjects(GameObject debugPrefab)
    {
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < height; ++z)
            {
                GridPosition pos = new GridPosition(x, z);
                GameObject gameObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(pos), Quaternion.identity);
                GridDebugObject debug = gameObject.GetComponent<GridDebugObject>();
                debug.SetGridObject(GetGridObject(pos));
               

            }
        }
    }


    public GridObject GetGridObject(GridPosition _gridPosition)
    {
        return gridObjectArray[_gridPosition.x, _gridPosition.z];
    }


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0 &&
            gridPosition.x < width && gridPosition.z < height;
    }
}

