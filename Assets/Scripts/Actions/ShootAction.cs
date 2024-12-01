using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    [SerializeField] private int maxShootDistance= 4;
    [SerializeField] private int gunDamage = 40;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }


    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private void Update()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection= (targetUnit.GetWorldPosition()-unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime* rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if (stateTimer <= 0.0f)
        {
            NextState();
        }

    }
    private void NextState()
    {

        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:

                ActionComplete();
                break;
        }
    }


    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootDistance; x <= maxShootDistance; ++x)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                //越界检测
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }


                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                //半圆形
                if (testDistance > maxShootDistance) continue;



                //相同位置检测
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                //如果格子里没有Unit，就不会有敌人Unit
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                //如果当前ShootAction和目标位置上的Unit类型一致，就跳过
                if (targetUnit.GetIsEnemy()==unit.GetIsEnemy())
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {


        ActionStart(onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        //stateMachine initialize
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        state = State.Aiming;
        canShootBullet = true;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        }); ;
        targetUnit.Damage(gunDamage);
    }
}
