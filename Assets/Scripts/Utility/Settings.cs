using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Settings : MonoBehaviour
{
    public PostProcessProfile ColourVolume;
    public Toggle FullscreenToggle;
    public Slider[] AudioSliders = new Slider[3];
    public Slider[] AudioSlidersPaused = new Slider[3];
    public Slider ColourBlindIntensitySlider;
    public Slider ColourBlindIntensitySliderPaused;
    public AudioMixer Mixer;
    string[] m_audioNames = new string[3] { "Master", "Effects", "Music" };
    public PostProcessVolume ColourBlindSettings;
    public Texture[] Luts;
    private static Settings Instance;
    public TMP_Dropdown ColourBlindPresetsMain;
    public TMP_Dropdown ColourBlindPresetsPaused;
    public TMP_Dropdown ResolutionsMain;
    public TMP_Dropdown RefreshRatesMain;
    Resolution[] m_resolutions;
    List<float> m_rates = new List<float>();
    PauseSystem m_pauseSystem;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetAudioSettings();
        SetColourBlindIntensityLoad();
        m_pauseSystem = GameObject.Find("PauseMenuHolder").GetComponent<PauseSystem>();
        SetColourBlindPresets();
        ColourBlindLoad();
        //    GetResolutions();
        //GetRefreshRates();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            for (int i = 0; i < m_audioNames.Length; ++i)
            {
                AudioSlidersPaused[i].value = PlayerPrefs.GetFloat(m_audioNames[i]);
            }
            SetColourBlindIntensityLoad();
        }
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < m_audioNames.Length; ++i)
            {
                AudioSliders[i].value = PlayerPrefs.GetFloat(m_audioNames[i]);
            }
        }
   
    }
    void SetAudioSettings()
    {
        for (int i = 0; i < m_audioNames.Length; ++i)
        {
            if (AudioSliders[i] != null)
                AudioSliders[i].value = PlayerPrefs.GetFloat(m_audioNames[i]);
            if (AudioSlidersPaused[i] !=null)
            AudioSlidersPaused[i].value = PlayerPrefs.GetFloat(m_audioNames[i]);
            Mixer.SetFloat(m_audioNames[i], PlayerPrefs.GetFloat(m_audioNames[i]));
        }
    }
    void SetColourBlindPresets()
    {

        ColourBlindPresetsMain.options.Clear();
        ColourBlindPresetsMain.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
        foreach (Texture t in Luts)
        {
            ColourBlindPresetsMain.options.Add(new TMP_Dropdown.OptionData() { text = t.name });
        }
        ColourBlindPresetsPaused.options = ColourBlindPresetsMain.options;
    }
   public void SetColourBlindPreset()
    {
        if ( !m_pauseSystem.GetPauseState())
        {
            if (ColourBlindPresetsMain.value > 0 )
            {
                ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLut.value = Luts[ColourBlindPresetsMain.value - 1];
            }
            else
            {
                ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLut.value = null;
        
            }
            ColourBlindPresetsPaused.value = ColourBlindPresetsMain.value;
        }
        else if(m_pauseSystem.GetPauseState() )
        {
            if (ColourBlindPresetsPaused.value > 0)
            {
                ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLut.value = Luts[ColourBlindPresetsPaused.value - 1];
            }
            else
            {
                ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLut.value = null;
            }
            ColourBlindPresetsMain.value = ColourBlindPresetsPaused.value;
        }
   
        PlayerPrefs.SetInt("ColourBlindPresetValuePaused", ColourBlindPresetsPaused.value);
        PlayerPrefs.SetInt("ColourBlindPresetValue", ColourBlindPresetsMain.value);
    }
    void ColourBlindLoad()
    {
        ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLut.value = Luts[PlayerPrefs.GetInt("ColourBlindPresetValue")];
        ColourBlindPresetsMain.value = PlayerPrefs.GetInt("ColourBlindPresetValue");
        ColourBlindPresetsPaused.value = PlayerPrefs.GetInt("ColourBlindPresetValuePaused");
    }
    public void SetResolution()
    {
        Screen.SetResolution(m_resolutions[ ResolutionsMain.value].width, m_resolutions[ResolutionsMain.value].height,Screen.fullScreen, (int)m_rates[RefreshRatesMain.value]);
    }
    public void SetRefreshRate()
    {
        Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,Screen.fullScreen,(int)m_rates[RefreshRatesMain.value]);
    }
    void GetResolutions()
    {
        m_resolutions = Screen.resolutions;
        for (int i = 0; i < m_resolutions.Length; ++i)
        {
            if(m_resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            ResolutionsMain.options.Add(new TMP_Dropdown.OptionData() { text =  "" + m_resolutions[i].width + " X " + m_resolutions[i].height});
        }
    }
    void GetRefreshRates()
    {
        Resolution[] resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            if(!m_rates.Contains(resolutions[i].refreshRate))
                m_rates.Add(resolutions[i].refreshRate); 
        }
        m_rates.Sort();
        for (int i = 0; i < m_rates.Count; ++i)
        {
                RefreshRatesMain.options.Add(new TMP_Dropdown.OptionData() { text = m_rates[i].ToString() + " hz" });
        }
    }
    void SetColourBlindIntensityLoad()
    {
        ColourBlindIntensitySlider.value = PlayerPrefs.GetFloat("ColourBlindIntensity");
        ColourBlindIntensitySliderPaused.value = PlayerPrefs.GetFloat("ColourBlindIntensity");
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

    public void SetColourBlindIntensity(float _value)
    {
        PlayerPrefs.SetFloat("ColourBlindIntensity", _value);

        ColourBlindSettings.profile.GetSetting<ColorGrading>().ldrLutContribution.value = _value ;
    }
}
