using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Unit's numerics.
/// </summary>
public class UnitStats : MonoBehaviour
{
    protected EnemyHUD enemyHUD;        //For Player, this will always be empty
    protected UnitAttack unitAttack;    //Used for Stun
    protected byte grabWorks;           //For Enemy, checks which Grab actually works against an enemy.
    protected byte meterDelay;          //Meter delay before actual recharge
    protected byte statAttack;          //Physical Attack
    protected byte statDefense;         //Physical Defense
    protected byte statMeterGain;       //Meter gain, stat * 0.5f;
    protected byte statGrabChance = 50; //For Enemy, not to exceed 100. Chance on being grabbed
    protected int maxStun = 20;         //Stun
    protected float grabTimer = 2f;     //Grab timer; if not stunned, double grab timer.
    protected float meterDelayTimer;    //Timer before Meter can recharge.
    protected int maxHealth;            //Health
    protected int maxMeter;             //Meter to do Special Moves
    protected int currentHealth;        //If health goes to 0, KO
    protected int currentStun;          //If stun goes to max, get stunned.
    protected int currentMeter;         //Current Meter for Special moves

    public HUDMeters meters;

    protected virtual void Awake()
    {
        enemyHUD = FindObjectOfType<EnemyHUD>();
        unitAttack = GetComponent<UnitAttack>();
    }
    protected virtual void Start()
    {
        meterDelay = 3;
    }
    protected virtual void Update()
    {
        if (currentMeter < maxMeter)
        {
            if (meterDelayTimer > 0f)
            {
                meterDelayTimer -= Time.deltaTime;
            }
            else
            {
                RestoreUnit(0, statMeterGain);
                meterDelayTimer = meterDelay;
            }
        }
        
    }
    /// <summary>
    /// Have the Unit take Damage. Accounts for all Damage.
    /// Returns true if Current Stun equals Max Stun (unit is now stunned).
    /// </summary>
    /// <param name="incomingAttack"></param>
    /// <param name="attackingUnit"></param>
    /// <returns></returns>
    public bool TakeDamage(Attack incomingAttack, UnitStats attackingUnit)
    {
        int totalDamage = 0;
        totalDamage = incomingAttack.Damage();
        currentHealth -= totalDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.LogWarning("Zero health! KO!!");
        }
        currentMeter += 3;
        if (currentMeter >= maxMeter)
        {
            currentMeter = maxMeter;
        }
        currentStun += totalDamage;
        if (meters != null)
        {
            meters.SetHealthBarCurrent(currentHealth);
        }
        return currentStun >= maxStun;
    }
    /// <summary>
    /// Spend meter.
    /// </summary>
    /// <param name="meterBurn"></param>
    public bool MeterBurn(int meterBurn)
    {
        if (currentMeter < meterBurn)
        {
            return false;
        }
        currentMeter -= meterBurn;
        if (meters != null)
        {
            meters.SetMeterBarCurrent(currentMeter);
        }
        return true;
    }
    /// <summary>
    /// Have the Unit restore Stamina and/or Meter.
    /// </summary>
    /// <param name="healthRestore"></param>
    /// <param name="meterRestore"></param>
    public void RestoreUnit(int healthRestore, int meterRestore)
    {
        Debug.Log("Current Meter: " + currentMeter);
        currentHealth += healthRestore;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        currentMeter += meterRestore;
        if (currentMeter >= maxMeter)
        {
            currentMeter = maxMeter;
        }
        if (meters != null)
        {
            meters.SetHealthBarCurrent(currentHealth);
            meters.SetMeterBarCurrent(currentMeter);
        }
    }
    /// <summary>
    /// Rest the Unit up and max out their health bars.
    /// </summary>
    public void RestAll()
    {
        currentHealth = maxHealth;
        currentMeter = maxMeter;
        currentStun = 0;
        if (meters != null)
        {
            meters.SetHealthBarCurrent(currentHealth);
            meters.SetMeterBarCurrent(currentMeter);
        }
    }
    /// <summary>
    /// Reset this Unit's stun.
    /// </summary>
    public void ResetStun()
    {
        currentStun = 0;
    }
    /// <summary>
    /// Return if this Unit's is stunned or not.
    /// </summary>
    public bool Stunned()
    {
        return currentStun >= maxStun;
    }
    /// <summary>
    /// Has the Unit's stamina depleted?
    /// </summary>
    /// <returns></returns>
    public bool StaminaEmpty()
    {
        return currentHealth <= 0;
    }
    /// <summary>
    /// Has the Unit reached their Meter limit?
    /// </summary>
    /// <returns></returns>
    public bool MeterMaxedOut()
    {
        return currentMeter >= maxMeter;
    }
    /// <summary>
    /// Can this Unit be grabbed?
    /// </summary>
    /// <returns></returns>
    public bool CanGrab()
    {
        if (statGrabChance >= 100)
        {
            return true;
        }
        return Random.Range(0, 100) <= statGrabChance;
    }
    /// <summary>
    /// Will a certain grab work against this Unit? If against Player, always return true.
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
    public bool CanGrabWork(int attackType)
    {
        if (GetComponent<PlayerStats>())
        {
            return true;
        }
        return (((0x1 << (attackType - 1)) & grabWorks) >> (attackType - 1)) == 0x1;
    }
    public int CurrentHealth()
    {
        return currentHealth;
    }
    public int CurrentMeter()
    {
        return currentMeter;
    }
    public int MaxHealth()
    {
        return maxHealth;
    }
    public int MaxMeter()
    {
        return maxMeter;
    }
    public float GetGrabTimer()
    {
        return grabTimer * (unitAttack.Stunned() ? 2 : 1);
    }

    /// <summary>
    /// Get HUD for some player.
    /// </summary>
    /// <param name="whichPlayer"></param>
    protected void GetMeter(byte whichPlayer)
    {
        meters = FindObjectOfType<HUDMetersGrid>().GetMeter(whichPlayer);
    }

    public void SetTest()
    {
        statAttack = 5;
        statDefense = 2;
        statMeterGain = 1;
        maxHealth = 100;
        currentHealth = maxHealth;
        currentMeter = 0;
    }
}
