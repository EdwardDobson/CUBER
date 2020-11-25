
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSplashScreen : MonoBehaviour
{
    Image m_background;
    [SerializeField]
    float m_duration;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().SetStart(false);
        GameObject.Find("PauseMenuHolder").GetComponent<PauseSystem>().CanPause = false;
        m_background = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        StartCoroutine(fadeOut(m_duration));
    }
    IEnumerator fadeOut( float duration)
    {
        float counter = 0;
        //Get current color
        Color backgroundColour = m_background.material.color;
  
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            m_background.color = new Color(backgroundColour.r, backgroundColour.g, backgroundColour.b, alpha);
            yield return null;
        }
        GameObject.Find("Player").GetComponent<PlayerMovement>().SetStart(true);
        GameObject.Find("PauseMenuHolder").GetComponent<PauseSystem>().CanPause = true;
        gameObject.SetActive(false);
    }
}
