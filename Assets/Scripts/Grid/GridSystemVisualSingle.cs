using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRender;

    public void Show()
    {
        meshRender.enabled = true;
    }
    public void Hide()
    {
        meshRender.enabled = false;
    }
}
