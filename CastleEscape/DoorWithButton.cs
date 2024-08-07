using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorWithButton : Door
{
    [SerializeField] private GameObject _bars;
    public void OpenDoorWithButton(){
        UnlockDoor();
    }

    protected override void UnlockDoor()
    {
        _bars.transform.DOScaleX(0, 2f).OnComplete(() => Destroy(_doorBlock));
    }
}
