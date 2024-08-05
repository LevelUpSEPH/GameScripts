using System;
using UnityEngine;
using Chameleon.Game.Scripts.Abstract;
using Chameleon.Game.Scripts.Model;
using DG.Tweening;
using Sirenix.OdinInspector;
using Chameleon.Game.Scripts.Controller;
using UnityEditor;

public class GridObject : MonoBehaviour
{
    public static event Action<Vector3, GridObject> GridInitialized;
    public static event Action<Vector3> GridChanged;
    public static event Action GridHasFrog;
    
    [SerializeField] private GameObject _selectedVisual;
    [SerializeField] private Transform _gridContentPositionReference;
    [SerializeField] private GridContentBase _gridContent;
    [SerializeField] private GridColor _gridColor;

    private bool _isEmpty = false;

    private void Start()
    {
        if(_gridContent == null)
            return;
            
        if(_gridContent is FrogGridContent)
            GridHasFrog?.Invoke();
    }

    public void ShowAndHandleContentInitialState()
    {
        _selectedVisual.SetActive(true);
        
        if(_gridContent != null)
        {
            _gridContent.SetActive(true);
            _gridContent.SetGridColor(_gridColor);
        }
        else
        {
            _isEmpty = true;
            GridInitialized?.Invoke(transform.position, this);
        }
    }

    public void Show()
    {
        _selectedVisual.SetActive(true);

        if(_gridContent != null)
        {
            _gridContent.ScaleUpContent();
            _gridContent.SetGridColor(_gridColor);
        }
        else
        {
            EmptyGridObject();
        }
    }

    public void Hide()
    {
        ScaleDownAndDisable();
        HideContent();
    }

    public void HideWithoutTween()
    {
        _selectedVisual.SetActive(false);
        HideContent();
    }

    private void ScaleDownAndDisable()
    {
        _selectedVisual.transform.DOScale(Vector3.one * .1f, .25f).OnComplete(() => _selectedVisual.SetActive(false));
    }

    public void HideContent(bool forceHide = false)
    {
        if(_gridContent != null && _gridContent.gameObject.activeInHierarchy)
        {
            if(!(_gridContent is FruitGridContent) || forceHide)
            {
                _gridContent.ScaleDownContent();
            }
        }
    }
    
    public GridColor GetGridColor()
    {
        return _gridColor;
    }
    
    public bool TryGetGridContent(out IGridContent gridContent)
    {
        if(_gridContent == null)
        {
            gridContent = null;
            return false;
        }
        gridContent = _gridContent;
        return true;
    }

    // for debugging, will swap out later on
    public void EmptyGridObject()
    {
        _isEmpty = true;

        GridChanged?.Invoke(transform.position);
    }

    public void InteractWithContent()
    {
        if(_gridContent != null)
            _gridContent.Interact();
    }

    public bool GetIsEmpty()
    {
        return _isEmpty;
    }

    #region SCENE_BUILDING

        #if UNITY_EDITOR
            [SerializeField] MeshRenderer gridMeshRenderer;
            [Header("Grid Content Spawning")]
            [SerializeField] private GridContentType _gridContentType;

            [SerializeField] private GameObject _fruitGridContentPrefab;
            [SerializeField] private GameObject _frogGridContentPrefab;
            [SerializeField] private GameObject _rotatingFrogGridContentPrefab;
            [SerializeField] private GameObject _reflectorGridContentPrefab;

            SerializedObject serializedObject;
            SerializedProperty colorProperty;
            GridColor currentGridColor = GridColor.Default;

            [Button]
            private void AttachContent()
            {
                switch (_gridContentType)
                {
                    case GridContentType.Fruit:
                        AttachContent(_fruitGridContentPrefab);
                        break;
                    case GridContentType.Frog:
                        AttachContent(_frogGridContentPrefab);
                        break;
                    case GridContentType.RotatingFrog:
                        AttachContent(_rotatingFrogGridContentPrefab);
                        break;
                    case GridContentType.Reflector:
                        AttachContent(_reflectorGridContentPrefab);
                        break;
                }
            }

            [Button]
            private void ClearGrid()
            {
                if(_gridContent != null)
                {
                    DestroyImmediate(_gridContent.gameObject);
                    _gridContent = null;
                }
            }

            private void AttachContent(GameObject contentPrefab)
            {
                if(_gridContent != null)
                    DestroyImmediate(_gridContent.gameObject);

                GameObject content = PrefabUtility.InstantiatePrefab(contentPrefab) as GameObject;
                GridContentBase gridContent = content.GetComponent<GridContentBase>();

                Transform layerParent = transform.parent.parent;

                Transform targetLayerParent = layerParent.transform.Find("GridContent");

                string parentName;
                
                if(gridContent is FrogGridContent)
                {
                    parentName = "Frog";
                }
                else
                {
                    parentName = _gridContentType.ToString();
                }

                parentName = parentName + "Contents";

                Transform contentParent = targetLayerParent.Find(parentName);
        
                content.transform.position = _gridContentPositionReference.position;
                content.transform.parent = contentParent;

                _gridContent = gridContent;

                SetContent();
            }

            private void OnValidate()
            {
                if(Application.isPlaying)
                    return;

                serializedObject = new SerializedObject(this);
                colorProperty = serializedObject.FindProperty("_gridColor");
                GridColor newGridColor = (GridColor)colorProperty.enumValueIndex;
                if(currentGridColor != newGridColor)
                {
                    currentGridColor = newGridColor;
                }

                // set both content and self colors
                if(_gridContent != null)
                {
                    _gridContent.SetMaterial(newGridColor);

                    _gridContent.transform.position = _gridContentPositionReference.position;
                }
                Material hexMaterial = HexagonMaterials.instance.GetHexMaterialOfColor(newGridColor);
                if(hexMaterial != null)
                    gridMeshRenderer.material = hexMaterial;
            }

            private void SetContent()
            {
                if(_gridContent != null)
                {
                    _gridContent.SetMaterial(_gridColor);

                    _gridContent.transform.position = _gridContentPositionReference.position;
                }
            }

            public enum GridContentType
            {
                Fruit,
                Frog,
                RotatingFrog,
                Reflector
            }
        
        #endif

    #endregion
}
