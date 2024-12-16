using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private static GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private static GridSystemVisualSingle lastGridSystemVisualSingle;
    private static GridSystemVisual _instance;
    
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,Blue,Red, RedSoft,Yellow
    }
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    public static GridSystemVisual Instance
    {
        get
        {
            return _instance;
        }
        private set
        {

        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); ++x)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); ++z)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform= Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();


            }
        }
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnSelectedActionChanged;
        UpdateGridVisual();
/*        for(int x = 0; x < LevelGrid.Instance.GetWidth(); ++x)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); ++z)
            {
                gridSystemVisualSingleArray[x, z].Show(GetGridVisualTypeMaterial(GridVisualType.White));
            }
        }*/
    }

    private void Update()
    {
        if (lastGridSystemVisualSingle != null)
        {
            lastGridSystemVisualSingle.HideSelected();
        }
        Vector3 mouseWorldPosition = MouseWorld.Instance.GetPosition();
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(mouseWorldPosition);
        if (LevelGrid.Instance.IsValidGridPosition(gridPosition))
        {
            lastGridSystemVisualSingle = gridSystemVisualSingleArray[gridPosition.x, gridPosition.z];
        }
        //更新选中位置
        if (lastGridSystemVisualSingle != null)
        {
            lastGridSystemVisualSingle.ShowSelected();
        }
    }
    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); ++x)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); ++z)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }
    public void ShowGridPositionList(List<GridPosition> gridPositionList,GridVisualType gridVisualType)
    {
        Material material = GetGridVisualTypeMaterial(gridVisualType);
        foreach (var gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(material);
        }
    }
    private void ShowGridPositionRange(GridPosition gridPosition,int range,GridVisualType gridVisualType)
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -range; x <= range; ++x)
        {
            for(int z = -range; z <= range; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; ++x)
        {
            for (int z = -range; z <= range; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    private void UpdateGridVisual()
    {
        GridSystemVisual.Instance.HideAllGridPosition();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType = GridVisualType.White;
        switch (selectedAction)
        {
            case MoveAction moveAction:
                gridVisualType=GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(shootAction.GetUnit().GetGridPosition(), shootAction.GetMaxShootDistance(),GridVisualType.RedSoft);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(swordAction.GetUnit().GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }


        GridSystemVisual.Instance.ShowGridPositionList(selectedAction.GetValidActionGridPosition(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender,EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    public Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(var gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError("Couldn't find GridVisualMaterial");
        return null;
    }
}
