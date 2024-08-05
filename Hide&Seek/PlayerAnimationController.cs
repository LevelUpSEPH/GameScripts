using UnityEngine;

public class PlayerAnimationController : AnimationControllerBase
{
    [SerializeField] private ParticleSystem _blueSmoke;
    [SerializeField] private ParticleSystem _yellowSmoke;
    [SerializeField] private ParticleSystem _redSmoke;
    [SerializeField] private ParticleSystem _purpleSmoke;
    [SerializeField] private ParticleSystem _cageSmoke;

    public void SetIsHidden(bool toSet){
        if(_isAnimatorValid)
            _targetAnimator.SetBool("Hiding", toSet);
    }

    public void PlaySmokeParticle(Colors.Color color) {
        switch (color) {
            case Colors.Color.Blue:
                _blueSmoke.Play();
                break;
            case Colors.Color.Yellow:
                _yellowSmoke.Play();
                break;
            case Colors.Color.Red:
                _redSmoke.Play();
                break;
            case Colors.Color.Purple:
                _purpleSmoke.Play();
                break;
            case Colors.Color.Uncolored:
                break;
            default:
                break;
        }
    }

    public void PlayCageSmokeParticle() {
        _cageSmoke.Play();
    }
}
