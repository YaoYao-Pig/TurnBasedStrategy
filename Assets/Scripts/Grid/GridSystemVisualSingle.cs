using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRender;
    [SerializeField] private GameObject selectedGameObject;

    public void Show(Material material)
    {
        meshRender.enabled = true;
        meshRender.material = material;
    }
    public void Hide()
    {
        meshRender.enabled = false;
    }

    public void ShowSelected()
    {
        selectedGameObject.SetActive(true);
    }
    public void HideSelected()
    {
        selectedGameObject.SetActive(false);
    }
}
