public class MonsterController : MonsterControllerBase
{
    private bool _isWalking;
    public void OnMonsterMovementStateChanged(bool isWalking){
        if(_isWalking == isWalking)
            return;
        _isWalking = isWalking;
        _monsterAnimationController.SetMoveAnimationPlaying(isWalking);
    }
}
