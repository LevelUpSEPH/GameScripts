namespace Chameleon.Game.Scripts.Abstract
{
    public interface IGridContent
    {
        bool CanInteract {get;set;}
        void Interact();
        void PlayInteractSound();
        void PlayInteractVibration();
    }
}