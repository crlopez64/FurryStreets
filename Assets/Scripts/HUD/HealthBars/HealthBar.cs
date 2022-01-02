using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of showing visual Health.
/// </summary>
public class HealthBar : MonoBehaviour
{
    private MeterFill fill;
    private MeterDrain drain;

    private void Awake()
    {
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
    }
}
