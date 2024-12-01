using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在每个Unit的选中提示上
/// </summary>
public class UnitSelectedVisual : MonoBehaviour
{
    private Unit unit;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        //保证最开始的时候就调用一次
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender,EventArgs args)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
    public void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}
