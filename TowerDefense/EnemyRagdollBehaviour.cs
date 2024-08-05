using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdollBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _rigParent;

    [SerializeField] private List<Transform> _bones = new List<Transform>();
    private List<Vector3> _bonePositions = new List<Vector3>();
    private List<Quaternion> _boneRotations = new List<Quaternion>();

    private void Awake(){
        InitializeList();
    }

    private void OnEnable(){
        transform.position = transform.parent.position;
        ApplyForceToBones();
    }

    private void InitializeList(){
        GetAllBones(_rigParent);
    }

    private void GetAllBones(Transform parent){
        foreach(Transform child2 in parent){
            _bones.Add(child2);
            _bonePositions.Add(child2.transform.localPosition);
            _boneRotations.Add(child2.transform.localRotation);   
            if(child2.childCount > 0)
                GetAllBones(child2);
        }

    }

    private void ApplyForceToBones(){
        Vector3 randomDirection = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
        foreach(Transform bone in _bones){
            if(bone.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                rigidbody.AddExplosionForce(10f, transform.position + randomDirection, 2f, 5f, ForceMode.Impulse);
        }
    }

    private void ResetRagdoll(){
        for(int i = 0; i < _bones.Count; i++){
            _bones[i].transform.localPosition = _bonePositions[i];
            _bones[i].transform.localRotation = _boneRotations[i];
        }
    }
    
    private void OnDisable(){
        ResetRagdoll();
    }
}
