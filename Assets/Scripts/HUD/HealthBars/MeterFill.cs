using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of keeping track of meter fill.
/// </summary>
public class MeterFill : MonoBehaviour
{
    protected Color currentTrueColor;
    protected Color currentLerpColor;
    protected Color maxValueColor;
    protected Color minValueColor;
    private Image fill;
    private float percentage;
    private float maxStatValue;
    private float maxFillValue;
    private float meterBurnTimer;

    private void Awake()
    {
        fill = GetComponent<Image>();
    }
    private void Update()
    {
        currentLerpColor = Color.Lerp(minValueColor, maxValueColor, percentage);
        if (meterBurnTimer >= 0)
        {
            meterBurnTimer -= Time.deltaTime * 1.25f;
        }
        currentTrueColor = Color.Lerp(currentLerpColor, Color.white, meterBurnTimer);
        fill.color = currentTrueColor;
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
        maxFillValue = maxValue / 2000;
        if (fill != null)
        {
            fill.fillAmount = maxFillValue;
        }
    }
    /// <summary>
    /// Set the whole Slider value relative to its max stat value.
    /// </summary>
    /// <param name="value"></param>
    public void SetSliderValue(float value, bool meterBurned)
    {
        if (value > maxStatValue)
        {
            value = maxStatValue;
        }
        percentage = value / maxStatValue;
        fill.fillAmount = percentage * maxFillValue;
        if (meterBurned)
        {
            meterBurnTimer = 1f;
        }
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
