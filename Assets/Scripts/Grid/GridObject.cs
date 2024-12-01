using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    public GridObject(GridSystem _gridSystem,GridPosition _gridPosition)
    {
        this.gridSystem = _gridSystem;
        this.gridPosition = _gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach(var unit in unitList)
        {
            unitString += unit.ToString();
        }
        return gridPosition.ToString()+"\n"+ unitString;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }
}
