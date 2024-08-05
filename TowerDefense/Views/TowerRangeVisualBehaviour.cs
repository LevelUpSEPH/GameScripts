using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerRangeVisualBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject _towerRangeSphere;
    public virtual void ShowSphere(float radius){
        _towerRangeSphere.transform.localScale = Vector3.one * radius * 2;
        _towerRangeSphere.SetActive(true);
    }   

    public virtual void HideSphere(){
        _towerRangeSphere.SetActive(false);
    }
    
}
