using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCastleHealthbar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _castleHealthText;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Gradient _gradient;

    private float _health;

    private void Start(){
        MainCastle.CastleHealthChanged += OnCastleHealthChanged;
        LevelBehaviour.Started += OnLevelStarted;
    }

    private void OnDisable(){
        MainCastle.CastleHealthChanged -= OnCastleHealthChanged;
        LevelBehaviour.Started -= OnLevelStarted;
        StopAllCoroutines();
    }

    private void HandleHealthBarPercentage(){
        _healthBar.fillAmount = _health;
    }

    private void HandleGradientAmount() {
        _healthBar.color = _gradient.Evaluate(_health);
    }

    private void OnCastleHealthChanged(){
        MainCastle mainCastle = GameObject.FindGameObjectWithTag("MainCastle").GetComponent<MainCastle>();
        StartCoroutine(UpdateHealthbarTo(mainCastle.GetHealthNormalized()));
    }

    private void OnLevelStarted(){
        _health = 1f; // normalized max value
        HandleHealthBarPercentage();
        HandleGradientAmount();
        UpdateHealthText();
    }

    private IEnumerator UpdateHealthbarTo(float newHp){
        if(_health > newHp)
            while((_health - newHp) > 0.01f){
                _health -= 0.01f;
                HandleHealthBarPercentage();
                HandleGradientAmount();
                UpdateHealthText();
                yield return new WaitForSeconds(0.01f);
        }
        else
            while((newHp - _health > 0.01f)){
                _health += 0.01f;
                HandleHealthBarPercentage();
                HandleGradientAmount();
                UpdateHealthText();
                yield return new WaitForSeconds(0.01f);
            }
    }

    private void UpdateHealthText(){
        //_castleHealthText.text = "HP : " + ((int)(_health * 100.0f)).ToString();
    }

}
