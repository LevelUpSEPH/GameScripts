using Chameleon.Game.ArcadeIdle.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Chameleon.Game.ArcadeIdle
{
    public class NeedItem : MonoBehaviour
    {
        private Image _poopTypeImage;
        private PoopType _neededType;
        public PoopType NeededType => _neededType;

        private void Awake()
        {
            _poopTypeImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _poopTypeImage.sprite = null;
        }

        public void SetNeedType(PoopType poopType)
        {
            _neededType = poopType;
            if(_neededType != PoopType.None)
                _poopTypeImage.sprite = PoopIconByType.instance.GetPoopIconByType(_neededType);
            else
                _poopTypeImage.sprite = null;
        }

        public PoopType GetNeededType()
        {
            return _neededType;
        }

    }
}