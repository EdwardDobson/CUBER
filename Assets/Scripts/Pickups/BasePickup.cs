using UnityEngine;
public enum PickupType
{
    Health,
    Score,
    Shield,
    Lives,
    OrangeKey,
    PurpleKey,
    WhiteKey
}
[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup", order = 1)]
public class BasePickup : ScriptableObject
{
    public int Value;
    public PickupType PickupType;
    public bool ShouldReduceValue;
    public AudioClip SoundClip;
}
