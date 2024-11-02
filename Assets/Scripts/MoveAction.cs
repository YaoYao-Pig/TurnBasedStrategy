using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPosition;
    private float stoppingDistance = .1f;
    private float unitRotateSpeed = 6.0f;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private int maxMoveDistance=4;
    private Unit unit;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(targetPosition, transform.position) >= stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveSpeed * moveDirection * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, unitRotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> vailidGridPositionList= GetValidActionGridPosition();
        return vailidGridPositionList.Contains(gridPosition);
    }
    public List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x < maxMoveDistance; ++x)
        {
            for(int z = -maxMoveDistance; z < maxMoveDistance; ++z)
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
                Debug.Log(testGridPosition);
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
