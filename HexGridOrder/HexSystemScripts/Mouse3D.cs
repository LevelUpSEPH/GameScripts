using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    //[SerializeField] private Transform mouseTransform;

    private void Awake() {
        Instance = this;
    }

    public static Vector3 GetMouseWorldPosition() {
        if (Instance == null) {
            Debug.LogError("Mouse3D Object does not exist!");
        }
        return Instance.GetMouseWorldPosition_Instance();
    }

    public static Vector3 GetMouseWorldPosition(Vector3 screenPos)
    {
       if(Instance == null)
        {
            Debug.Log("Error getting an instance of Mouse3D");
        }
        return Instance.GetMouseWorldPosition_Instance(screenPos);
    }

    private Vector3 GetMouseWorldPosition_Instance(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }

    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            return raycastHit.point;
        } else {
            return Vector3.zero;
        }
    }

}
