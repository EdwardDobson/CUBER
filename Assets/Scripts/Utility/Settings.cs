using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider[] ColourSliders = new Slider[3];
    public PostProcessProfile ColourVolume;
    public Toggle FullscreenToggle;
    public Slider[] AudioSliders = new Slider[3];
    public AudioMixer Mixer;
    string[] m_audioNames = new string[3] { "Master", "Effects", "Music" };
    string[] m_colourNames = new string[3] { "Red", "Green", "Blue" };
    
    void Start()
    {
        SetAudioSettings();
        SetColourLevels();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetAudioSettings()
    {
        for(int i = 0; i  < m_audioNames.Length; ++i)
        {
            AudioSliders[i].value = PlayerPrefs.GetFloat(m_audioNames[i]);
            Mixer.SetFloat(m_audioNames[i], PlayerPrefs.GetFloat(m_audioNames[i]));
        }
    }
    void SetColourLevels()
    {
 
        ColourVolume.GetSetting<ColorGrading>().mixerRedOutRedIn.value = PlayerPrefs.GetFloat("Red");
        ColourVolume.GetSetting<ColorGrading>().mixerGreenOutGreenIn.value = PlayerPrefs.GetFloat("Green");
        ColourVolume.GetSetting<ColorGrading>().mixerBlueOutBlueIn.value = PlayerPrefs.GetFloat("Blue");
        for (int i = 0; i < m_colourNames.Length; ++i)
        {
            ColourSliders[i].value = PlayerPrefs.GetFloat(m_colourNames[i]);
        }
    }
    public void ColourLevels()
    {
        if(GameObject.Find("Graphics") != null)
        {
            ColourVolume.GetSetting<ColorGrading>().mixerRedOutRedIn.value = ColourSliders[0].value;
            ColourVolume.GetSetting<ColorGrading>().mixerGreenOutGreenIn.value = ColourSliders[1].value;
            ColourVolume.GetSetting<ColorGrading>().mixerBlueOutBlueIn.value = ColourSliders[2].value;
            for(int i = 0; i  < m_colourNames.Length; ++i)
            {
                PlayerPrefs.SetFloat(m_colourNames[i], ColourSliders[i].value);
            }
        }
    
    }
    public void Fullscreen()
    {
        if(FullscreenToggle.isOn)
        Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }
    public void MasterVolume(float _value)
    {
        Mixer.SetFloat("Master", _value);
        PlayerPrefs.SetFloat("Master", _value);
    }
    public void EffectsVolume(float _value)
    {
        Mixer.SetFloat("Effects", _value);
        PlayerPrefs.SetFloat("Effects", _value);
    }
    public void MusicVolume(float _value)
    {
        Mixer.SetFloat("Music", _value);
        PlayerPrefs.SetFloat("Music", _value);
    }
}
