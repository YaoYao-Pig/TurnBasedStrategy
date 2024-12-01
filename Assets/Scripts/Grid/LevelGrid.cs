using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private GridSystem gridSystem;
    [SerializeField] private GameObject gridDebugPrefab;

    private static LevelGrid _instance;
    public static LevelGrid Instance
    {
        get { return _instance; }
        private set { }
    }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else
        {
            Destroy(gameObject);
        }
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugPrefab);
    }

    //设置某个Unit归属于某个Grid
    public void AddUnitAtGridPosition(GridPosition _gridPosition,Unit _unit)
    {
        GridObject gridObject= gridSystem.GetGridObject(_gridPosition);
        gridObject.AddUnit(_unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition _gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(_gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition _gridPosition,Unit unit)
    {

        GridObject gridObject = gridSystem.GetGridObject(_gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMoveGridPosition(Unit unit,GridPosition fromGridPosition,GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition,unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }
    //暴露gridSystem的接口
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
