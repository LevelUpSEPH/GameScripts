using UnityEngine;

[CreateAssetMenu(fileName = "Towers", menuName = "ScriptableObjects/Tower", order = 1)]
public class TowerSO : ScriptableObject
{
    public enum TowerType{
        SingleTarget,
        AOE,
        Sniper,
        CircleAOE
    }
    public TowerType towerType;
    public string towerName;
    public TowerSO nextLevelTower = null;
    public float range = 5f;
    public float shootingCD = 2;
    public float shotDamage = 10;    
    public int towerCost = 1;
}