using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorTutorial : TutorialBase
{
    private bool _anyKeyUsed = false;

    protected override void Start()
    {
        base.Start();
        PlayerController.AnyKeyUsed += OnAnyKeyUsed;
    }

    private void OnDisable(){
        PlayerController.AnyKeyUsed -= OnAnyKeyUsed;
    }

    private void OnAnyKeyUsed(){
        _anyKeyUsed = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _anyKeyUsed;
    }

}
