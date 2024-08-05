using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class DjinnController : MonoBehaviour
{
    [SerializeField] private GameObject _djinnModel;
    [SerializeField] private TextMeshProUGUI _bubbleText;
    [SerializeField] private GameObject _bubbleParent;
    [SerializeField] private float _showForSeconds = 1;

    [SerializeField] private ParticleSystem _smokeParticle;


    private float _typingCD = 0.05f;
    public static event Action ShowedProphecy;

    private void OnEnable(){
        LampBehaviour.LampTransferFinished += OnLampTransferFinished;
    }

    private void OnDisable(){
        LampBehaviour.LampTransferFinished -= OnLampTransferFinished;
    }

    private void OnLampTransferFinished(){
        Debug.Log("lamp transfer finished");
        // throw particle        
        _djinnModel.SetActive(true);
        PlaySmokeParticle();
        PlayIdleAnimation();

        EndLevelTextLibrary.instance.SetResults();
        
        StartCoroutine(TypeWrite(EndLevelTextLibrary.instance.GetTextResult()));
    }

    private IEnumerator TypeWrite(string text) {
        yield return new WaitForSeconds(2f); // camera transition time
        _bubbleParent.SetActive(true);
        foreach (char character in text) {
            _bubbleText.text += character.ToString();
            yield return new WaitForSeconds(_typingCD);
        }
        ShowedProphecy?.Invoke();
    }

    private void PlayIdleAnimation() {
        _djinnModel.transform.DOMoveY(transform.position.y + 0.7f, 1.5f)
                   .SetLoops(-1, LoopType.Yoyo)
                   /*.SetEase(Ease.OutCubic)*/;
    }

    private void PlaySmokeParticle() {
        _smokeParticle.Play();
    }
}