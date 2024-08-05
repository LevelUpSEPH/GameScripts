using DG.Tweening;
using UnityEngine;
using Lofelt.NiceVibrations;
using Chameleon.Game.Scripts.Model;

namespace Chameleon.Game.Scripts.Abstract
{
    public abstract class GridContentBase : MonoBehaviour, IGridContent
    {
        public bool CanInteract { get; set; }
        [SerializeField] private AudioClip _interactSound;
        [SerializeField] protected Transform _transformToScale;

        protected Vector3 _startingScale = Vector3.one;
        protected GridColor _gridColor;

        protected virtual void Start()
        {
            SetStartingScale();
            CanInteract = true;
        }

        public abstract void Interact();

        public void PlayInteractSound()
        {
            SoundController.instance.PlayAudio(_interactSound);
        }

        public void PlayInteractVibration()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        protected virtual void OnInteract()
        {
            PlayInteractSound();
            PlayInteractVibration();
        }

        public void ScaleUpContent()
        {
            SetActive(true);
            _transformToScale.localScale = Vector3.one * .1f;
            _transformToScale.DOScale(_startingScale, .3f).OnComplete(() => 
            {
                CanInteract = true;
            });
        }

        public void ScaleDownContent()
        {
            _transformToScale.localScale = Vector3.one * .1f;
            SetActive(false);
            CanInteract = false;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetGridColor(GridColor gridColor)
        {
            _gridColor = gridColor;

            // set material or image
        }

        public GridColor GetGridColor()
        {
            return _gridColor;
        }

        private void SetStartingScale()
        {
            _startingScale = _transformToScale.localScale;
        }

        #if UNITY_EDITOR
        public abstract void SetMaterial(GridColor gridColor);
        #endif
    }
}