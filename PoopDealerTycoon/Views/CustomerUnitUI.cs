using UnityEngine;
using UnityEngine.UI;
using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Units
{
    public class CustomerUnitUI : NeedsUI
    {
        [SerializeField] private CustomerUnit _customerUnit;
        [SerializeField] private Image _nextWantedPoopImage;

        protected override void OnEnable()
        {
            SetVisualsActive(true);
            base.OnEnable();
        }
        protected override void RegisterEvents()
        {
            _customerUnit.NeedsChanged += UpdateWantedImage;
            _customerUnit.CustomerInitialized += UpdateWantedImage;
            _customerUnit.CustomerLeft += OnUnitLeft;
        }

        protected override void UnregisterEvents()
        {
            _customerUnit.NeedsChanged -= UpdateWantedImage;
            _customerUnit.CustomerInitialized -= UpdateWantedImage;
            _customerUnit.CustomerLeft -= OnUnitLeft;
        }

        protected override void UpdateWantedImage()
        {
            PoopType nextNeedType = GetNextWant();
            if(nextNeedType != PoopType.None)
                _nextWantedPoopImage.sprite = PoopIconByType.instance.GetPoopIconByType(nextNeedType);
            else
                _nextWantedPoopImage.sprite = PoopIconByType.instance.GetCheckoutIcon();
        }

        protected PoopType GetNextWant()
        {
            return _customerUnit.GetNextWant();
        }

        private void OnUnitLeft()
        {
            SetVisualsActive(false);
        }
    }
}