using UnityEngine;
public enum PickupType
{
    Health,
    Score,
    Shield,
}
[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup", order = 1)]
public class BasePickup : ScriptableObject
{
    public int Value;
    public PickupType PickupType;
    public bool ShouldReduceValue;
}
