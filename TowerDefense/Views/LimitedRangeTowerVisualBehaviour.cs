using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedRangeTowerVisualBehaviour : TowerRangeVisualBehaviour
{
    [SerializeField] private GameObject _unavailableZone;
    [SerializeField] private LimitedRangeTower _limitedRangeTower;

    public override void ShowSphere(float radius)
    {
        base.ShowSphere(radius);
        ShowUnavailableZone();
    }

    public override void HideSphere()
    {
        base.HideSphere();
        HideUnavailableZone();
    }

    private void ShowUnavailableZone(){
        _unavailableZone.SetActive(true);
        _unavailableZone.transform.localScale = Vector3.one * _limitedRangeTower.GetUnavailableZoneRadius() * 2;
    }

    private void HideUnavailableZone(){
        _unavailableZone.SetActive(false);
    }
}
