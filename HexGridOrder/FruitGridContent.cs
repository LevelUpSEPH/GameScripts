using UnityEngine;
using Chameleon.Game.Scripts.Abstract;
using DG.Tweening;
using Chameleon.Game.Scripts.Model;

namespace Chameleon.Game.Scripts.Controller
{
    public class FruitGridContent : GridContentBase
    {
        [SerializeField] private Renderer meshRenderer;
        private bool _isAnimationsActive = true;
        private Color _originalColor;

        protected override void Start()
        {
            base.Start();
            _originalColor = meshRenderer.material.color;
        }
        public override void Interact()
        {
            if(!CanInteract)
                return;
            
            PlayInteractAnimation();

            OnInteract();
        }

        private void PlayInteractAnimation()
        {
            if(!_isAnimationsActive)
                return;

            _transformToScale.DOKill();
            _transformToScale.localScale = _startingScale;
            
            _transformToScale.DOPunchScale(_startingScale * 1.02f, .3f, 1);
        }

        public void FlashRed()
        {
            if(!_isAnimationsActive)
                return;
            
            meshRenderer.material.DOKill();
            meshRenderer.material.color = _originalColor;
            meshRenderer.material.DOColor(Color.red, .15f).OnComplete(() => meshRenderer.material.DOColor(_originalColor, .15f));
        }

        public void SetCanAnimate(bool canAnimate)
        {
            _isAnimationsActive = canAnimate;
        }

        #region SCENE_BUILDING        
            #if UNITY_EDITOR
                public override void SetMaterial(GridColor gridColor)
                {
                    Material hexMaterial = HexagonMaterials.instance.GetFruitMaterialOfColor(gridColor);
                    
                    if(hexMaterial != null)
                        meshRenderer.material = hexMaterial;
                }
            #endif
        #endregion
    }

}