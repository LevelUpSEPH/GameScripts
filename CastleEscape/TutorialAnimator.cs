using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialAnimator : MonoBehaviour
{
    [SerializeField] private float _tweenInterval = 1.3f;
    [SerializeField] private Transform _tutorialArrow;
    [SerializeField] private Transform _tutorialText;

    void Start()
    {
        _tutorialArrow.DOMoveY(_tutorialArrow.position.y + .3f, _tweenInterval)
        .OnComplete(() => _tutorialArrow.DOMoveY(_tutorialArrow.position.y - .3f, _tweenInterval))
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear);

        Vector3 textStartScale = new Vector3(_tutorialText.localScale.x, _tutorialText.localScale.y, _tutorialText.localScale.z);
        Vector3 textEndScale = new Vector3(_tutorialText.localScale.x + 0.2f, _tutorialText.localScale.y + 0.2f, _tutorialText.localScale.z + 0.2f);

        _tutorialText.DOScale(textEndScale, _tweenInterval)
        .OnComplete(() => _tutorialText.DOScale(textStartScale, _tweenInterval))
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear);
    }
}
