using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StaticMonsterMovementController : MonsterMovementControllerBase
{
    [SerializeField] private float _rotationAngle = 45f;

    [Range(1.51f, 10)]
    [SerializeField] private float _rotationInterval = 3f;

    protected override void OnDisable()
    {
        base.OnDisable();
        StopMovement();
    }

    private IEnumerator StartRotation()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(_rotationInterval);
            Vector3 endRotation = transform.rotation.eulerAngles + Vector3.up * _rotationAngle;
            float duration = 1.5f;
            transform.DORotate(endRotation, duration, RotateMode.FastBeyond360);
        }
    }

    public override void StartMovement()
    {
        StartCoroutine(StartRotation());
    }

    public override void StopMovement()
    {
        StopAllCoroutines();
    }
}
