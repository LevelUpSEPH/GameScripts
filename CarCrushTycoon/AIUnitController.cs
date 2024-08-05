using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;

namespace Chameleon.Game.ArcadeIdle.Unit
{
    public class AIUnitController : BaseUnitController
    {
        // holds a reference to the car they are inside, input mode changes to car if inside one
        protected override void OnEnteredCarTrigger(CarController triggeredCar)
        {
            // enter car
        }

        protected override void OnExitCarTrigger()
        {
        
        }
    }
}