using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.Scripts.Abstract;
using FrogGridContent = Chameleon.Game.Scripts.Controller.FrogGridContent;

public class TestingHexGrid : MonoBehaviour // GridManager
{
    public static event Action<bool> GameFinished;
    public static event Action<int> MoveCountChanged;
    [SerializeField] private int _moveCount = 5;
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    
    [Tooltip("Drag all grid objects in order")]
    [SerializeField] private List<LayerObjectListPair> _objectListsByLayer;

    private Dictionary<int, Dictionary<XYCoord, GameObject>> _gridObjectDictionaryByLayer;

    private struct XYCoord
    {
        public int x;
        public int y;

        public XYCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private int _widthToUse;
    private int _heightToUse;

    private bool _isInputEnabled = true;
    private bool _isInClick = false;

    private GridHexXZ<GridObject> gridHexXZ;
    private int _frogCount = 0;

    private void Start()
    {
        FrogGridContent.ClearedGrid += OnGridCleared;
        FrogGridContent.TongueAnimationComplete += OnTongueAnimationComplete;
        InputController.PointerDown += OnPointerDown;
        InputController.PointerUp += OnPointerUp;
        InputController.DragBegin += OnDragged;

        GridObject.GridHasFrog += OnGridHasFrog;
        FrogGridContent.ClearedGrid += OnFrogCountDecreased;

        SetWidthAndHeight();

        StartCoroutine(WaitAndBuildDict());

        InvokeMoveCountChanged(_moveCount);
    }

    private void OnDisable()
    {
        FrogGridContent.ClearedGrid -= OnGridCleared;
        FrogGridContent.TongueAnimationComplete -= OnTongueAnimationComplete;
        InputController.PointerDown -= OnPointerDown;
        InputController.PointerUp -= OnPointerUp;
        InputController.DragBegin -= OnDragged;

        GridObject.GridHasFrog -= OnGridHasFrog;
        FrogGridContent.ClearedGrid -= OnFrogCountDecreased;
    }

    private void SetWidthAndHeight() // can change according to game camera angle
    {
        _widthToUse = _height;
        _heightToUse = _width;
    }

    private void InitializeGridObjects()
    {
        CreateGridHex();

        foreach(var keyValuePairP in _gridObjectDictionaryByLayer.Values)
        {
            foreach(var keyValuePair in keyValuePairP)
            {
                gridHexXZ.SetGridObject(keyValuePair.Key.x, keyValuePair.Key.y, keyValuePair.Value.GetComponent<GridObject>());
            }
        }
    }

    private void CreateGridHex()
    {
        float cellSize = 1.04f;
        
        gridHexXZ = 
            new GridHexXZ<GridObject>(_widthToUse, _heightToUse, cellSize, Vector3.zero);
    }

    private IEnumerator WaitAndBuildDict()
    {
        yield return new WaitForSeconds(.2f);
        BuildDict();
        InitializeGridObjects();
    }

    private void BuildDict()
    {
        _gridObjectDictionaryByLayer = new Dictionary<int, Dictionary<XYCoord, GameObject>>();

        int layer = 1;
        foreach(var gridObjectList in _objectListsByLayer)
        {
            Dictionary<XYCoord, GameObject> tempDict = new Dictionary<XYCoord, GameObject>();
            for (int x = 0; x < _widthToUse; x++)
            {
                for (int z = 0; z < _heightToUse; z++)
                {
                    if(gridObjectList.objectList.Count == 0)
                        break;
                    
                    tempDict.Add(new XYCoord(x, z), gridObjectList.objectList[0]);
                    gridObjectList.objectList.RemoveAt(0);
                }
            }
            _gridObjectDictionaryByLayer.Add(layer, tempDict);
            layer++;
        }
    }

    private int GetNumberFromString(string targetString, int charStartIndex, char targetKey)
    {
        int totalSum = 0;
        int multiplier = 10;
        for(int j = charStartIndex + 1; targetString[j] != targetKey; j++)
        {
            totalSum *= multiplier;
            totalSum += (int)targetString[j];
        }

        return totalSum;
    }

    private void OnPointerDown()
    {
        if(!_isInputEnabled)
            return;
        _isInClick = true;
    }

    private void OnPointerUp(Vector3 pointerUpPosition)
    {
        if(!_isInputEnabled)
            return;
        if(!_isInClick)
            return;
        
        _isInClick = false;

        GridObject hitObject;
        if(gridHexXZ.TryGetGridObject(Mouse3D.GetMouseWorldPosition(pointerUpPosition), out hitObject))
        {
            hitObject.InteractWithContent();
            if(hitObject.TryGetGridContent(out IGridContent gridContent))
            {
                if(gridContent is FrogGridContent)
                {
                    if(_moveCount > 0)
                    {
                        FrogGridContent frogContent = (gridContent as FrogGridContent);
                        if(frogContent.TryFlingTongue(gridHexXZ))
                            UseMove();
                    }
                }
            }
        }
    }

    private void OnDragged(Vector3 unused)
    {
        _isInClick = false;
    }

    private void OnTongueAnimationComplete()
    {
        CheckGameFinished();
    }

    private void OnGridCleared(GridObject clearedGrid)
    {
        clearedGrid.EmptyGridObject();
    }

    private void OnGridHasFrog()
    {
        _frogCount++;
    }

    private void OnFrogCountDecreased(GridObject unimportant)
    {
        _frogCount--;
    }
    
    private void UseMove()
    {
        _moveCount--;
        InvokeMoveCountChanged(_moveCount);
    }

    private void InvokeMoveCountChanged(int moveCount)
    {
        MoveCountChanged?.Invoke(moveCount);
    }

    private void CheckGameFinished()
    {
        if(gridHexXZ.GetIsEmpty())
        {
            _isInputEnabled = false;
            GameFinished?.Invoke(true);
        }
        else if(_moveCount <= 0 || _frogCount > _moveCount)
        {
            _isInputEnabled = false;
            GameFinished?.Invoke(false);
        }
    }
}

[System.Serializable]
public class LayerObjectListPair
{
    public int layer;
    public List<GameObject> objectList;
}