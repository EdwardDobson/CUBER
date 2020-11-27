using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum DoorColour
{
    Orange,
    Purple,
    White
}
public class DoorOpen : MonoBehaviour
{
    public DoorColour doorColour;
    public AudioClip DoorSound;
}
