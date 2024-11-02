using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected bool isActive = false;
    protected Unit unit;
    protected Action onActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition,Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> vailidGridPositionList = GetValidActionGridPosition();
        return vailidGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPosition();
}
