using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMProFadeOut : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private float fadeInTime;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private bool isFading;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        isFading = false;
        StartCoroutine(FadeInAfterDelay());
    }

    private IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        isFading = true;
        float currentTime = 0f;
        Color originalColor = textMeshPro.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        while (currentTime < fadeInTime && isFading)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / fadeInTime;
            Color newColor = Color.Lerp(originalColor, targetColor, t);
            textMeshPro.color = newColor;

            yield return null;
        }
        isFading = false;
    }

    public void Restart()
    {
        if (isFading) return;
        Color originalColor = textMeshPro.color;
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        StartCoroutine(FadeInAfterDelay());
    }
}
