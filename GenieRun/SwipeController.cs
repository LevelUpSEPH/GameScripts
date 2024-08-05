using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : Singleton<SwipeController>
{
    private float _horizontalInput;
    [SerializeField] private float _speedLimit = 35;

    protected override void Awake()
    {
        base.Awake();
        RegisterEvents();
    }

    private void OnDisable(){
        UnRegisterEvents();
    }

    private void RegisterEvents(){
        InputController.Dragging += OnDragging;
        InputController.DragEnd += OnStopDrag;
    }

    private void UnRegisterEvents(){
        InputController.Dragging -= OnDragging;
        InputController.DragEnd -= OnStopDrag;
    }

    private void OnDragging(PointerEventData eventData){
        _horizontalInput = Mathf.Clamp(eventData.delta.x, -_speedLimit, _speedLimit);
        Debug.Log("dragging: " + eventData.delta);
    }

    private void OnStopDrag(Vector3 pos){
        _horizontalInput = 0;
        Debug.Log("dragg stop");
    }

    public float GetHorizontalInput(){
        return _horizontalInput;
    }

}
