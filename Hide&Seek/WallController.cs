using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour, IColored, IColorable
{
    [SerializeField] private Colors.Color _wallColor;
    private MeshRenderer _wallMeshRenderer;

    private void Start(){
        _wallMeshRenderer = GetComponent<MeshRenderer>();
        Paint(_wallColor);
    }

    public Colors.Color GetColor(){
        return _wallColor;
    }

    public void Paint(Colors.Color color){        
        _wallColor = color;
        if (color == Colors.Color.Uncolored)
            return;
        _wallMeshRenderer.material = MaterialHolder.instance.GetMaterialOfColor(color);
    }
}
