using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of keeping track of meter fill.
/// </summary>
public class MeterFill : MonoBehaviour
{
    protected Color maxValueColor;
    protected Color minValueColor;
    private Image fill;
    private float maxStatValue;
    private float maxFillValue;

    private void Awake()
    {
        fill = GetComponent<Image>();
    }

    /// <summary>
    /// Set the max color.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetMaxColor(float r, float g, float b)
    {
        maxValueColor = new Color(r, g, b, 1f);
    }
    /// <summary>
    /// Set the min color.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetMinColor(float r, float g, float b)
    {
        minValueColor = new Color(r, g, b, 1f);
    }
    /// <summary>
    /// Set the max value for a stat.
    /// </summary>
    /// <param name="maxValue"></param>
    public void SetMaxValue(float maxValue)
    {
        maxStatValue = maxValue;
        maxFillValue = maxValue / 200;
        if (fill != null)
        {
            fill.fillAmount = maxFillValue;
        }
    }
    /// <summary>
    /// Set the whole Slider value relative to its max stat value.
    /// </summary>
    /// <param name="value"></param>
    public void SetSliderValue(float value)
    {
        if (value > maxStatValue)
        {
            value = maxStatValue;
        }
        float percentage = value / maxStatValue;
        fill.fillAmount = percentage * maxFillValue;
    }
    /// <summary>
    /// Get the percentage float.
    /// </summary>
    /// <returns></returns>
    public float GetValue()
    {
        return fill.fillAmount;
    }
    /// <summary>
    /// Get the fill.
    /// </summary>
    /// <returns></returns>
    public Image GetFill()
    {
        return fill;
    }
}
