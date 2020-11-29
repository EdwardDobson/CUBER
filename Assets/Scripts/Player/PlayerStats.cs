using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    int m_maxHealth = 5;
    [SerializeField]
    int m_currentHealth = 0;
    [SerializeField]
    int m_currentShield = 0;
    [SerializeField]
    int m_maxShield = 3;
    [SerializeField]
    int m_maxScore = 0;
    [SerializeField]
    int m_currentScore = 0;
    [SerializeField]
    int m_lives = 3;
    GameObject m_ui;
    GameObject m_gameOverScreen;
    GameObject m_objectivesScreen;
    TextMeshProUGUI m_livesText;
    TextMeshProUGUI m_scoreText;
    TextMeshProUGUI m_uiKeysText;
    Vector3 m_spawnLocation;
    ParticleManager m_particleManager;
    int m_orangeKeyTotal = 0;
    int m_purpleKeyTotal = 0;
    int m_whiteKeyTotal = 0;
    public  TextMeshProUGUI[] m_keyTexts =new TextMeshProUGUI[3];
    [SerializeField]
    bool m_bigStarCollected;
    [SerializeField]
    bool m_didntDie;
    [SerializeField]
    bool m_gainedScoreThreshold;
    [SerializeField]
    int m_totalPointsInScene;
    [SerializeField]
    int m_pointThreshold;
    GameObject[] m_scoreObjs;
    AudioSource m_source;
    PostProcessProfile m_volume;
    GameObject m_heartImage;
    void Start()
    {

        m_currentScore = m_maxScore;
        m_currentHealth = m_maxHealth;
        m_source = GetComponent<AudioSource>();
        m_volume = GameObject.Find("PostFx").GetComponent<PostProcessVolume>().profile;
        if (GameObject.Find("GameManager")!= null)
        {
            m_ui = GameObject.Find("GameManager").transform.GetChild(0).gameObject;
            m_objectivesScreen = GameObject.Find("GameManager").transform.GetChild(3).gameObject;
        }
        if (m_ui != null)
        {
            m_keyTexts = new TextMeshProUGUI[3];
            m_ui.transform.GetChild(0).GetComponent<Slider>().maxValue = m_maxHealth;
            m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_maxHealth;
            m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
            m_ui.transform.GetChild(1).GetComponent<Slider>().maxValue = m_maxShield;
            m_heartImage = m_ui.transform.GetChild(10).gameObject;
            m_livesText = m_ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            m_scoreText = m_ui.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            m_uiKeysText = m_ui.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
            m_livesText.text = "<sprite index= 0> x " + m_lives;
            m_keyTexts[0] = m_ui.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
            m_keyTexts[1] = m_ui.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
            m_keyTexts[2] = m_ui.transform.GetChild(8).GetComponent<TextMeshProUGUI>();

        }
        if (GameObject.Find("GameManager") != null)
            m_gameOverScreen = GameObject.Find("GameManager").transform.GetChild(1).gameObject;
        m_spawnLocation = transform.position;
        m_particleManager = GetComponent<ParticleManager>();
        m_volume.GetSetting<Vignette>().color.value = Color.black;
        Invoke("LateStart", 0.5f);

    }
    void LateStart()
    {
        m_scoreObjs = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject g in m_scoreObjs)
        {
            if(g.GetComponent<Pickup>() != null)
                m_totalPointsInScene += g.GetComponent<Pickup>().PickupObject.Value;
        }
        if(m_scoreObjs.Length > 0)
        m_pointThreshold = m_totalPointsInScene / m_scoreObjs.Length * 200;
        m_objectivesScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Each objective completed earns you a star!\n\n\n" +  "Gain " + m_pointThreshold + " score.\n\n" + "Collect the big star.\n\n" + "Complete the level without dying.";

    }
    void Update()
    {
        if (m_currentHealth > m_maxHealth)
            m_currentHealth = m_maxHealth;
        if (m_currentShield > m_maxShield)
            m_currentShield = m_maxShield;
        if (m_currentShield <= 0)
            m_currentShield = 0;
        if (m_currentScore <= 0)
            m_currentScore = 0;
        if (m_lives >= 3)
            m_lives = 3;
        Die();
        if (m_ui != null)
            m_scoreText.text = "Score:" + m_currentScore;
        m_keyTexts[0].text = "<sprite index= 0> x " + m_whiteKeyTotal;
        m_keyTexts[1].text = "<sprite index= 1> x " + m_purpleKeyTotal;
        m_keyTexts[2].text = "<sprite index= 2> x " + m_orangeKeyTotal;
        if (m_currentScore >= m_pointThreshold)
        {
            m_gainedScoreThreshold = true;
        }
        else m_gainedScoreThreshold = false;
        if(Input.GetKey(KeyCode.Tab) && SceneManager.GetActiveScene().buildIndex > 1)
        {
            m_objectivesScreen.SetActive(true);
        }
        else
        {
            m_objectivesScreen.SetActive(false);
        }
    }
    public void AddStats(ref int _valueToIncrease,  int _valueForIncrease, int _maxValue = 0, GameObject _pickup = null)
    {
        if (_valueToIncrease < _maxValue || _maxValue == 0)
        {
            _valueToIncrease += _valueForIncrease;
            if(_pickup)
            {
                PlayEffect(_pickup);

                m_source.clip =   _pickup.GetComponent<Pickup>().PickupObject.SoundClip;
                m_source.Play();
                Destroy(_pickup);
            }
        }
        m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
        m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
    }
    //Used for negative pickups
    public void ReduceStatValue(ref int _valueToDecrease, int _valueForDecrease, int _maxValue = 0, GameObject _pickup = null)
    {
        if (_valueToDecrease < _maxValue || _maxValue == 0)
        {
            _valueToDecrease -= _valueForDecrease;
            if (_pickup)
                Destroy(_pickup);
        }
        m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
        m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
    }
    public void TakeDamage(int _value)
    {
        if(m_currentShield <= 0)
        {
            if (m_currentHealth >= 0)
            {
                m_currentHealth -= _value;
                m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
                m_particleManager.PlayEffect(ParticleType.Blood);
                StartCoroutine(FlashDamage(Color.red));
            }
        }
        else
        {
            if (m_currentShield >= 0)
            {
                m_currentShield -= _value;
                m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
                m_particleManager.PlayEffect(ParticleType.Shield);
                StartCoroutine(FlashDamage(Color.blue));
            }
        }
    }
    IEnumerator FlashDamage(Color _colour)
    {
        m_volume.GetSetting<Vignette>().color.value = _colour;
        yield return new WaitForSeconds(0.5f);
        m_volume.GetSetting<Vignette>().color.value = Color.black;
    }
    public void ResetScore()
    {
        m_currentScore = 0;
    }
    public void Die()
    {
        if (m_currentHealth <= 0 && m_lives > 0)
        {
            transform.GetChild(0).GetComponent<AudioSource>().Play();
            m_lives -= 1;
            if(m_ui != null)
            m_livesText.text = "<sprite index= 0> x " + m_lives;
            m_currentHealth = m_maxHealth;
            GetComponent<PlayerMovement>().RopeReset();
            m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
            m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
            m_heartImage.SetActive(true);
       
            Respawn();
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a / 2);
        }
        else if(m_lives <= 0)
        {
            if(m_gameOverScreen != null)
            {
                m_gameOverScreen.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
    public void Respawn()
    {
            transform.position = m_spawnLocation;
    }
    public void SetSpawnPoint(Vector3 _location)
    {
        m_spawnLocation = _location;
    }
    public void IncreaseStats(PickupType _type,GameObject _pickup)
    {
        switch (_type)
        {
            case PickupType.Health:
                AddStats(ref m_currentHealth, _pickup.GetComponent<Pickup>().PickupObject.Value,  m_maxHealth, _pickup);
                break;
            case PickupType.Score:
                AddStats(ref m_currentScore, _pickup.GetComponent<Pickup>().PickupObject.Value, m_maxScore, _pickup);
                break;
            case PickupType.Shield:
                AddStats(ref m_currentShield, _pickup.GetComponent<Pickup>().PickupObject.Value, m_maxShield,_pickup);
                break;
            case PickupType.Lives:
                AddStats(ref m_lives, _pickup.GetComponent<Pickup>().PickupObject.Value, 3, _pickup);
                break;
            case PickupType.WhiteKey:
                AddStats(ref m_whiteKeyTotal, _pickup.GetComponent<Pickup>().PickupObject.Value, 0, _pickup);
                break;
            case PickupType.PurpleKey:
                AddStats(ref m_purpleKeyTotal, _pickup.GetComponent<Pickup>().PickupObject.Value, 0, _pickup);
                break;
            case PickupType.OrangeKey:
                AddStats(ref m_orangeKeyTotal, _pickup.GetComponent<Pickup>().PickupObject.Value, 0, _pickup);
                break;

        }
    }
    //A function used for calling applying negative pickup values
    public void ReduceStats(PickupType _type, GameObject _pickup = null)
    {
        switch (_type)
        {
            case PickupType.Health:
                ReduceStatValue(ref m_currentHealth, _pickup.GetComponent<Pickup>().PickupObject.Value,0, _pickup);
                break;
            case PickupType.Score:
                ReduceStatValue(ref m_currentScore,_pickup.GetComponent<Pickup>().PickupObject.Value,m_maxScore, _pickup);
                break;
            case PickupType.Shield:
                ReduceStatValue(ref m_currentShield, _pickup.GetComponent<Pickup>().PickupObject.Value, m_maxScore, _pickup);
                break;
            case PickupType.WhiteKey:
                ReduceStatValue(ref m_whiteKeyTotal, 1);
                break;
            case PickupType.PurpleKey:
                ReduceStatValue(ref m_purpleKeyTotal, 1);
                break;
            case PickupType.OrangeKey:
                ReduceStatValue(ref m_orangeKeyTotal, 1);
                break;
        }
    }
    
    public int GetWhiteKeyTotal()
    {
        return m_whiteKeyTotal;
    }
    public int GetPurpleKeyTotal()
    {
        return m_purpleKeyTotal;
    }
    public int GetOrangeKeyTotal()
    {
        return m_orangeKeyTotal;
    }
    //Used to play pickup and door effects
    void PlayEffect(GameObject _obj)
    {
        if(_obj.transform.childCount > 0)
        {
            GameObject newGameObject = Instantiate(_obj.transform.GetChild(0).gameObject, null);
            newGameObject.transform.position = _obj.transform.position;
            newGameObject.GetComponent<ParticleSystem>().Play();
            Destroy(newGameObject, newGameObject.GetComponent<ParticleSystem>().main.duration * 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("DoorToOpen"))
        {
            DoorOpen dOpen= collision.GetComponent<DoorOpen>();
            if(dOpen != null)
            {
                switch (dOpen.doorColour)
                {
                    case DoorColour.Orange:
                        OpenDoor(collision.gameObject, DoorColour.Orange,ref m_orangeKeyTotal);
                        break;
                    case DoorColour.Purple:
                        OpenDoor(collision.gameObject, DoorColour.Purple,ref  m_purpleKeyTotal);
                        break;
                    case DoorColour.White:
                        OpenDoor(collision.gameObject, DoorColour.White, ref m_whiteKeyTotal);
                        break;
                }
            }
        } 
        if(collision.tag.Contains("BigStar"))
        {
            m_bigStarCollected = true;
        }
    }
    void OpenDoor(GameObject _door,DoorColour _colour, ref int _keyTotal)
    {
        if (_keyTotal > 0)
        {
            m_source.clip = _door.GetComponent<DoorOpen>().DoorSound;
            m_source.Play();
            _keyTotal -= 1;
            PlayEffect(_door);
            Destroy(_door.gameObject);
        }
        else
        {
            if (m_ui != null)
            {
                switch (_colour)
                {
                    case DoorColour.White:
                        m_uiKeysText.text = "Need more" + "<sprite index= 0>" + " keys";
                        break;
                    case DoorColour.Purple:
                        m_uiKeysText.text = "Need more" + "<sprite index= 1>" + " keys";
                        break;
                    case DoorColour.Orange:
                        m_uiKeysText.text = "Need more" + "<sprite index= 2>" + " keys";
                        break;
                }
            }
        }
    }
    public int GetCurrentHealth()
    {
        return m_currentHealth;
    }
    public int GetMaxHealth()
    {
        return m_maxHealth;
    }
    public int GetCurrentShield()
    {
        return m_currentShield;
    }
    public int GetMaxShield()
    {
        return m_maxShield;
    }
    public int GetScore()
    {
        return m_currentScore;
    }
    public bool GetBigStarCollected()
    {
        return m_bigStarCollected;
    }
    public bool GetPointThresholdMet()
    {
        return m_gainedScoreThreshold;
    }
    public bool CheckIfDied()
    {
        if (m_lives >= 3)
        {
            m_didntDie = true;
        }
        else
            m_didntDie = false;
        return m_didntDie;
    }
    public int AddToScore(int _value)
    {
        return m_currentScore += _value;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DoorToOpen"))
        {
            if (m_ui != null)
                m_uiKeysText.text = "";
        }
    }

}
