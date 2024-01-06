using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class TowerInfo : ScriptableObject
{
    public TowerType towerType;

    public int cost;
    
    public float attackRange;
    public float attackSpeed;
    public float damage;

}
 
public enum TowerType { 
    SingleTarget,
    AOETower,
    DebuffTower
}