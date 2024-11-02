using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged+= UnityActionSystem_OnSelectedUnitActionButtons;
        CreateUnitActionButtons();
    }


    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (var baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform= Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            actionButtonTransform.GetComponent<ActionButtonUI>().SetBaseAction(baseAction);
        }
    }
    
    private void UnityActionSystem_OnSelectedUnitActionButtons(object sender,EventArgs args)
    {
        CreateUnitActionButtons();
    }
}
