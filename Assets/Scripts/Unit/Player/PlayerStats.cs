using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Player's stats.
/// </summary>
public class PlayerStats : UnitStats
{

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
        meters.SetMeterBarCurrent(currentMeter);
        GetComponentInChildren<ParticlePooler>().SetParticlesSpecialMovesWolf();
    }
}
