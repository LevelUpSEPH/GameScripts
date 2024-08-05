using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SceneNavMeshManager : Singleton<SceneNavMeshManager>
{
    private void OnDisable(){
        ResetSceneNavMeshData();
    }

    public void LoadNewNavmesh(){
        ResetSceneNavMeshData();
        LoadNavMesh();
    }

    private void LoadNavMesh()
    {
        NavMeshData nextNavmeshData = GetNavMeshToLoad();
        NavMeshDataInstance loadedNavmesh = NavMesh.AddNavMeshData(nextNavmeshData);
        Debug.Log("Loaded navmesh data : " + loadedNavmesh + " data navMesh valid : " + loadedNavmesh.valid);
    }   

    private void ResetSceneNavMeshData()
    {
        NavMesh.RemoveAllNavMeshData();
    }

    private NavMeshData GetNavMeshToLoad()
    {
        var level = LevelController.instance.Levels[((PlayerData.Instance.PlayerLevel - 1) % LevelController.instance.Levels.Count)];
        NavMeshData navMeshData = Resources.Load<NavMeshData>($"NavMeshes/NavMeshLevel_{level}");

        return navMeshData;
    }
}
