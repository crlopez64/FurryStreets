using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of the background color of the health bar.
/// </summary>
public class MeterBackground : MonoBehaviour
{
    private Image background;
    private Color standardColor;
    private Color dangerColor;
    private float lightUpTimer;

    private void Awake()
    {
        background = GetComponent<Image>();
        standardColor = new Color(0.2f, 0.2f, 0.2f);
        dangerColor = new Color(0.8f, 0.2f, 0.2f);
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (lightUpTimer >= 0f)
        {
            lightUpTimer -= Time.deltaTime * 1.2f;
        }
        background.color = Color.Lerp(standardColor, dangerColor, lightUpTimer);
    }

    /// <summary>
    /// Light up the background.
    /// </summary>
    public void LightUp()
    {
        lightUpTimer = 1f;
    }
}
