public class MonsterAnimationController : AnimationControllerBase
{
    public void PlayCatchingAnimation()
    {
        if(_isAnimatorValid)
            _targetAnimator.SetTrigger("Catching");
    }
}
