
using UnityEngine;
[CreateAssetMenu(fileName = "Deuteranopia", menuName = "ColourBlindPresets/Deuteranopia", order = 1)]
public class DeuteranopiaPreset : ColourBlindBase
{
    public DeuteranopiaPreset()
    {
        Colour = new Vector4(0, 0, 255, 255);
        Colour2 = new Vector4(255, 255, 0, 255);
    }
}
