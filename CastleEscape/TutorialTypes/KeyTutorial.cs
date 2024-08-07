using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : TutorialBase
{
    private bool _anyKeyCollected = false;
    protected override void Start()
    {
        base.Start();
        Key.AnyKeyAcquired += OnAnyKeyAcquired;
    }

    private void OnDisable(){
        Key.AnyKeyAcquired -= OnAnyKeyAcquired;
    }

    private void OnAnyKeyAcquired(Key.KeyType unused){
        _anyKeyCollected = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _anyKeyCollected;
    }

}
