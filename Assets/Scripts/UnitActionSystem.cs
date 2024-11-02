using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

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
    public event EventHandler OnActionChanged;
    public event EventHandler<bool> OnBusyUIChanged;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;

    private bool isBusy = false;

    private void Awake()
    {
        if (_instance == null){
            _instance = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //�����ѡ��Unit��������ô���Update�ͷ���
        if (TryHandleUnitSelection()){
            return;
        }
        //����ִ�е�ǰѡ�е�Action
        HandleSelectedAction();
    }

    //����Buttonѡ�е�Action
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPosition());
            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyUIChanged?.Invoke(this,isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyUIChanged?.Invoke(this,isBusy);
    }
    public bool GetIsBusy()
    {
        return isBusy;
    }

    //�����ײ������Unit����ô��ѡ��Unit
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit result;
            if (Physics.Raycast(ray, out result, float.MaxValue, unitLayerMask))
            {
                if (result.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }


    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        //Ĭ������£�ѡ��һ��Unit��Ĭ�϶�����Move
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnActionChanged?.Invoke(this,EventArgs.Empty);
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }


}
