using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of showing visual Health.
/// </summary>
public class HealthBar : MonoBehaviour
{
    private Image meterMask;
    private MeterFill fill;
    private MeterDrain drain;

    private void Awake()
    {
        meterMask = GetComponent<Image>();
        fill = GetComponentInChildren<MeterFill>();
        drain = GetComponentInChildren<MeterDrain>();
    }
    private void Start()
    {
        fill.SetMaxColor(1f, 0f, 0f);
        fill.SetMinColor(0.4f, 0f, 0f);
        drain.SetColor(1f, 1f, 1f);
    }
    private void Update()
    {
        if (fill.GetValue() < drain.GetValue())
        {
            drain.Drain();
        }
    }

    /// <summary>
    /// Adjust the health mask based on the character chosen. Mateo(Wolf) = 0, Fox(Fox) = 1
    /// </summary>
    /// <param name="characterChosen"></param>
    public void SetMeterMask(byte characterChosen)
    {
        switch (characterChosen)
        {
            case 0:
                meterMask.sprite = Resources.Load<Sprite>("HUD/HealthBars/HealthMeterWolf/HealthMeterFillWolf0");
                break;
            case 1:
                meterMask.sprite = Resources.Load<Sprite>("HUD/HealthBars/HealthMeterFox/HealthMeterFillFox0");
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Set the whole value for Health.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        fill.SetSliderValue(value);
        if (fill.GetValue() > drain.GetValue())
        {
            drain.SetValue(value);
        }
    }
    /// <summary>
    /// Set the max health value.
    /// </summary>
    /// <param name="maxValue"></param>
    public void SetMaxValue(float maxValue)
    {
        fill.SetMaxValue(maxValue);
        drain.SetMaxValue(maxValue);
    }
}
