using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : Singleton<MaterialHolder>
{
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _purpleMaterial;
    [SerializeField] private Material _uncoloredMaterial;


    public Material GetMaterialOfColor(Colors.Color color){
        if(color == Colors.Color.Blue)
            return _blueMaterial;
        else if(color == Colors.Color.Red)
            return _redMaterial;
        else if (color == Colors.Color.Yellow)
            return _yellowMaterial;
        else if (color == Colors.Color.Purple)
            return _purpleMaterial;
        else
            return _uncoloredMaterial;
    }
}
