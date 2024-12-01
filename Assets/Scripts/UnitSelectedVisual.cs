using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ÿ��Unit��ѡ����ʾ��
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
        //��֤�ʼ��ʱ��͵���һ��
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
