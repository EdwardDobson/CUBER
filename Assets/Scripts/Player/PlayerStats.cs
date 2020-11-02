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
    TextMeshProUGUI m_livesText;
    TextMeshProUGUI m_scoreText;
    bool m_shouldRespawn;
    Vector3 m_spawnLocation;
    ParticleSystem m_bloodEffect;
    ParticleSystem m_shieldEffect;
    // Start is called before the first frame update
    void Start()
    {
        m_currentScore = m_maxScore;
        m_currentHealth = m_maxHealth;
        m_ui = GameObject.Find("UI");
        m_ui.transform.GetChild(0).GetComponent<Slider>().maxValue = m_maxHealth;
        m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_maxHealth;
        m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;
        m_ui.transform.GetChild(1).GetComponent<Slider>().maxValue = m_maxShield;
        m_livesText = m_ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        m_scoreText = m_ui.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        m_livesText.text = "Lives: " + m_lives;
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
        Die();
        m_ui.transform.GetChild(0).GetComponent<Slider>().value = m_currentHealth;
        m_ui.transform.GetChild(1).GetComponent<Slider>().value = m_currentShield;

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
    }
    public void TakeDamage(int _value)
    {
        if(m_currentShield <= 0)
        {
            if (m_currentHealth >= 0)
            {
                m_currentHealth -= _value;
                PlayBloodEffect();
            }
        }
        else
        {
            if (m_currentShield >= 0)
            {
                m_currentShield -= _value;
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
            m_livesText.text = "Lives: " + m_lives;
            m_currentHealth = m_maxHealth;
            m_shouldRespawn = true;
            GetComponent<PlayerMovement>().RopeReset();
            //Respawn
            Respawn();
        }
        else if(m_lives <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Game Over");
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
        }
    }
    //A function used for calling applying negative pickup values
    public void ReduceStats(PickupType _type, GameObject _pickup)
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
}
