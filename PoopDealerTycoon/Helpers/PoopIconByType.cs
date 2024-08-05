using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketUtils.SerializableDictionary;
using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopIconByType : Singleton<PoopIconByType>
    {
        [SerializeField] private PoopIcons _poopIcons = new PoopIcons();
        [SerializeField] private Sprite _checkoutIcon;

        public Sprite GetPoopIconByType(PoopType poopType)
        {
            return _poopIcons[poopType];
        }

        public Sprite GetCheckoutIcon()
        {
            return _checkoutIcon;
        }
    }

    [Serializable]
    public class PoopIcons : SerializableDictionary<PoopType, Sprite>{}
}