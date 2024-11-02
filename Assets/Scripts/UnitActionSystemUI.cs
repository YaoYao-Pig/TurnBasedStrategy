using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged+= UnityActionSystem_OnSelectedUnitActionButtons;
        UnitActionSystem.Instance.OnActionChanged += UnityActionSystem_OnSelectedActionChanged;
        
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }


    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (var baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform= Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            var actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUIList.Add(actionButtonUI);
            actionButtonUI.SetBaseAction(baseAction);
        }
    }
    
    private void UnityActionSystem_OnSelectedUnitActionButtons(object sender,EventArgs args)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();

    }
    private void UnityActionSystem_OnSelectedActionChanged(object sender, EventArgs args)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach(var actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

}
