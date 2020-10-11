
using UnityEngine;
[CreateAssetMenu(fileName = "Hazard", menuName = "Hazard", order = 1)]
public class BaseHazard : ScriptableObject
{
    public int Damage;
    public float MaxCoolDown;
    public float CoolDown;
    public int Uses;
    public int MaxUses;
    public bool isOverTimeAttack;
    public bool infiniteUses;
}
