using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MouseController : Singleton<MouseController>
{
    //private bool _isClickAvailable = false;
    
    
    private void Start(){
        InputController.PointerDown += OnPointerDown;
        //InputController.PointerUp += OnPointerUp;
    }

    private void OnDisable(){
        InputController.PointerDown -= OnPointerDown;
        //InputController.PointerUp -= OnPointerUp;
    }

    private void OnClick(Vector2 mousePos){
        // can select placed towers
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit raycastHit;
        if(Physics.Raycast(ray, out raycastHit, 50f, 256)){
            if(raycastHit.collider.gameObject.TryGetComponent<TowerBase>(out TowerBase tower)){
                if(tower != SelectedTowerController.instance.GetSelectedTower())
                    SelectedTowerController.instance.DeselectTower();
                SelectedTowerController.instance.SelectTower(tower); 
            }   
        }
        else{
                SelectedTowerController.instance.DeselectTower();
            }
    }

    public RaycastHit GetMouseWorldPositionHit(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if(Physics.Raycast(ray, out raycastHit, 50f, 128)){
            return raycastHit;
        }

        return raycastHit;
    }

    public RaycastHit GetWorldPositionFromScreen(Vector2 screenPos){
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit raycastHit;

        if(Physics.Raycast(ray, out raycastHit, 50f, 128)){
            return raycastHit;
        }

        return raycastHit;
    }

    /*private void OnPointerUp(){
        _isClickAvailable = true;

        if(SelectedTowerControllerUI.instance.GetIsTowerSelected()){
            SelectedTowerControllerUI.instance.DisableVisuals();
            Debug.Log("Tower was selec")
        }
    }
    */

    public void OnPointerDown(Vector3 mousePos){
        //if(!_isClickAvailable)
          //  return;

        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        OnClick(mousePos2d);
        //_isClickAvailable = false;
    }

}
