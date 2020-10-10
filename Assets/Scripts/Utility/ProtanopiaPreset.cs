using UnityEngine;
[CreateAssetMenu(fileName = "Protanopia", menuName = "ColourBlindPresets/Protanopia", order = 1)]
public class ProtanopiaPreset : ColourBlindBase
{
    public ProtanopiaPreset()
        {
        Colour = new Vector4(0, 0, 255, 255);
        Colour2 = new Vector4(255, 255, 0, 255);
    }
}
