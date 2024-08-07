using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorTutorial : TutorialBase
{
    private bool _anyButtonPressed = false;
    protected override void Start()
    {
        base.Start();
        OpenDoorButton.AnyDoorButtonPressed += OnAnyDoorButtonPressed;
    }

    private void OnDisable(){
        OpenDoorButton.AnyDoorButtonPressed -= OnAnyDoorButtonPressed;
    }

    private void OnAnyDoorButtonPressed(){
        _anyButtonPressed = true;
    }

    protected override bool HasMetTutorialCondition()
    {
        return _anyButtonPressed;
    }
}
