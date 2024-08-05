using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chameleon.Game.Scripts.Timer
{
    public class LevelTimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _remainingTimeText;
        [SerializeField] private Image _clockImage;
        [SerializeField] private GameObject _clock;

        private void Update()
        {
            HandleUIElementsActivation();
            if(!LevelTimerController.instance.IsTimerActive)
                return;

            float remainingTime = LevelTimerController.instance.GetRemainingTime();
            UpdateUIElements(remainingTime);
        }

        private void HandleUIElementsActivation()
        {
            if(_clock.activeInHierarchy && !LevelTimerController.instance.IsTimerActive)
            {
                SetUIElementsActive(false);
            }
            else if(!_clockImage.gameObject.activeInHierarchy && LevelTimerController.instance.IsTimerActive)
            {
                SetUIElementsActive(true);
            }
        }

        private void SetUIElementsActive(bool isActive)
        {
            _clock.SetActive(isActive);
            _remainingTimeText.gameObject.SetActive(isActive);
        }

        private void UpdateUIElements(float remainingTime)
        {
            UpdateRemainingTimeText(remainingTime);
            UpdateClockFillAmount();
            HandleTextColor(remainingTime);
        }

        private void UpdateRemainingTimeText(float remainingTime)
        {
            if(remainingTime > 9.5f)
                _remainingTimeText.text = "00:" + remainingTime.ToString(".");
            else
                _remainingTimeText.text = "00:0" + remainingTime.ToString(".");
        }

        private void UpdateClockFillAmount()
        {
            _clockImage.fillAmount = LevelTimerController.instance.GetRemainingTimePercentage();
        }

        private void HandleTextColor(float remainingTime)
        {
            if(remainingTime < 9.5f && _remainingTimeText.color != Color.red)
            {
                ActivateRedText();
            }
            else if(remainingTime > 9.5f && _remainingTimeText.color != Color.white)
            {
                ActivateNormalText();
            }
        }

        private void ActivateNormalText()
        {
            _remainingTimeText.color = Color.white;
        }

        private void ActivateRedText()
        {
            _remainingTimeText.color = Color.red;
        }
    }

}