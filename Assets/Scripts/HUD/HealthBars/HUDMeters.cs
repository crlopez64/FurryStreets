using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of a Player's Health and Meter.
/// </summary>
public class HUDMeters : MonoBehaviour
{
    private MeterPortraitMask portrait;
    private MeterFrame meterFrame;
    private HealthBar healthBar;
    private MeterBar meterBar;
    private MeterBackground healthBarBackground;
    private MeterBackground meterBarBackground;
    private HUDSpecialGraphic specialGraphic;

    private void Awake()
    {
        portrait = GetComponentInChildren<MeterPortraitMask>();
        meterFrame = GetComponentInChildren<MeterFrame>();
        healthBar = GetComponentInChildren<HealthBar>();
        meterBar = GetComponentInChildren<MeterBar>();
        healthBarBackground = healthBar.GetComponent<MeterBackground>();
        meterBarBackground = meterBar.GetComponent<MeterBackground>();
        specialGraphic = GetComponentInChildren<HUDSpecialGraphic>();
    }
    private void Start()
    {
        specialGraphic.TurnOffGraphic();
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
    public void SetMeterBarCurrent(float currentValue, bool meterBurned)
    {
        meterBar.SetValue(currentValue, meterBurned);
        if (meterBurned)
        {
            specialGraphic.MeterBurn();
            specialGraphic.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Light up the Health bar for Danger reasons.
    /// </summary>
    public void LightUpHealth()
    {
        healthBarBackground.LightUp();
    }
    /// <summary>
    /// Light up the Meter bar to show there's not enough meter.
    /// </summary>
    public void LightUpMeter()
    {
        meterBarBackground.LightUp();
    }
    /// <summary>
    /// Set the meters if playing with Mateo (wolf).
    /// </summary>
    public void SetMetersWolf()
    {
        healthBar.SetMaxValue(1000);
        meterBar.SetMaxValue(650);
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
