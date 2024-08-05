namespace Chameleon.Game.ArcadeIdle
{
    public class PoopDropperHorse : AnimatedPoopDropper
    {
        protected override void SpawnPoop()
        {
            StartCoroutine(SpawnPoopAndWait());
        }
    }
}