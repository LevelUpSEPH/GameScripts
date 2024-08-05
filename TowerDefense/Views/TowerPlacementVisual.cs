using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;
    
    private void Awake(){
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void SetIsInBuildableZone(bool toSet){
        if(toSet)
            _meshRenderer.material = _greenMaterial;
        else
            _meshRenderer.material = _redMaterial;
    }

    public void SetVisibility(bool toSet){
        _meshRenderer.enabled = toSet;
    }
}
