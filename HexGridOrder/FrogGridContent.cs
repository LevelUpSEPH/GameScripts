using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Chameleon.Game.Scripts.Abstract;
using DG.Tweening;
using Chameleon.Game.Scripts.Model;
using Lofelt.NiceVibrations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chameleon.Game.Scripts.Controller
{
    public class FrogGridContent : GridContentBase
    {
        public static event Action<GridObject> ClearedGrid;
        public static event Action TongueAnimationComplete;

        public event Action AteFruit;
        public event Action FlungTongue;
        public event Action ReturnToIdle;

        [SerializeField] protected TongueDirection _tongueDirection;
        [SerializeField] private LineRenderer _tongue;
        [SerializeField] private float _animationSpeed = 4;
        [SerializeField] private Transform _tongueInitialSpot;

        private delegate Vector3Int ApplyFlingDirectionDelegate(Vector3Int coordinate, int amountToMove);
        private ApplyFlingDirectionDelegate ApplyFlingDirection;

        private GridHexXZ<GridObject> _gridHex;

        private Vector3 _currentPosition;

        private Dictionary<GridContentBase, int> _gridContentPathStartNodePair = new Dictionary<GridContentBase, int>();
        private List<GridObject> _gridNodes = new List<GridObject>();
        private List<GridObject> _heldFruits = new List<GridObject>();
        private Transform _nearestToFrog;
        private bool _ateFruit = false;

        private int _tonguedFruits = 0;
        private int _ateFruits = 0;

        public override void Interact()
        {
            if(!CanInteract)
                return;
            
            PlayInteractAnimation();

            SetDirectionGetter();

            OnInteract();
        }

        private void SetDirectionGetter()
        {
            ApplyFlingDirection = GetFunctionByDirection(_tongueDirection);
        }

        private ApplyFlingDirectionDelegate GetFunctionByDirection(TongueDirection direction)
        {
            switch (direction)
            {
                case TongueDirection.Up:
                    return AxialCoordinateMovementHelper.MoveUp;
                case TongueDirection.LeftUp:
                    return AxialCoordinateMovementHelper.MoveUpperLeft;
                case TongueDirection.LeftDown:
                    return AxialCoordinateMovementHelper.MoveLowerLeft;
                case TongueDirection.Down:
                    return AxialCoordinateMovementHelper.MoveDown;
                case TongueDirection.RightDown:
                    return AxialCoordinateMovementHelper.MoveLowerRight;
                case TongueDirection.RightUp:
                    return AxialCoordinateMovementHelper.MoveUpperRight;
                default:
                    return null;
            }
        }

        public virtual bool TryFlingTongue(GridHexXZ<GridObject> gridHex)
        {
            if(!CanInteract)
                return false;
            if(_gridHex == null)
                _gridHex = gridHex;
            
            _tonguedFruits = 0;
            _ateFruits = 0;

            CanInteract = false;
            _currentPosition = transform.position;

            InvokeFlungTongue();
            
            ResetGridNodes();
            _gridContentPathStartNodePair.Clear();
            
            StartCoroutine(IterateThroughPathNode());
            return true;
        }

        private bool _isInIteration = false;

        private IEnumerator IterateThroughPathNode()
        {
            _isInIteration = false;

            TongueReturnType tongueReturnType = TongueReturnType.Continue;
            while(tongueReturnType == TongueReturnType.Continue)
            {
                _gridHex.GetXZ(_currentPosition, out int x, out int z);
                Vector2Int convertedNewCoord = ApplyMovementAndReturn(new Vector2Int(x, z));

                yield return new WaitForSeconds(.01f);
                if(!_isInIteration)
                {
                    _isInIteration = true;
                    tongueReturnType = HandleNewGrid(convertedNewCoord);
                }
            }


            if(_gridNodes.Count <= 1)
            {
                ResetGridNodes();
                CanInteract = true;
                InvokeReturnToIdle();
                InvokeTongueAnimationComplete();
                yield break;
            }

            StartCoroutine(DrawTongueAcrossNodes(() =>
            {
                DOVirtual.DelayedCall(.5f, () => 
                {
                    if(tongueReturnType == TongueReturnType.FailEnd)
                    {
                        _gridContentPathStartNodePair.Clear();
                        StartCoroutine(DrawTongueBackwards(false));
                    }
                    else
                    {
                        StartCoroutine(DrawTongueBackwards(true));
                    }
                });
            }));

            yield break;
        }

        private IEnumerator DrawTongueAcrossNodes(Action onDrawComplete)
        {
            _tongue.positionCount = 2;;
            _tongue.SetPosition(0, _tongueInitialSpot.position);
            _tongue.SetPosition(1, _tongueInitialSpot.position);
            int drawnPoints = 1;
            bool isInTween = false;
            GameObject lineTarget = new GameObject("TongueLineTarget");
            lineTarget.transform.SetParent(transform);
            lineTarget.transform.position = _tongueInitialSpot.position;            

            while(drawnPoints < _tongue.positionCount)
            {
                _tongue.SetPosition(drawnPoints, lineTarget.transform.position);
                if(!isInTween)
                {
                    isInTween = true;
                    if(_gridNodes.Count >= drawnPoints + 1)
                    {
                        Vector3 targetPosition = _gridNodes[drawnPoints].transform.position;
                        targetPosition.y = targetPosition.y + .2f;
                        lineTarget.transform.DOMove(targetPosition, _animationSpeed).SetSpeedBased(true)
                        .OnComplete(() => 
                        {
                            Vector3 pointToSet = _gridNodes[drawnPoints].transform.position;
                            pointToSet.y = pointToSet.y + .2f;
                            _tongue.SetPosition(drawnPoints, pointToSet);

                            InteractWithContent(_gridNodes[drawnPoints]);
                            
                            drawnPoints++;
                            if(_tongue.positionCount < _gridNodes.Count)
                            {
                                _tongue.positionCount++;
                                Vector3 finalPoint = _gridNodes[drawnPoints - 1].transform.position;
                                finalPoint.y = finalPoint.y + .2f;
                                _tongue.SetPosition(_tongue.positionCount - 1, finalPoint);
                            }
                            isInTween = false;
                        })
                        .SetEase(Ease.Linear);
                    }
                    else
                        break;
                }
                yield return new WaitForSeconds(.025f);
            }
            Destroy(lineTarget);

            onDrawComplete?.Invoke();
            yield break;
        }

        private IEnumerator DrawTongueBackwards(bool willEatFruits)
        {
            SoundController.instance.PlayAudio(SoundsHolder.instance.PullBackFoodSound);

            int drawnPoints = _tongue.positionCount - 1;
            bool isInTween = false;
            GameObject lineTarget = new GameObject("TongueLineTarget");
            lineTarget.transform.SetParent(transform);
            Vector3 lineTargetPosition = _gridNodes[drawnPoints].transform.position;
            lineTargetPosition.y = lineTargetPosition.y + .2f;
            lineTarget.transform.position = lineTargetPosition;
            _gridHex.GetXZ(lineTarget.transform.position, out int x, out int z);
            Vector2Int currentTongueTipPosition = new Vector2Int(x, z);
            SetNearestToFrog(lineTarget.transform);

            float failSpeedMultiplier = willEatFruits ? 1 : 2;
            
            if(willEatFruits)
            {
                TrySendGridFruitToFrog(_gridNodes[drawnPoints]);
            }

            while(drawnPoints > 0)
            {
                _tongue.SetPosition(drawnPoints, lineTarget.transform.position);

                Vector3 targetPosition = _gridNodes[drawnPoints - 1].transform.position;
                targetPosition.y = targetPosition.y + .2f;
                if(!isInTween)
                {
                    isInTween = true;
                    Tweener tweener = lineTarget.transform.DOMove(targetPosition, _animationSpeed * failSpeedMultiplier)
                    .OnComplete(() => 
                    {
                        drawnPoints--;
                        _tongue.positionCount--;;
                        isInTween = false;
                    })
                    .SetEase(Ease.Linear)
                    .SetSpeedBased(true);

                    if(willEatFruits)
                    {
                        AnimateFruitsReturning(tweener, lineTarget.transform, currentTongueTipPosition);
                    }
                }
                yield return new WaitForSeconds(.01f);
            }

            yield return new WaitForSeconds(.1f);
            Destroy(lineTarget);

            if(!_ateFruit)
                InvokeTongueAnimationComplete();

            InvokeReturnToIdle();
            
            if(_ateFruit)
            {
                RemoveFrog();
            }
            else
            {
                CanInteract = true;
                ResetGridNodes();
                _ateFruit = false;
            }
            
            yield break;
        }

        private void AnimateFruitsReturning(Tweener tweener, Transform tongueTip, Vector2Int currentTongueTipPosition)
        {
            
            _gridHex.GetXZ(tongueTip.position, out int TTx, out int TTz);
            _gridHex.TryGetGridObject(TTx, TTz, out GridObject initialLastGridObject);
            GridObject lastGridObject = initialLastGridObject;

            tweener.OnUpdate(() => 
            {
                if(_nearestToFrog == null)
                    return;
                _gridHex.GetXZ(_nearestToFrog.position, out int x, out int z);
                Vector2Int newTongueTipPosition = new Vector2Int(x, z);
                if(newTongueTipPosition != currentTongueTipPosition)
                {
                    // currentTongueTip hex clear
                    currentTongueTipPosition = newTongueTipPosition;

                    if(_gridHex.TryGetGridObject(currentTongueTipPosition.x, currentTongueTipPosition.y, out GridObject gridObject))
                        if(!_heldFruits.Contains(gridObject))
                        {
                            if(TrySendGridFruitToFrog(gridObject))
                            {
                                _heldFruits.Add(gridObject);
                            }
                        }
                }

                Debug.DrawLine(tongueTip.position, tongueTip.position + Vector3.up * 3f, Color.red);
                _gridHex.GetXZ(tongueTip.position, out int tipX, out int tipZ);
                if(_gridHex.TryGetGridObject(tipX, tipZ, out GridObject tipGridObject))
                {
                    if(tipGridObject != lastGridObject && lastGridObject != null)
                    {
                        ClearedGrid?.Invoke(lastGridObject);
                        lastGridObject = tipGridObject;
                    }
                }
            });
        }

        private bool TrySendGridFruitToFrog(GridObject targetGrid)
        {
            GridContentBase lastGridContent = null;
            if(targetGrid.TryGetGridContent(out IGridContent gridContent))
            {
                lastGridContent = (gridContent as GridContentBase);

                if(lastGridContent is FruitGridContent)
                {
                    
                    if(!_gridContentPathStartNodePair.ContainsKey(lastGridContent))
                    {
                        return false;
                    }

                    (lastGridContent as FruitGridContent).SetCanAnimate(false);
                    MoveFruitToIndex(lastGridContent, _gridContentPathStartNodePair[lastGridContent]);
                    SetNearestToFrog(lastGridContent.transform);
                    _ateFruit = true;
                    return true;
                }
            }
            return false;
        }

        private void InteractWithContent(GridObject gridObject)
        {
            gridObject.InteractWithContent();

            if(!gridObject.TryGetGridContent(out IGridContent gridContent))
            {
                return;
            }
            if(!(gridContent is FruitGridContent))
            {
                return;
            }
            if(gridObject.GetGridColor() == _gridColor)
            {
                float tonguePitch = Mathf.Pow(1.05946f, _tonguedFruits);

                SoundController.instance.PlayAudio(SoundsHolder.instance.CorrectFoodSound, pitch: tonguePitch);
                _tonguedFruits++;
                return;
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            SoundController.instance.PlayAudio(SoundsHolder.instance.WrongFoodSound);
            (gridContent as FruitGridContent).FlashRed();
        }

        private Vector2Int ApplyMovementAndReturn(Vector2Int currentCoordinate)
        {
            Vector3Int convertedCoord = HexCoordinateConverter.ConvertToAxialCoordinates(currentCoordinate, _gridHex.GetHeight());

            Vector3Int newCoord = ApplyFlingDirection(convertedCoord, 1);

            Vector2Int convertedNewCoord = HexCoordinateConverter.ConvertToHexagonalCoordinates(newCoord, _gridHex.GetHeight());

            return convertedNewCoord;
        }

        private TongueReturnType HandleNewGrid(Vector2Int nextGridPosition)
        {
            if(!_gridHex.TryGetGridObject(nextGridPosition.x, nextGridPosition.y, out GridObject gridObject))
            {
                // Debug.Log("There is no such grid object, returning succesfully");
                _isInIteration = false;
                return TongueReturnType.SuccessEnd;
            }

            if(gridObject.GetGridColor() != _gridColor)
            {
                if(TryHandleNextGridHasColor(nextGridPosition.x, nextGridPosition.y, out TongueReturnType tongueReturnType))
                {
                    return tongueReturnType;
                }

                // Debug.Log("Next grid object has a different color, returning failingly");
                _isInIteration = false;

                if(gridObject.TryGetGridContent(out IGridContent unusedFruit))
                    _gridNodes.Add(gridObject);
                return TongueReturnType.FailEnd;
            }

            if(!gridObject.TryGetGridContent(out IGridContent gridContent))
            {
                Debug.LogWarning("Hit an empty grid object, returning succesfully but why is it empty and enabled?");
                _isInIteration = false;
                return TongueReturnType.SuccessEnd;
            }

            _currentPosition = _gridHex.GetWorldPosition(nextGridPosition.x, nextGridPosition.y);
            _gridNodes.Add(gridObject);

            if(gridContent is ReflectorGridContent)
            {
                HandleInteractionWithReflector(gridContent as ReflectorGridContent);
                return TongueReturnType.Continue;
            }

            if(gridContent is FruitGridContent)
            {
                HandleInteractionWithFruit(gridContent as FruitGridContent);
                return TongueReturnType.Continue;
            }

            HandleNoInteraction();
            return TongueReturnType.FailEnd;
        }

        private bool TryHandleNextGridHasColor(int xPos, int yPos, out TongueReturnType tongueReturnType)
        {
            if(_gridHex.TryGetAllGridObjects(xPos, yPos, out List<GridObject> allGridObjects))
            {
                bool containsSameColor = false;
                foreach(GridObject gridObjectInList in allGridObjects)
                {
                    if(gridObjectInList.GetGridColor() == _gridColor)
                    {
                        containsSameColor = true;
                        break;
                    }
                }
                if(!containsSameColor)
                {
                    _isInIteration = false;
                    tongueReturnType = TongueReturnType.SuccessEnd;
                    return true;
                }
            }
            tongueReturnType = TongueReturnType.FailEnd;
            return false;
        }

        private void HandleInteractionWithFruit(FruitGridContent fruit)
        {
            _gridContentPathStartNodePair.Add(fruit, _gridNodes.Count - 2);

            _isInIteration = false;
        }

        private void MoveFruitToIndex(GridContentBase gridContent, int pointIndexToMove)
        {

            if(pointIndexToMove == 0)
            {
                gridContent.transform.DOScale(Vector3.one * .1f, (1f/(_animationSpeed / 2)))
                .OnComplete(() => gridContent.gameObject.SetActive(false));
            }
            gridContent.transform.DOMove(_gridNodes[pointIndexToMove].transform.position, _animationSpeed)
            .OnComplete(() => 
            {
                if(pointIndexToMove == 0)
                {
                    gridContent.transform.DOScale(Vector3.one * .1f, .25f)
                    .OnComplete(() => gridContent.gameObject.SetActive(false));

                    InvokeAteFruit();
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

                    float eatPitch = Mathf.Pow(1.05946f, _ateFruits);
                    
                    SoundController.instance.PlayAudio(SoundsHolder.instance.EatFoodSound, pitch: eatPitch);
                    _ateFruits++;

                    _gridContentPathStartNodePair.Remove(gridContent);
                }
                else
                {
                    pointIndexToMove--;
                    MoveFruitToIndex(gridContent, pointIndexToMove);
                }
            })
            .SetSpeedBased(true)
            .SetEase(Ease.Linear);
        }

        private void HandleInteractionWithReflector(ReflectorGridContent reflector)
        {
            ApplyFlingDirection = GetFunctionByDirection(reflector.GetReflectDirection());
            _isInIteration = false;
        }

        private void HandleNoInteraction()
        {
            Debug.LogWarning("None of the if checks worked, check for any issues");
            _isInIteration = false;

            ResetGridNodes();
            _gridContentPathStartNodePair.Clear();
        }

        private void PlayInteractAnimation()
        {
            _transformToScale.DOKill();
            _transformToScale.localScale = _startingScale;
            
            _transformToScale.DOPunchScale(_startingScale * 0.3f, .3f, 1);            
        }

        private void SetNearestToFrog(Transform newNearestToFrog)
        {
            _nearestToFrog = newNearestToFrog;
        }

        private void ResetGridNodes()
        {
            _heldFruits.Clear();
            _gridNodes.Clear();
            SetNearestToFrog(null);
            
            _gridHex.GetXZ(transform.position, out int x, out int z);
            if(_gridHex.TryGetGridObject(x, z, out GridObject gridObject))
                _gridNodes.Add(gridObject);
            else
                Debug.LogWarning("Something went wrong while initializing grid nodes");
        }

        private void InvokeTongueAnimationComplete()
        {
            TongueAnimationComplete?.Invoke();
        }

        private void RemoveFrog()
        {
            GridObject thisGridObject;
            _gridHex.TryGetGridObject(transform.position, out thisGridObject);
            ClearedGrid?.Invoke(thisGridObject);

            InvokeTongueAnimationComplete();
        }

        private void InvokeAteFruit()
        {
            AteFruit?.Invoke();
        }

        private void InvokeFlungTongue()
        {
            FlungTongue?.Invoke();
        }

        private void InvokeReturnToIdle()
        {
            ReturnToIdle?.Invoke();
        }

        protected void SetTongueDirection(TongueDirection tongueDirection)
        {
            _tongueDirection = tongueDirection;

            SetDirectionGetter();
        }

        private float GetRotationOfDirection(TongueDirection newDirection)
        {
            float yRotation;
            switch(newDirection)
            {
                case TongueDirection.Up:
                    yRotation = 0;
                    break;
                case TongueDirection.LeftUp:
                    yRotation = 300;
                    break;
                case TongueDirection.LeftDown:
                    yRotation = 240;
                    break;
                case TongueDirection.Down:
                    yRotation = 180;
                    break;
                case TongueDirection.RightDown:
                    yRotation = 120;
                    break;
                case TongueDirection.RightUp:
                    yRotation = 60;
                    break;
                default:
                    yRotation = 0;
                    break;
            }
            return yRotation;
        }

        #region SCENE_BUILDING
            #if UNITY_EDITOR
                SerializedObject serializedObject;
                SerializedProperty directionProperty;
                TongueDirection currentDirection;
                [SerializeField] private Renderer _frogMeshRenderer;

                private void OnValidate()
                {
                    if(Application.isPlaying)
                        return;

                    serializedObject = new SerializedObject(this);
                    directionProperty = serializedObject.FindProperty(nameof(_tongueDirection));
                    TongueDirection newDirection = (TongueDirection)directionProperty.enumValueIndex;
                    if(currentDirection != newDirection)
                    {
                        currentDirection = newDirection;
                    }
                    float yRotation = GetRotationOfDirection(newDirection);
                    Vector3 targetRotation = transform.localRotation.eulerAngles;
                    targetRotation.y = yRotation;
                    transform.localRotation = Quaternion.Euler(targetRotation);
                }

                public override void SetMaterial(GridColor gridColor)
                {
                    Material gridMaterial = HexagonMaterials.instance.GetFrogMaterialOfColor(gridColor);
                    if(gridMaterial != null)
                        _frogMeshRenderer.material = gridMaterial;
                }
        #endif
        #endregion SCENE_BUILDING
    }


    public enum TongueDirection
    {
        Up,
        LeftUp,
        LeftDown,
        Down,
        RightDown,
        RightUp
    }

    public enum TongueReturnType
    {
        Continue,
        FailEnd,
        SuccessEnd
    }
}