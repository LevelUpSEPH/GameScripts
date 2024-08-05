using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Chameleon.Game.ArcadeIdle.Unit;

namespace Chameleon.Game.ArcadeIdle
{
    public class SwitchCarDirectionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private GameObject _visuals;
        [SerializeField] private GameObject _forwardGroup;
        [SerializeField] private GameObject _reverseGroup;
        [SerializeField] private PlayerUnitController _targetPlayer;
        private CarController _activeCar;
        private Image _inputTarget;
        private float _inputThreshold;
        private Vector2 _dragBeginPosition;
        private bool _dragged = false;

        private void Start()
        {
            _inputTarget = GetComponent<Image>();
            _inputThreshold = (_inputTarget.rectTransform.sizeDelta.y / 4);

            _targetPlayer.EnteredCar += OnEnteredCar;
            _targetPlayer.LeftCar += OnLeftCar;
        }

        private void OnDestroy()
        {
            _targetPlayer.EnteredCar -= OnEnteredCar;
            _targetPlayer.LeftCar -= OnLeftCar;
        }

        private void OnEnteredCar(CarController enteredCar)
        {
            _activeCar = enteredCar;
            SetInputActive(true);
            UpdateSwitchButton();
            SetVisualsActive(true);
        }

        private void OnLeftCar()
        {
            _activeCar = null;
            SetInputActive(false);
            SetVisualsActive(false);
        }

        private void SetInputActive(bool isActive)
        {
            _inputTarget.raycastTarget = isActive;
        }

        private void SetVisualsActive(bool isActive)
        {
            _visuals.SetActive(isActive);
        }

        private void SwitchCarDirection()
        {
            _activeCar.ToggleIsOnForward();
            UpdateSwitchButton();
        }

        private void SwitchToForward()
        {
            _activeCar.SetIsOnForward(true);
            UpdateSwitchButton();
        }

        private void SwitchToReverse()
        {
            _activeCar.SetIsOnForward(false);
            UpdateSwitchButton();
        }

        private void UpdateSwitchButton()
        {
            SwitchActiveImage(_activeCar.GetIsOnForward());
        }

        private void SwitchActiveImage(bool isForward)
        {
            _forwardGroup.SetActive(isForward);
            _reverseGroup.SetActive(!isForward);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragged = true;
            _dragBeginPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float delta;
            delta = _dragBeginPosition.y - eventData.position.y;

            if(delta >= _inputThreshold && _activeCar.GetIsOnForward())
            {
                SwitchToReverse();
            }
            else if(delta <= -_inputThreshold && !_activeCar.GetIsOnForward())
            {
                SwitchToForward();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragged = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(!_dragged)
                SwitchCarDirection();
        }
    }
}