public class PatrolMonsterController : MonsterController
{
    private bool _sawPlayer;

    protected override void OnSawPlayer()
    {
        if(_sawPlayer)
            return;
        _sawPlayer = true;
        Catch(_playerController.GetComponent<IHider>());
    }
}
