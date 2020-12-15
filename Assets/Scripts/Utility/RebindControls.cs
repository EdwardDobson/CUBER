using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RebindControls : MonoBehaviour
{
    public KeyCode[] Codes;
    public TMP_InputField[] Fields;
    public TMP_InputField[] FieldsPaused;
    public TextMeshProUGUI KeyInUseText;
    public TextMeshProUGUI KeyInUseTextPaused;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Fields.Length; ++i)
        {
            Fields[i].characterLimit = 1;
        }
        for (int i = 0; i < FieldsPaused.Length; ++i)
        {
            FieldsPaused[i].characterLimit = 1;
        }
        if(Fields.All(c => c.text == "" ))
        {
            ResetControls();
        }
        else
        LoadControls();
    }
    private void OnGUI()
    {
        for (int i = 0; i < Fields.Length; ++i)
        {
            if (Fields[i].isFocused)
            {
                Event e = Event.current;

                if (e.isKey && e.type == EventType.KeyUp)
                {
                    if(Codes.All(c => c != e.keyCode))
                    {
                        if (!Fields[i].text.Contains(e.keyCode.ToString()))
                        {
                            Fields[i].text = e.keyCode.ToString();
                            Codes[i] = e.keyCode;
                        }
                    }
                    else if(Codes[i] != e.keyCode)
                    {
                        StartCoroutine(HideText());
                        Fields[i].text = Codes[i].ToString();
                    }
                }
            }
        }
        for (int i = 0; i < Fields.Length; ++i)
        {
            if (FieldsPaused[i].isFocused)
            {
                Event e = Event.current;

                if (e.isKey && e.type == EventType.KeyUp)
                {
                    if (Codes.All(c => c != e.keyCode))
                    {
                        if (!FieldsPaused[i].text.Contains(e.keyCode.ToString()))
                        {
                            FieldsPaused[i].text = e.keyCode.ToString();
                            Codes[i] = e.keyCode;
                        }
                    }
                    else if (Codes[i] != e.keyCode)
                    {
                        StartCoroutine(HideText());
                        FieldsPaused[i].text = Codes[i].ToString();
                    }
                }
            }
        }
    }
    IEnumerator HideText()
    {
        KeyInUseText.text = "Key in use";
        KeyInUseTextPaused.text = "Key in use";
        yield return new WaitForSeconds(1);
        KeyInUseText.text = "";
        KeyInUseTextPaused.text = "";
    }
    private void OnDisable()
    {
        KeyInUseText.text = "";
        KeyInUseTextPaused.text = "";
    }
    private void OnEnable()
    {
        LoadControls();
    }
    public void ResetControls()
    {
        Codes[0] = KeyCode.W;
        Codes[1] = KeyCode.A;
        Codes[2] = KeyCode.D;
        Codes[3] = KeyCode.Space;
        Codes[4] = KeyCode.Tab;
        Codes[5] = KeyCode.E;

        Fields[0].text = Codes[0].ToString();
        Fields[1].text = Codes[1].ToString();
        Fields[2].text = Codes[2].ToString();
        Fields[3].text = Codes[3].ToString();
        Fields[4].text = Codes[4].ToString();
        Fields[5].text = Codes[5].ToString();

        FieldsPaused[0].text = Codes[0].ToString();
        FieldsPaused[1].text = Codes[1].ToString();
        FieldsPaused[2].text = Codes[2].ToString();
        FieldsPaused[3].text = Codes[3].ToString();
        FieldsPaused[4].text = Codes[4].ToString();
        FieldsPaused[5].text = Codes[5].ToString();
    }
    void LoadControls()
    {
        Codes[0] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Grapple"));
        Codes[1] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left"));
        Codes[2] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right"));
        Codes[3] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump"));
        Codes[4] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Tab"));
        Codes[5] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact"));

        Fields[0].text = Codes[0].ToString();
        Fields[1].text = Codes[1].ToString();
        Fields[2].text = Codes[2].ToString();
        Fields[3].text = Codes[3].ToString();
        Fields[4].text = Codes[4].ToString();
        Fields[5].text = Codes[5].ToString();

        FieldsPaused[0].text = Codes[0].ToString();
        FieldsPaused[1].text = Codes[1].ToString();
        FieldsPaused[2].text = Codes[2].ToString();
        FieldsPaused[3].text = Codes[3].ToString();
        FieldsPaused[4].text = Codes[4].ToString();
        FieldsPaused[5].text = Codes[5].ToString();
    }
    public void SaveControl()
    {
        PlayerPrefs.SetString("Grapple", Codes[0].ToString());
        PlayerPrefs.SetString("Left", Codes[1].ToString());
        PlayerPrefs.SetString("Right", Codes[2].ToString());
        PlayerPrefs.SetString("Jump", Codes[3].ToString());
        PlayerPrefs.SetString("Tab", Codes[4].ToString());
        PlayerPrefs.SetString("Interact", Codes[5].ToString());
    }
}
