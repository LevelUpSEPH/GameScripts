using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private Image _healthBar;
    [SerializeField] private GameObject _healthBarParent;
    [SerializeField] private Gradient _gradient;
    
    private bool _hasHealth = true;

    private void OnEnable(){
        _healthBarParent.SetActive(false);
        _hasHealth = true;
    }

    private void Update(){
        HandleGradientAmount();
    }

    public void UpdateVisuals(){
        float healthPercentage = _enemyController.GetHealthPercentage();
        _healthBar.fillAmount = healthPercentage;

        if(healthPercentage <= 0){
            _healthBarParent.SetActive(false);
            _hasHealth = false;
        }

        if(!_hasHealth)
            return;

        StopAllCoroutines();


        SetVisualsActive(true);
        StartCoroutine(HealthShownTime());
    }

    private void HandleGradientAmount() {
        _healthBar.color = _gradient.Evaluate(_enemyController.GetHealthPercentage());
    }

    private IEnumerator HealthShownTime(){
        yield return new WaitForSeconds(1.5f);
        SetVisualsActive(false);
    }

    private void SetVisualsActive(bool toSet){
        _healthBarParent.SetActive(toSet);
    }

}   
