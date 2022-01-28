using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Unit's numerics.
/// </summary>
public class UnitStats : MonoBehaviour
{
    protected EnemyHUD enemyHUD;                //For Player, this will always be empty
    protected UnitAttack unitAttack;            //Used for Stun
    protected byte grabWorks;                   //For Enemy, checks which Grab actually works against an enemy.
    protected byte meterDelay;                  //Meter delay before actual recharge
    protected byte statMeterGain;               //Meter gain, stat * 0.5f;
    protected byte statGrabChance = 50;         //For Enemy, not to exceed 100. Chance on being grabbed
    protected float grabTimer = 2f;             //Grab timer; if not stunned, double grab timer.
    protected float meterDelayTimer;            //Timer before Meter can recharge.
    protected int maxMeter;                     //Meter to do Special Moves
    protected int maxHealth;                    //Health
    protected int currentStun;                  //If stun goes to max, get stunned.
    protected int maxStun = 20;                 //Stun
    protected int currentMeter;                 //Current Meter for Special moves
    protected int currentHealth;                //If health goes to 0, KO

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
    public virtual bool TakeDamage(Attack incomingAttack, UnitStats attackingUnit)
    {
        int totalDamage = 0;
        if (attackingUnit.GetComponent<PlayerStats>() != null)
        {
            //Include other stats
        }
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
        //If airborne, and knockback/popUp, add more stun
        float immediateStunMultiplier = 1.0f;
        if (!GetComponent<UnitMove>().Grounded())
        {
            if (incomingAttack.AttributeKnockback() || incomingAttack.AttributeKnockbackFar() ||
                incomingAttack.AttributePopUp())
            {
                immediateStunMultiplier += 0.3f;
            }
        }
        //If attack has heavy stun, add more stun
        if (incomingAttack.AttributeHeavyStun())
        {
            immediateStunMultiplier += 0.3f;
        }
        //If attack is last hit in its String proper
        if (GetComponent<EnemyStats>() != null)
        {
            currentStun += (int)(totalDamage * immediateStunMultiplier);
        }
        return currentStun >= maxStun;
    }
    /// <summary>
    /// Spend meter. Return true if Meter was successfully removed.
    /// </summary>
    /// <param name="meterBurn"></param>
    public virtual bool MeterBurn(int meterBurn)
    {
        if (currentMeter < meterBurn)
        {
            return false;
        }
        currentMeter -= meterBurn;
        return true;
    }
    /// <summary>
    /// Have the Unit restore Stamina and/or Meter.
    /// </summary>
    /// <param name="healthRestore"></param>
    /// <param name="meterRestore"></param>
    public virtual void RestoreUnit(int healthRestore, int meterRestore)
    {
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
        
    }
    /// <summary>
    /// Rest the Unit up and max out their health bars.
    /// </summary>
    public virtual void RestAll()
    {
        currentHealth = maxHealth;
        currentMeter = maxMeter;
        currentStun = 0;
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

    

    public void SetTest()
    {
        statMeterGain = 1;
        maxHealth = 300;
        maxStun = 100;
        currentHealth = maxHealth;
        currentMeter = 0;
    }
}
