using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class HighlightBehaviour : MonoBehaviour
{
    [SerializeField] Transform _highlightTarget;

    private void Start() {
        _highlightTarget.DOScale(1.05f, 1.2f).SetLoops(-1, LoopType.Yoyo);    
    }
}
