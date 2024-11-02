using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    private static UnitActionSystem _instance;
    public static UnitActionSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UnitActionSystem>();
                if (_instance == null)
                {
                    var obj = new GameObject("UnitActionSystem");
                    _instance=obj.AddComponent<UnitActionSystem>();
                }
            }
            return _instance;
        }
        private set { }
        
    }
    public event EventHandler OnSelectedUnitChanged;
    
    
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void Awake()
    {
        if (_instance == null){
            _instance = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            selectedUnit.Move(MouseWorld.Instance.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        if(Physics.Raycast(ray, out result, float.MaxValue, unitLayerMask))
        {
            if(result.transform.TryGetComponent<Unit>(out Unit unit)){
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }


    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }


}
