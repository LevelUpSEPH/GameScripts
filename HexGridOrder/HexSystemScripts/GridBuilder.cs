using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class GridBuilder : MonoBehaviour
{
    #if UNITY_EDITOR
    public event Action GridBuilt;
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    [SerializeField] private int _layerCount = 3;

    private int widthToUse;
    private int heightToUse;

    [SerializeField] private GameObject pfHex;
    [SerializeField] private GameObject backgroundHex;
    private Queue<CoordinateObjectPair> _instantiatedQueue;

    private GridHexXZ<GridObject> gridHexXZ;

    private bool _isRoutineActive = false;
    private int _builtLayers = 0;
    private Transform _targetHexParent;

    private void CreateGridHex()
    {
        float cellSize = 1.04f;
        widthToUse = _height;
        heightToUse = _width;

        gridHexXZ = new GridHexXZ<GridObject>(widthToUse, heightToUse, cellSize, Vector3.zero);
    }

    [Button]
    private void BuildGrids()
    {
        if(_isRoutineActive)
            return;
        if(_targetHexParent == null)
        {
            GameObject hexParent = GameObject.Find("HexSystem");
            if(hexParent)
                _targetHexParent = hexParent.transform;
        }

        _builtLayers = 0;

        GridBuilt += BuildGrid;
        
        BuildGrid();
    }

    private void BuildGrid()
    {
        if(_builtLayers > _layerCount)
        {
            GridBuilt -= BuildGrid;
            _isRoutineActive = false;
            return;
        }

        if(_builtLayers == 0)
        {
            BuildBackgroundGrid();
        }
        else
        {
            BuildNormalGrid(_builtLayers);
        }

        _builtLayers++;
        
    }

    private void BuildNormalGrid(int layer)
    {
        Debug.Log("Starting Normal Build");

        GameObject layerParent = new GameObject("Layer" + layer);
        if(_targetHexParent != null)
            layerParent.transform.parent = _targetHexParent;
        
        // setting hierarchy
        GameObject hexParent = new GameObject("HexContent");
        GameObject gridContentParent = new GameObject("GridContent");

        GameObject gridContentFrogParent = new GameObject("FrogContents");
        GameObject gridContentFruitParent = new GameObject("FruitContents");
        GameObject gridContentReflectorParent = new GameObject("ReflectorContents");

        hexParent.transform.parent = layerParent.transform;
        gridContentParent.transform.parent = layerParent.transform;

        gridContentFrogParent.transform.parent = gridContentParent.transform;
        gridContentFruitParent.transform.parent = gridContentParent.transform;
        gridContentReflectorParent.transform.parent = gridContentParent.transform;

        BuildGrid(pfHex, hexParent.transform);

        layerParent.transform.position = new Vector3(0, 0.1f * (layer - 1), 0);
    }

    private void BuildBackgroundGrid()
    {
        Debug.Log("Starting Background Build");

        GameObject backGroundParent = new GameObject("BackgroundLayer");
        if(_targetHexParent != null)
            backGroundParent.transform.parent = _targetHexParent;

        BuildGrid(backgroundHex, backGroundParent.transform);

        backGroundParent.transform.position = new Vector3(0, -0.1f, 0);
    }

    private void BuildGrid(GameObject prefabToUse, Transform parent)
    {
        EditorApplication.update -= OnEditorUpdate;

        _instantiatedQueue = new Queue<CoordinateObjectPair>();
        CreateGridHex();

        GameObject gameObjectToInstantaite = GetGameObjectToInstantiate(prefabToUse);

        EditorApplication.update += OnEditorUpdate;

        for (int x = 0; x < widthToUse; x++)
        {
            for (int z = 0; z < heightToUse; z++)
            {
                GameObject instantiatedObject = (PrefabUtility.InstantiatePrefab(gameObjectToInstantaite) as GameObject);
                
                AddNewGridToQueue(x, z, instantiatedObject);

                instantiatedObject.transform.parent = parent;
            }
        }
    }

    private GameObject GetGameObjectToInstantiate(GameObject prefabToUse)
    {
        string assetpath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabToUse);
            
        GameObject gameObjectToInstantiate = AssetDatabase.LoadAssetAtPath(assetpath, typeof(GameObject)) as GameObject;
                
        return gameObjectToInstantiate;
    }

    private void AddNewGridToQueue(int x, int z, GameObject instantiatedObject)
    {
        CoordinateObjectPair newPair = new CoordinateObjectPair();
                
        newPair.x = x;
        newPair.z = z;
        newPair.instantiatedGameObject = instantiatedObject;

        _instantiatedQueue.Enqueue(newPair);         
    }

    private int _editorTick = 0;
    private void OnEditorUpdate()
    {
        _editorTick++;
        if(_instantiatedQueue.Count <= 0)
            return;
        if(_editorTick % 5 != 0)
            return;

        CoordinateObjectPair finishedPair = _instantiatedQueue.Dequeue();
        GameObject instantiatedObject = finishedPair.instantiatedGameObject;
        int x = finishedPair.x;
        int z = finishedPair.z;

        SetObjectProperties(instantiatedObject, x, z);
        instantiatedObject.transform.localPosition = new Vector3(instantiatedObject.transform.localPosition.x, 0, instantiatedObject.transform.localPosition.z);

        if(x + 1 == widthToUse && z + 1 == heightToUse)
        {
            EditorApplication.update -= OnEditorUpdate;
            GridBuilt?.Invoke();
            Debug.Log("Build Complete");
        }
    }

    private void SetObjectProperties(GameObject targetObject, int x, int z)
    {
        targetObject.transform.position = gridHexXZ.GetWorldPosition(x, z);
        targetObject.transform.rotation = Quaternion.identity;

        targetObject.name = "Hex(" + x + "," + z + ")";
    }
    
    private struct CoordinateObjectPair
    {
        public int x;
        public int z;
        public GameObject instantiatedGameObject;
    }

    #endif
}