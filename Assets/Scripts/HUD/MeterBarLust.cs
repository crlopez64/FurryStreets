using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of a Unit's Lust on the HUD. Drain appears when Lust is increasing.
/// </summary>
public class MeterBarLust : MeterBar
{
    private void Awake()
    {
        maxValueColor = new Color(1f, 0.48f, 0.48f, 1f);
        minValueColor = new Color(0.8f, 0f, 0.16f, 1f);
    }
    private void Update()
    {
        if (fillDrain != null)
        {
            if (fillDrain.fillAmount > fillActual.fillAmount)
            {
                fillDrain.fillAmount -= Time.deltaTime * 0.3f;
            }
        }
        fillActual.color = Color.Lerp(minValueColor, maxValueColor, fillActual.fillAmount / maxFillValue);
    }

    /// <summary>
    /// Set the Drain Slider value. Actual Slider will follow suit.
    /// </summary>
    /// <param name="value"></param>
    public override void SetSliderValue(float value)
    {
        if (value > maxStatValue)
        {
            value = maxStatValue;
        }
        float percentage = value / maxStatValue;
        fillDrain.fillAmount = percentage * maxFillValue;

        //If Drain is greater than Actual, set Drain to Actual
        if (fillActual.fillAmount > fillDrain.fillAmount)
        {
            fillActual.fillAmount = fillDrain.fillAmount;
        }
        if (currentValueText != null)
        {
            currentValueText.text = value.ToString();
        }
    }
}
