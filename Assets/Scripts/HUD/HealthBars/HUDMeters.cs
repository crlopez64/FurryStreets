using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of a Player's Health and Meter.
/// </summary>
public class HUDMeters : MonoBehaviour
{
    private MeterPortrait portrait;
    private MeterFrame meterFrame;
    private HealthBar healthBar;
    private MeterBar meterBar;

    private void Awake()
    {
        portrait = GetComponentInChildren<MeterPortrait>();
        meterFrame = GetComponentInChildren<MeterFrame>();
        healthBar = GetComponentInChildren<HealthBar>();
        meterBar = GetComponentInChildren<MeterBar>();
    }

    /// <summary>
    /// Adjust HUD based on the character chosen. Mateo(Wolf) = 0, Fox(Fox) = 1
    /// </summary>
    /// <param name="characterChosen"></param>
    public void AdjustHUD(byte characterChosen)
    {
        if (characterChosen > 4)
        {
            characterChosen = 4;
        }
        portrait.SetPortrait(characterChosen);
        meterFrame.SetMeterFrame(characterChosen);
        healthBar.SetMeterMask(characterChosen);
        meterBar.SetMeterMask(characterChosen);
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
    /// <summary>
    /// Set the meters if playing with Mateo (wolf).
    /// </summary>
    public void SetMetersWolf()
    {
        healthBar.SetMaxValue(100);
        meterBar.SetMaxValue(65);
    }
    /// <summary>
    /// Set the meters if palying with Fox (fox).
    /// </summary>
    public void SetMetersFox()
    {
        healthBar.SetMaxValue(80);
        meterBar.SetMaxValue(100);
    }
}
