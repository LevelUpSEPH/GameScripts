using UnityEngine;
using TMPro;

namespace Chameleon.Game.ArcadeIdle
{
    public class PlayerSlotsManagerStatusUI : MonoBehaviour
    {
        [SerializeField] private PlayerPoopSlotsManager _playerSlotsManager;
        [SerializeField] private GameObject _fullVisuals;

        private void Start()
        {
            _playerSlotsManager.AllSlotsFilled += OnAllSlotsFilled;
            _playerSlotsManager.SomeSlotsEmptied += OnSomeSlotsEmptied;
        }

        private void OnDisable()
        {
            _playerSlotsManager.AllSlotsFilled -= OnAllSlotsFilled;
            _playerSlotsManager.SomeSlotsEmptied -= OnSomeSlotsEmptied;
        }

        private void OnAllSlotsFilled()
        {
            if(_fullVisuals.activeInHierarchy)
                return;
            SetFullVisualsActive(true);
        }

        private void OnSomeSlotsEmptied()
        {
            if(!_fullVisuals.activeInHierarchy)
                return;    
            SetFullVisualsActive(false);
        }

        private void SetFullVisualsActive(bool isVisualsActive)
        {
            _fullVisuals.SetActive(isVisualsActive);
        }
    }
}
