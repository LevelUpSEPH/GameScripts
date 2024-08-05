using Chameleon.Game.ArcadeIdle.Helpers;

namespace Chameleon.Game.ArcadeIdle.Upgrade
{
    public class PoopUpgradeButtonActivationController : UpgradeButtonsActivationController
    {
        protected override int GetInitialButtonCountToActivate()
        {
            return PoopTypeActivityController.instance.GetActivePoopTypeCount();
        }

        protected override void RegisterEvents()
        {
            PoopShowPlace.ShowplaceActivated += ActivateNextButton;
        }

        protected override void UnregisterEvents()
        {
            PoopShowPlace.ShowplaceActivated -= ActivateNextButton;
        }
    }
}