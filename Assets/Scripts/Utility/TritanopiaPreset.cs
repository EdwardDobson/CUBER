
using UnityEngine;
[CreateAssetMenu(fileName = "Tritanopia", menuName = "ColourBlindPresets/Tritanopia", order = 1)]
public class TritanopiaPreset : ColourBlindBase
{
    public TritanopiaPreset()
    {
        Colour = new Vector4(255, 0, 0, 255);
        Colour2 = new Vector4(0, 255, 0, 255);
    }
}
