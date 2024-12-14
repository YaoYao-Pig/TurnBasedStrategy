using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathFindingDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;

    [SerializeField] private TextMeshPro hCostText;

    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableImage;


    private PathNode pathNode;
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;

    }
    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetfCost().ToString();
        isWalkableImage.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
