using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointPicker : Singleton<RandomPointPicker>
{

    private List<Transform> _mapEdges = new List<Transform>();
    [SerializeField] private LayerMask _obstructionLayer = 64;
    [SerializeField] private LayerMask _groundLayer = 512;
    private float _xClampPositive;
    private float _zClampPositive;
    private float _xClampNegative;
    private float _zClampNegative;

    private void Start(){
        InLevelController.MapEdgesSet += OnMapEdgesSet;
        if(InLevelController.instance.IsMapEdgesSet)
            InitializeMapEdges();
    }

    private void OnDisable()
    {
        InLevelController.MapEdgesSet -= OnMapEdgesSet;
    }

    private void OnMapEdgesSet()
    {
        InitializeMapEdges();
    }

    private void InitializeMapEdges(){
        _mapEdges.Clear();
        _mapEdges = InLevelController.instance.GetMapEdges();

        FindMapLimits();
    }

    private void FindMapLimits(){
        if(_mapEdges.Count <= 0)
            return;
        _xClampNegative = _mapEdges[0].position.x;
        _xClampPositive = _mapEdges[1].position.x;
        _zClampPositive = _mapEdges[0].position.z;
        _zClampNegative = _mapEdges[2].position.z;

        HandlePointDuos(ref _xClampNegative,ref _xClampPositive);
        HandlePointDuos(ref _zClampNegative, ref _zClampPositive);
    }

    private void HandlePointDuos(ref float negativeClamp,ref float positiveClamp)
    {
        if(negativeClamp > positiveClamp)
        {
            float tempValue = negativeClamp;
            negativeClamp = positiveClamp;
            positiveClamp = tempValue;
        }
    }

    public bool TryGetRandomSpotToMove(out Vector3 spotToMove){
        Vector3 point = GetRandomPoint();
        if(!IsPointObstructed(point) && IsPointInWorld(point)){
            spotToMove = point;
            return true;
        }
        else
        {
            spotToMove = Vector3.zero;
            return false;
        }
    }

    private Vector3 GetRandomPoint()
    {
        float xPos = Random.Range(_xClampNegative, _xClampPositive);
        float zPos = Random.Range(_zClampNegative, _zClampPositive);
        return new Vector3(xPos, transform.position.y, zPos);
    }

    private bool IsPointObstructed(Vector3 point){
        Vector3 pointOffset = Vector3.up * 4f;
        if(Physics.Raycast(point + pointOffset, Vector3.down, 10f, _obstructionLayer))
            return true;
        return false;
    }

    private bool IsPointInWorld(Vector3 point){
        Vector3 pointOffset = Vector3.up * 2f;
        if(Physics.Raycast(point + pointOffset, Vector3.down, 5f, _groundLayer))
            return true;
        return false;

    }
    
}
