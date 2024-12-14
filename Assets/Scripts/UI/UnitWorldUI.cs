using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateHeathBar();
        UpdateActionPointsText();
    }
    private void Unit_OnAnyActionPointsChanged(object sender,EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetAciontPoints().ToString();
    }
    private void HealthSystem_OnDamaged(object sender,EventArgs e)
    {
        UpdateHeathBar();
    }
    private void UpdateHeathBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
