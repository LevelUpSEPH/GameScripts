using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpray : MonoBehaviour
{
    [SerializeField] private Colors.Color _colorToPaint;
    [SerializeField] private Transform _sprayerTransform;
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            PressPlate();
        }
    }

    private void PressPlate(){
        RaycastHit raycastHit;
        if(!Physics.Raycast(_sprayerTransform.position, _sprayerTransform.forward, out raycastHit, 2f, 64)){
            return;
        }
        if(!raycastHit.collider.CompareTag("Wall"))
            return;
        WallController wall = raycastHit.collider.GetComponent<WallController>();
        wall.GetComponent<IColorable>().Paint(_colorToPaint);
    }
}
