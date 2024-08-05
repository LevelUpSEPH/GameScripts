using UnityEngine;
using Chameleon.Game.Scripts.Abstract;
using Chameleon.Game.Scripts.Model;
using UnityEditor;
using UnityEngine.UI;


namespace Chameleon.Game.Scripts.Controller
{
    public class ReflectorGridContent : GridContentBase
    {
        [SerializeField] private TongueDirection _reflectDirection;
        [SerializeField] private Image _arrowImage;

        public override void Interact()
        {
            if(!CanInteract)
                return;
            
            PlayInteractAnimation();

            OnInteract();
        }

        private void PlayInteractAnimation()
        {
            // maybe glow / spin or sth?
        }

        public TongueDirection GetReflectDirection()
        {
            return _reflectDirection;
        }

        #region SCENE_BUILDING        
            #if UNITY_EDITOR
                [SerializeField] private Transform modelTransform;
                SerializedObject serializedObject;
                SerializedProperty directionProperty;
                TongueDirection currentDirection;

                private void OnValidate()
                {
                    if(Application.isPlaying)
                        return;

                    serializedObject = new SerializedObject(this);
                    directionProperty = serializedObject.FindProperty(nameof(_reflectDirection));
                    TongueDirection newDirection = (TongueDirection)directionProperty.enumValueIndex;
                    if(currentDirection != newDirection)
                    {
                        currentDirection = newDirection;
                    }

                    float zRotation = GetRotationOfDirection(newDirection);
                    Vector3 targetRotation = modelTransform.localRotation.eulerAngles;
                    targetRotation.z = zRotation;
                    modelTransform.localRotation = Quaternion.Euler(targetRotation);
                }

                private float GetRotationOfDirection(TongueDirection newDirection)
                {
                    float zRotation;
                    switch(newDirection)
                    {
                        case TongueDirection.Up:
                            zRotation = 180;
                            break;
                        case TongueDirection.LeftUp:
                            zRotation = 240;
                            break;
                        case TongueDirection.LeftDown:
                            zRotation = 300;
                            break;
                        case TongueDirection.Down:
                            zRotation = 0;
                            break;
                        case TongueDirection.RightDown:
                            zRotation = 60;
                            break;
                        case TongueDirection.RightUp:
                            zRotation = 120;
                            break;
                        default:
                            zRotation = 0;
                            break;
                    }
                    return zRotation;
                }

                public override void SetMaterial(GridColor gridColor)
                {
                    _arrowImage.sprite = HexagonMaterials.instance.GetArrowIconOfColor(gridColor);
                }
        #endif
        #endregion
    }
}