using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour, IColored
{
    [SerializeField] private Colors.Color _paintColor;
    [SerializeField] private List<MeshRenderer> _colorVisuals = new List<MeshRenderer>();

    private void OnTriggerEnter(Collider other){
        if(!other.CompareTag("Player"))
            return;
        PlayerController playerController = other.GetComponent<PlayerController>();
        PaintPlayer(playerController);
    }

    private void PaintPlayer(PlayerController playerController) {
        Colors.Color initialPlayerColor = playerController.GetColor();
        ToggleMaterialColor(initialPlayerColor);
        playerController.Paint(_paintColor);
        _paintColor = initialPlayerColor;
    }

    private void ToggleMaterialColor(Colors.Color targetColor) {
        foreach (MeshRenderer rend in _colorVisuals) {
            rend.material = MaterialHolder.instance.GetMaterialOfColor(targetColor);
        }
    }

    public Colors.Color GetColor(){
        return _paintColor;
    }
}
