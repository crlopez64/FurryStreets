using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of a Player's Health and Meter.
/// </summary>
public class HUDMeters : MonoBehaviour
{
    private HealthBar healthBar;
    private MeterBar meterBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        meterBar = GetComponentInChildren<MeterBar>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SetHealthBarCurrent(50);
            SetMeterBarCurrent(30);
        }
    }

    /// <summary>
    /// Set the Health bar.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    public void SetHealthBar(float maxValue, float currentValue)
    {
        healthBar.SetMaxValue(maxValue);
        healthBar.SetValue(currentValue);
    }
    /// <summary>
    /// Set the Meter bar.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    public void SetMeterBar(float maxValue, float currentValue)
    {
        meterBar.SetMaxValue(maxValue);
        meterBar.SetValue(currentValue);
    }
    /// <summary>
    /// Set the health bar visually.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetHealthBarCurrent(float currentValue)
    {
        healthBar.SetValue(currentValue);
    }
    /// <summary>
    /// Set the meter bar visually.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetMeterBarCurrent(float currentValue)
    {
        meterBar.SetValue(currentValue);
    }
}
