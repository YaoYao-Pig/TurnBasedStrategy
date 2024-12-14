using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private List<Vector3> positionList;
    private float stoppingDistance = .1f;
    private float unitRotateSpeed = 6.0f;

    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private int maxMoveDistance=4;
    
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    private int currentPositionIndex;


    private void Update()
    {
        if (!isActive) return;
        MoveTowardsTargetPerUpdate();
    }

    private void MoveTowardsTargetPerUpdate()
    {
        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, unitRotateSpeed * Time.deltaTime);


        if (Vector3.Distance(targetPosition, transform.position) >= stoppingDistance)
        {
            transform.position += moveSpeed * moveDirection * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {

                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();

            }
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        List<GridPosition> pathGridPositionList=PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; ++x)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                //越界检测
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                //相同位置检测
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                //占用检测
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                //pathFinding可达性检验
                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }
                int pathFindingMultiplier = 10;

                if(PathFinding.Instance.GetPathLenght(unitGridPosition, testGridPosition)>maxMoveDistance* pathFindingMultiplier)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition= unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        //计算每个位置可以射击到的玩家数量
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = targetCountAtGridPosition *10};
    }


}
