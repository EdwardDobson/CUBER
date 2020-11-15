using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    TextMeshProUGUI m_livesText;
    TextMeshProUGUI m_scoreText;
    TextMeshProUGUI m_uiKeysText;
    bool m_shouldRespawn = false;
    Vector3 m_spawnLocation;
    ParticleSystem m_bloodEffect;
    ParticleSystem m_shieldEffect;
    int m_orangeKeyTotal;
    int m_purpleKeyTotal;
    int m_whiteKeyTotal;
    bool m_inDoor;
    // Start is called before the first frame update
    void Start()
    {
        m_currentScore = m_maxScore;
        m_currentHealth = m_maxHealth;
        m_ui = GameObject.Find("GameManager").transform.GetChild(0).gameObject;
        if(m_ui != null)
        {
            m_ui.transform.GetChild(0).GetComponent<Slider>().maxValue = m_maxHealth;
            m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_maxHealth;
            m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
            m_ui.transform.GetChild(1).GetComponent<Slider>().maxValue = m_maxShield;
            m_livesText = m_ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            m_scoreText = m_ui.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            m_uiKeysText = m_ui.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
            m_livesText.text = "Lives: " + m_lives;
        }
            m_gameOverScreen = GameObject.Find("GameManager").transform.GetChild(1).gameObject;
        m_spawnLocation = transform.position;
        m_bloodEffect = transform.GetChild(2).GetComponent<ParticleSystem>();
        m_shieldEffect = transform.GetChild(3).GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
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
     //   Debug.Log("Key Total: " + m_whiteKeyTotal);
        Die();
        if(m_ui != null)
        {
       
        }

        InDoorCheck();
        if (m_ui != null)
            m_scoreText.text = "Score: " + m_currentScore;
    }
    public void AddStats(ref int _valueToIncrease,  int _valueForIncrease, int _maxValue = 0, GameObject _pickup = null)
    {
        if (_valueToIncrease < _maxValue || _maxValue == 0)
        {
            _valueToIncrease += _valueForIncrease;
            if(_pickup)
            Destroy(_pickup);
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

                PlayBloodEffect();
            }
        }
        else
        {
            if (m_currentShield >= 0)
            {
                m_currentShield -= _value;
                m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
                PlayShieldEffect();

            }
        }
  
    }
    public void ResetScore()
    {
        m_currentScore = 0;
    }
    public void Die()
    {
        if (m_currentHealth <= 0 && m_lives > 0)
        {
            m_lives -= 1;
            if(m_ui != null)
            m_livesText.text = "Lives: " + m_lives;
            m_currentHealth = m_maxHealth;
            m_shouldRespawn = true;
            GetComponent<PlayerMovement>().RopeReset();
            m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
            m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
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
        switch(_type)
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
    
    void PlayBloodEffect()
    {
        m_bloodEffect.Play();
    } 
    void PlayShieldEffect()
    {
        m_shieldEffect.Play();
    }
    public int GetKeyTotal(PickupType _type)
    {
        int v = 0;
        switch (_type)
        {
            case PickupType.OrangeKey:
                v = m_orangeKeyTotal;
                break;
            case PickupType.WhiteKey:
                 v = m_whiteKeyTotal;
                Debug.Log("Key Total: " + v);
                break;
            case PickupType.PurpleKey:
                v =  m_purpleKeyTotal;
                break;
            default:
                break;
        }

        return v;
    }
    void InDoorCheck()
    {
        if (m_inDoor)
        {
            if (m_ui != null)
                m_uiKeysText.text = "Need more keys";
       //     Debug.Log("Door in");
        }
        else
        {
            if(m_ui != null)
            m_uiKeysText.text = "";
      //      Debug.Log("Door exit");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("DoorToOpen"))
        {
            m_inDoor = true;
            switch (collision.GetComponent<DoorOpen>().doorColour)
            {
                case DoorColour.Orange:
                    if (m_orangeKeyTotal > 0)
                    {
                        m_orangeKeyTotal -= 1;
                        collision.gameObject.SetActive(false);
                    }
                    break;
                case DoorColour.Purple:
                    if (m_purpleKeyTotal > 0)
                    {
                        m_purpleKeyTotal -= 1;
                        collision.gameObject.SetActive(false);
                    }

                    break;
                case DoorColour.White:

                    if (m_whiteKeyTotal > 0)
                    {
                        m_whiteKeyTotal -= 1;
                        collision.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("DoorToOpen"))
        {
            m_inDoor = false;
            if (m_ui != null)
                m_uiKeysText.text = "";
        
        }
    }
}
