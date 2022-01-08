using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Player's stats.
/// </summary>
public class PlayerStats : UnitStats
{
    private int highestCombo;
    private int comboHits;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Get Player One.");
        GetMeter(1);
        SetTestWolf();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Add one hit to this Player's Combo.
    /// </summary>
    public void AddToCombo()
    {
        comboHits++;
        if (comboHits > highestCombo)
        {
            highestCombo = comboHits;
        }
    }
    /// <summary>
    /// Reset the Combo is taking too long or taken damage.
    /// </summary>
    public void ResetCombo()
    {
        comboHits = 0;
    }

    public void SetTestWolf()
    {
        meters.SetMetersWolf();
        statAttack = 5;
        statDefense = 2;
        statMeterGain = 2;
        maxHealth = 100;
        maxMeter = 65;
        currentHealth = maxHealth;
        currentMeter = maxMeter;
        meters.SetHealthBarCurrent(currentHealth);
        meters.SetMeterBarCurrent(currentMeter, false);
        GetComponentInChildren<ParticlePooler>().SetParticlesSpecialMovesWolf();
    }
}
