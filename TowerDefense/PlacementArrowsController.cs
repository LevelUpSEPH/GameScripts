using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlacementArrowsController : MonoBehaviour
{
    private List<Transform> _placementArrowTransforms = new List<Transform>();
    private List<Vector3> _placementArrowLocalPositions = new List<Vector3>();

    private void Awake(){
        InitializeList();
    }

    private void InitializeList(){
        foreach(Transform child in transform){
            _placementArrowTransforms.Add(child);
            _placementArrowLocalPositions.Add(child.transform.localPosition);
        }
    }


    private void OnEnable(){
        TweenArrows();
    }

    private void OnDisable(){
        ResetArrows();
    }

    private void TweenArrows(){
        foreach(Transform arrowTransform in _placementArrowTransforms){
            Vector3 targetPos =  arrowTransform.localPosition * 1.25f;
            targetPos.y = arrowTransform.localPosition.y;
            arrowTransform.DOLocalMove(targetPos, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }

    private void ResetArrows(){
        StopTweens();
        ResetArrowsPositions();
    }
    
    private void ResetArrowsPositions(){
        for(int i = 0; i < _placementArrowLocalPositions.Count; i++){
            _placementArrowTransforms[i].localPosition = _placementArrowLocalPositions[i];
        }
    }

    private void StopTweens(){
        foreach(Transform arrowTransform in _placementArrowTransforms)
            arrowTransform.DOKill();
    }
}
