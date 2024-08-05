using UnityEngine;
using TMPro;

namespace Chameleon.Game.ArcadeIdle
{
    public class PoopShowplaceUI : MonoBehaviour
    {
        [SerializeField] private PoopShowPlace _poopShowplace;
        [SerializeField] private TextMeshProUGUI _poopAmountText;

        private void Start()
        {
            _poopShowplace.PoopCountChanged += UpdateAmountText;
            UpdateAmountText();
        }
        
        private void OnDisable()
        {
            _poopShowplace.PoopCountChanged -= UpdateAmountText;
        }

        private void UpdateAmountText()
        {
            _poopAmountText.text = _poopShowplace.GetCurrentCount() + "/" + _poopShowplace.GetMaxCount();
        }
    }
}
