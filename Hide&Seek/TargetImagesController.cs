using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetImagesController : MonoBehaviour
{
    [SerializeField] private GameObject _imagePrefab;
    private List<TargetImageBehaviour> _targetImages = new List<TargetImageBehaviour>();

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InLevelController.InitializedTargetsList += OnTargetListInitialized;
        TargetController.TargetGotCaught += OnTargetGotCaught;
    }

    private void UnregisterEvents()
    {
        InLevelController.InitializedTargetsList -= OnTargetListInitialized;
        TargetController.TargetGotCaught -= OnTargetGotCaught;
    }

    private void OnTargetListInitialized(){
        DisableImages();
        for(int i = 0; i < InLevelController.instance.TargetCountLeftToCatch; i++)
        {
            GameObject image = Instantiate(_imagePrefab, transform.position, Quaternion.identity, transform);
            TargetImageBehaviour targetImage = image.GetComponent<TargetImageBehaviour>();
            _targetImages.Add(targetImage);
        }
    }

    private void DisableImages()
    {
        for(int i = 0; i < _targetImages.Count; i++)
        {
            if(_targetImages[i] != null)
                Destroy(_targetImages[i].gameObject);
        }
        _targetImages.Clear();
    }

    private void OnTargetGotCaught(){
        for(int i = _targetImages.Count - 1; i >= 0; i--)
        {
            if(_targetImages[i].GetIsCaught())
                continue;
            _targetImages[i].SwitchToCaught();
            return;
        }
        
    }
}
