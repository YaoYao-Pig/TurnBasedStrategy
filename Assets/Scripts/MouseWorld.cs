using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld _instance;
    public static MouseWorld Instance
    {
        get {
            return _instance;
        }
        private set { }
    }
    [SerializeField] private LayerMask mousePlaneLayerMask;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    public Vector3 GetPosition()
    {
                
        Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        Physics.Raycast(ray,out result,float.MaxValue, mousePlaneLayerMask);
        return result.point;
    }

}
