using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColourBlindSettings : MonoBehaviour
{
    SpriteRenderer[] m_gameObjectsinScene;
    void Start()
    {


    }
    public void LoopThroughObjects(ColourBlindBase _obj)
    {
        string presetName = _obj.name;
        m_gameObjectsinScene = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer go in m_gameObjectsinScene)
        {
            if(go.gameObject.tag.Contains("DoColourChange"))
            PickPresetColour(go, _obj);
        }
    }
    public void PickPresetColour(SpriteRenderer _renderer, ColourBlindBase _obj)
    {
        float[] Colours = new float[3];
        Colours[0] = _renderer.color.r;
        Colours[1] = _renderer.color.g;
        Colours[2] = _renderer.color.b;
        if (Colours[0] == Colours.Max() || Colours[2] == Colours.Max())
        {
            _renderer.color = _obj.Colour;
        }
        if (Colours[1] == Colours.Max())
        {
            _renderer.color = _obj.Colour2;
        }
    }
}
