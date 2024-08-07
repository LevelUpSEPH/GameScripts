using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : Door
{
    private List<GameObject> _doorBars;
    [SerializeField] private GameObject _doorBarsParent;
    private int _destroyIndex = 0;
    private int _barCount = 0;

    private void Awake(){
        _doorBars = new List<GameObject>();
        foreach(Transform child in _doorBarsParent.transform){
            if(child.gameObject.activeInHierarchy){
                _doorBars.Add(child.gameObject);
                _barCount++;
            }            
        }
    }

    private void Start(){
        Unit.AnyUnitDied += OnAnyUnitDied;
        if(_doorBars.Count == 0)
            UnlockDoor();
    }

    private void OnDisable(){
        Unit.AnyUnitDied -= OnAnyUnitDied;
    }

    private void OnAnyUnitDied(Unit unit){
        RemoveBar(unit);
        if(_barCount == 0){
            UnlockDoor();
            Debug.Log("All bars removed");
        }        
    }

    private void RemoveBar(Unit unit){
        if(unit.gameObject.CompareTag("Enemy")){
            Destroy(_doorBars[_destroyIndex]);
            _destroyIndex++;
            _barCount--;
        }
    }
}
