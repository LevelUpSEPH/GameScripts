using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineOfSight : MonoBehaviour
{

    [SerializeField] protected float _radius; // length of fov
    
    [Range(0,360)]
    [SerializeField] protected float _angle; // width of fov
    protected float _angleMultiplier = 1f;

    [SerializeField] protected LayerMask _targetLayer;
    [SerializeField] protected LayerMask _obstructionLayer;

    [SerializeField] protected float _meshResolution = 1;

    [SerializeField] private MeshFilter _meshFilter;
    private Mesh _viewMesh;
    protected bool _isGameActive = false;

    public struct ViewCastInfo{
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;
    
        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle){
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }

    private void Awake(){
        StartCoroutine(FOVRoutine());
        InLevelController.GameStarted += OnGameStarted;
        InLevelController.LevelCompleted += OnLevelCompleted;
    }

    protected virtual void Start(){
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        _meshFilter.mesh = _viewMesh;
    }

    private void LateUpdate(){
        DrawFieldOfView();
    }

    private void OnDisable()
    {
        InLevelController.GameStarted -= OnGameStarted;
        InLevelController.LevelCompleted -= OnLevelCompleted;
    }

    private void OnGameStarted()
    {
        _isGameActive = true;
    }

    private void OnLevelCompleted(bool unused)
    {
        _isGameActive = false;
    }
    
    private ViewCastInfo ViewCast(float globalAngle){
        Vector3  dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dir,out hit, _radius, _obstructionLayer))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        else{
            return new ViewCastInfo(false, transform.position + dir * _radius, _radius, globalAngle);
        }
    }

    private IEnumerator FOVRoutine(){
        float delay = 0.1f;
        while(true){
            yield return new WaitForSeconds(delay);
            FieldofViewCheck();
        }
    }

    protected virtual void FieldofViewCheck(){
        if(!_isGameActive)
            return;
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetLayer);
        if(rangeChecks.Length > 0){
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < (_angle * _angleMultiplier)/2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionLayer)){
                    Debug.Log("I saw my target");
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal){
        if(!isGlobal){
            angleInDegrees += transform.eulerAngles.y;
        }
            
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void DrawFieldOfView(){
        int stepCount = Mathf.CeilToInt(_angle * _angleMultiplier * _meshResolution);
        float stepAngleSize = (_angle * _angleMultiplier) / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for(int i = 0; i<= stepCount; i++){
            float angle = transform.eulerAngles.y - (_angle * _angleMultiplier)/2 + stepAngleSize * i;
            ViewCastInfo viewCastInfo = ViewCast(angle);
            viewPoints.Add(viewCastInfo.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount -2) * 3];

        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCount - 1; i++){
            vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    public float GetRadius(){
        return _radius;
    }

    public float GetAngle(){
        return _angle * _angleMultiplier;
    }

}
