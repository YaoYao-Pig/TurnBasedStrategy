using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    private float stoppingDistance = .1f;
    private float unitRotateSpeed = 6.0f;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private int maxMoveDistance=4;


    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(targetPosition, transform.position) >= stoppingDistance)
        {
            transform.position += moveSpeed * moveDirection * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            isActive = false;
            unitAnimator.SetBool("IsWalking", false);
            onActionComplete.Invoke();
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, unitRotateSpeed * Time.deltaTime);
        
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
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
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
