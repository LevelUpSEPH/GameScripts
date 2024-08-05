using System;
using UnityEngine;
using DG.Tweening;
using RocketNavigation.Models;
using Game.Scripts.Models;

namespace Chameleon.Game.ArcadeIdle.Popups
{
    public class UpgradePopup : PopupView
    {
        public static event Action UpgradePopupOpened;

        protected override void OnOpened()
        {
            base.OnOpened();
            UpgradePopupOpened?.Invoke();
        }

        protected override void OpeningAnimation()
		{
			transform.DOKill();
			transform.localScale = Vector3.one * 0.3f;
			
				transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
				{
					OnOpened();
                    if(PlayerData.Instance.TutorialSaveData.IsSequenceCompleted(Tutorial.StepSystem.Data.TutorialSequenceType.UpgradePlayerStackSequence))
					    SetCloseButtonVisibility(true);
                    else
                        PlayerData.Stats[StatKeys.PlayerStackLevel].Changed += OnTutorialComplete;
				}).SetUpdate(UpdateType.Normal, true).timeScale = 1;
		}

        private void OnTutorialComplete()
        {
            PlayerData.Stats[StatKeys.PlayerStackLevel].Changed -= OnTutorialComplete;
            
            SetCloseButtonVisibility(true);
        }
    }
}