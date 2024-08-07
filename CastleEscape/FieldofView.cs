using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FieldofView : MonoBehaviour
{

    [SerializeField] private float _radius;
    
    [Range(0,360)]
    [SerializeField] private float _angle;


    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstructionLayer;

    private GameObject _playerRef;

    private bool _sawPlayer = false;

    [SerializeField] private float _meshResolution = 1;

    [SerializeField] private MeshFilter _meshFilter;
    private Mesh _viewMesh;

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
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void Start(){
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        _meshFilter.mesh = _viewMesh;
    }

    private void LateUpdate(){
        DrawFieldOfView();
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
        float delay = 0.2f;
        while(true){
            yield return new WaitForSeconds(delay);
            FieldofViewCheck();
        }
    }

    private void FieldofViewCheck(){
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _playerLayer);
        if(rangeChecks.Length > 0){
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < _angle/2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionLayer)){
                    if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, 128)){
                        PlayerController playerController = _playerRef.GetComponent<PlayerController>();
                        if(!playerController.GetIsPlayerHiding())
                        _sawPlayer = true;
                    }
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
        int stepCount = Mathf.CeilToInt(_angle * _meshResolution);
        float stepAngleSize = _angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for(int i = 0; i<= stepCount; i++){
            float angle = transform.eulerAngles.y - _angle/2 + stepAngleSize * i;
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
        return _angle;
    }

    public bool GetSawPlayer(){
        return _sawPlayer;
    }

    public GameObject GetPlayerRef(){
        return _playerRef;
    }
}
