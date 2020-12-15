using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public enum ControlsMenuDirection
{
    None,
    Left,
    Right
}
public class ControlSettings : MonoBehaviour
{
    public RawImage VideoImage;
    public VideoPlayer VideoPlayer;
    public VideoClip[] VideoClips;
    public ControlsMenuDirection Direction;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TMP_Dropdown VideoDropdown;
    string[] Descriptions = new string[] { "Press Space to jump.", "Press W to grapple.", "Press A or D + Space while on a wall." };
    [SerializeField]
    int m_index;
    RebindControls m_controls;
    // Start is called before the first frame update
    void Start()
    {
        Direction = ControlsMenuDirection.None;
        m_index = 0;
        if (Title != null)
        Title.text = VideoPlayer.clip.name;
        Description.text = Descriptions[m_index];
    }
    private void OnEnable()
    {
        m_controls = GameObject.Find("Rebindable Holder").GetComponent<RebindControls>();
        Descriptions = new string[3];
        Descriptions[0] = "Press " + m_controls.Codes[3].ToString() + " to jump.";
        Descriptions[1] = "Press " + m_controls.Codes[0].ToString() + " to grapple.";
        Descriptions[2] = "Press " + m_controls.Codes[1].ToString() + " or " + m_controls.Codes[2].ToString() + " while on a wall.";
    }
    // Update is called once per frame
    void Update()
    {
        VideoPlayer.clip = VideoClips[VideoDropdown.value];
        Description.text = Descriptions[VideoDropdown.value];
    }
    public void SwitchVideo()
    {
        switch(Direction)
        {
            case ControlsMenuDirection.Left:
                m_index -= 1;
                if (m_index < 0)
                    m_index = VideoClips.Length-1;
                VideoPlayer.clip = VideoClips[m_index];
                Title.text = VideoPlayer.clip.name;
                Description.text = Descriptions[m_index];
                break;
            case ControlsMenuDirection.Right:
                m_index += 1;
                if (m_index >= VideoClips.Length)
                    m_index = 0;
                VideoPlayer.clip = VideoClips[m_index];
                Title.text = VideoPlayer.clip.name;
                Description.text = Descriptions[m_index];
                break;
            default:
                break;
        }
    }
    public void SetDirection(string _direction)
    {
      if(_direction == "Left")
        {
            Direction = ControlsMenuDirection.Left;
        }
        if (_direction == "Right")
        {
            Direction = ControlsMenuDirection.Right;
        }
    }
}
