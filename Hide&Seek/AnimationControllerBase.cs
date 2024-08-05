using UnityEngine;

public class AnimationControllerBase : MonoBehaviour
{
    [SerializeField] protected Animator _targetAnimator;
    protected bool _isAnimatorValid => _targetAnimator != null;

    public void SetMoveAnimationPlaying(bool isMoving)
    {
        if(_isAnimatorValid)
            _targetAnimator.SetBool("IsMoving", isMoving);
    }
}
