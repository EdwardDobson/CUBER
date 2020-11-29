
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HeartFade : MonoBehaviour
{
    TextMeshProUGUI m_image;
    [SerializeField]
    float m_duration;
    void OnEnable()
    {
        m_image = GetComponent<TextMeshProUGUI>();
        StartCoroutine(fadeOut(m_duration));
    }
    IEnumerator fadeOut(float duration)
    {
        float counter = 0;
        Color backgroundColour = m_image.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            m_image.color = new Color(backgroundColour.r, backgroundColour.g, backgroundColour.b, alpha);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
