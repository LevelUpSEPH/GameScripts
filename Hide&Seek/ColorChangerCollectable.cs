using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private Colors.Color _collectableColor;

    public void Collect(){
        ColorManager.instance.SetPlayerColor(_collectableColor);
    }
}
