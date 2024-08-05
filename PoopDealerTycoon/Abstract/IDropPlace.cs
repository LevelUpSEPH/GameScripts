namespace Chameleon.Game.ArcadeIdle.Abstract
{
    public interface IDropPlace
    {
        public abstract bool TryDropPoop(PoopBase poopBase);
        public abstract bool GetCanReceivePoop(PoopBase poopBase = null);
        public abstract PoopType GetAcceptedPoopType();
    }
}