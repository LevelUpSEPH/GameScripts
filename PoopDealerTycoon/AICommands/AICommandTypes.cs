using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public enum AICommandType
    {
        TakePoop, // get poop type and go to colection zone
        TakePoopFromShow, // get poop type and go to a free point from showplace
        PlacePoop, // get poop type and go to placement zone of the poop type
        PlacePoopOnShow, // get poop type and go to placement point of poop type
        GotoCheckout, // get free checkout position
        LeaveShop // get random position outside
    }
}
