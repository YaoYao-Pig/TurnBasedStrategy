using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Unit> unitList;
    private List<Unit> friendlyuUnitList;
    private List<Unit> enemyUnitList;
    public static UnitManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        unitList = new List<Unit>();
        friendlyuUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }
    private void Unit_OnAnyUnitSpawned(object sender,EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        if (unit.GetIsEnemy())
        {
            Debug.Log("Add Enemy");
            enemyUnitList.Add(unit);
        }
        else
        {
            Debug.Log("Add Friend");
            friendlyuUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender,EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if (unit.GetIsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyuUnitList.Remove(unit);

        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetFriendlyuUnitList()
    {
        return friendlyuUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
