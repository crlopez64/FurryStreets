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
    protected byte statAttack;          //Physical Attack
    protected byte statGrabChance = 50; //For Enemy, not to exceed 100. Chance on being grabbed
    protected byte grabWorks;           //For Enemy, checks which Normal Grab actually works against an enemy.
    protected int maxStun = 20;         //Stun
    protected float grabTimer = 2f;     //Grab timer; if not stunned, double grab timer.
    public int maxHealth;              //Health
    public int maxMeter;               //Meter to do Special Moves
    protected int currentHealth;       //If stamina goes to 0, KO
    protected int currentStun;          //If stun goes to max, get stunned.
    protected int currentMeter;         //Current Meter for Special moves

    public MeterBarStamina staminaBar;
    public MeterBarLust meterBar;

    protected virtual void Awake()
    {
        enemyHUD = FindObjectOfType<EnemyHUD>();
        unitAttack = GetComponent<UnitAttack>();
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
        int damagePhysical = 0;

        currentHealth -= damagePhysical;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.LogWarning("Zero health! KO!!");
        }
        //currentMeter += damageSpecial;
        if (currentMeter >= maxMeter)
        {
            currentMeter = maxMeter;
            Debug.LogWarning("Max Meter! Heavy Stun!!");
        }
        currentStun += damagePhysical;
        if (enemyHUD != null)
        {
            enemyHUD.TurnOnHUD(this);
        }
        //if (staminaBar != null)
        //{
        //    staminaBar.SetSliderValue(currentStamina);
        //}
        if (meterBar != null)
        {
            meterBar.SetSliderValue(currentMeter);
        }
        return currentStun >= maxStun;
    }
    /// <summary>
    /// Have the Unit restore Stamina and/or Sexual store.
    /// </summary>
    /// <param name="healthRestore"></param>
    /// <param name="specialRestore"></param>
    public void RestoreUnit(int healthRestore, int specialRestore)
    {
        currentHealth += healthRestore;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        currentMeter -= specialRestore;
        if (currentMeter <= 0)
        {
            currentMeter = 0;
        }
        if (enemyHUD != null)
        {
            enemyHUD.TurnOnHUD(this);
        }
        if (staminaBar != null)
        {
            staminaBar.SetSliderValue(currentHealth);
        }
        if (meterBar != null)
        {
            meterBar.SetSliderValue(currentMeter);
        }
    }
    /// <summary>
    /// Rest the Unit up and max out their health bars.
    /// </summary>
    public void RestAll()
    {
        currentHealth = maxHealth;
        currentMeter = 0;
        currentStun = 0;
        if (staminaBar != null)
        {
            staminaBar.SetSliderValue(currentHealth);
        }
        if (meterBar != null)
        {
            meterBar.SetSliderValueNoDrain(currentMeter);
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
    /// Return the Meter level of the player. Will influence how Unit walks and idles.
    /// </summary>
    /// <returns></returns>
    public int MeterLevel()
    {
        int quartile = maxMeter / 4;
        if (currentMeter < quartile)
        {
            return 1;
        }
        else if ((currentMeter >= quartile) && (currentMeter < (quartile * 2)))
        {
            return 2;
        }
        else if ((currentMeter >= (quartile * 2)) && (currentMeter < (quartile * 3)))
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
    protected void TestStatsPlayer()
    {
        statAttack = 1;
        //statDefense = 1;
        //statResistance = 0;
        //statSpecial = 1;
        //statResistance = 0;
        maxHealth = 100;
        maxMeter = 100;
    }

    public void SetTest()
    {
        statAttack = 5;
        //statDefense = 2;
        //statResistance = 1;
        //statSpecial = 3;
        //statResistance = 1;
        currentHealth = maxHealth;
        currentMeter = 0;
    }
}
