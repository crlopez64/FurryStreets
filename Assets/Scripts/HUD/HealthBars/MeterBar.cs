using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of visual meter.
/// </summary>
public class MeterBar : MonoBehaviour
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
        fill.SetMaxColor(0f, 1f, 1f);
        fill.SetMinColor(0f, 0.6f, 1f);
        drain.SetColor(0.3f, 0.4f, 0.6f);
    }
    private void Update()
    {
        if (fill.GetValue() < drain.GetValue())
        {
            drain.Drain();
        }
    }

    /// <summary>
    /// Set the whole value for Meter.
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
