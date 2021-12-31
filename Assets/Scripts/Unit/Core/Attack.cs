using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of a basic attack.
/// </summary>
public class Attack
{
    private readonly List<Attack> nextAttacks;
    private bool finalUniqueAttack;
    private bool unlocked;
    private bool canAddAttack;
    private readonly Vector2 knockback;
    private readonly Vector2 hitboxDimensions;
    private readonly Vector2 unitToMove;
    private readonly bool mustUnlock;
    private readonly byte hitType;
    private readonly byte requiredAttack;   //1 = Punch, 2 = Kick; 5 is Idle, Numbers 1-9 is direction input
    private readonly int animationID;
    private readonly int attackPowerMeter;
    private readonly int attackPowerPhysical;
    private readonly int attackPowerMeterSelf;
    private readonly string attackName;

    /// <summary>
    /// Constructor for Attack.
    /// </summary>
    /// <param name="attackName"></param>
    /// <param name="mustUnlock"></param>
    /// <param name="unlocked"></param>
    /// <param name="requiredDirection"></param>
    /// <param name="requiredAttack"></param>
    /// <param name="attackPowerPhysical"></param>
    /// <param name="attackPowerMeter"></param>
    /// <param name="attackPowerMeterSelf"></param>
    /// <param name="animationID"></param>
    /// <param name="hitboxWidth"></param>
    /// <param name="hitboxHeight"></param>
    /// <param name="hitType"></param>
    /// <param name="knockbackDistance"></param>
    /// <param name="knockbackHeight"></param>
    /// <param name="moveX"></param>
    /// <param name="moveY"></param>
    /// <param name="finalUniqueAttack"></param>
    public Attack(string attackName, bool mustUnlock, bool unlocked, byte requiredDirection, byte requiredAttack,
        int attackPowerPhysical, int attackPowerMeter, int attackPowerMeterSelf, int animationID,
        float hitboxWidth, float hitboxHeight, byte hitType, float knockbackDistance, float knockbackHeight, float moveX, float moveY,
        bool finalUniqueAttack)
    {
        canAddAttack = true;
        nextAttacks = new List<Attack>(2);
        this.mustUnlock = mustUnlock;
        this.unlocked = unlocked;
        if (requiredAttack > 4)
        {
            requiredAttack = 0;
        }
        this.requiredAttack = requiredAttack;
        if ((requiredDirection == 0) || (requiredDirection > 9))
        {
            requiredDirection = 5;
        }
        if (requiredDirection > 1)
        {
            this.requiredAttack = (byte)((requiredDirection << 4) | requiredAttack);
        }
        this.attackPowerPhysical = attackPowerPhysical;
        this.attackPowerMeter = attackPowerMeter;
        this.attackPowerMeterSelf = attackPowerMeterSelf;
        this.animationID = animationID;
        this.attackName = attackName;
        hitboxDimensions = new Vector2(hitboxWidth, hitboxHeight);
        this.hitType = hitType;
        this.finalUniqueAttack = finalUniqueAttack;
        knockback = new Vector2(knockbackDistance, knockbackHeight);
        unitToMove = new Vector2(moveX, moveY);
    }
    /// <summary>
    /// Add an attack to branch off of.
    /// </summary>
    /// <param name="attack"></param>
    public void AddAttack(Attack attack)
    {
        if (canAddAttack)
        {
            nextAttacks.Add(attack);
        }
    }
    /// <summary>
    /// Stop attacking attacks to branch off of.
    /// </summary>
    public void StopAddingAttacks()
    {
        canAddAttack = false;
        nextAttacks.TrimExcess();
    }
    /// <summary>
    /// Set if the move has been unlocked.
    /// </summary>
    public void HasBeenUnlocked()
    {
        unlocked = true;
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj as Attack);
    }
    public bool Equals(Attack attack)
    {
        if (attack == null)
        {
            return false;
        }
        if (attack == this)
        {
            return true;
        }
        if (GetType() != attack.GetType())
        {
            return false;
        }
        return ((attackName == attack.attackName) && (animationID == attack.animationID));
    }
    /// <summary>
    /// Return if this attack has any other attack to chain off to.
    /// </summary>
    /// <returns></returns>
    public bool HasOptions()
    {
        return nextAttacks.Count > 0;
    }
    /// <summary>
    /// Return if the Move has to be unlocked before using it.
    /// </summary>
    /// <returns></returns>
    public bool MustUnlock()
    {
        return mustUnlock;
    }
    /// <summary>
    /// Return if the move has been unlocked. If the move does not require to be unlocked, return true.
    /// </summary>
    /// <returns></returns>
    public bool Unlocked()
    {
        return (!mustUnlock) || (mustUnlock && unlocked);
    }
    /// <summary>
    /// Is this the final attack in an Attack string?
    /// Another attack with a different name branching off this one will still make this attack unique.
    /// </summary>
    /// <returns></returns>
    public bool IsFinalUniqueAttack()
    {
        return finalUniqueAttack;
    }
    /// <summary>
    /// Return the Hit Type for hitstun against this Unit.
    /// </summary>
    /// <returns></returns>
    public byte GetHitType()
    {
        return hitType;
    }
    /// <summary>
    /// Return the required direction of the attack.
    /// </summary>
    /// <returns></returns>
    public byte RequiredDirection()
    {
        return (byte)(requiredAttack >> 4);
    }
    /// <summary>
    /// Return the required attack button of the attack.
    /// </summary>
    /// <returns></returns>
    public byte RequiredAttack()
    {
        return (byte)(requiredAttack & 0xF);
    }
    /// <summary>
    /// Return both the required attack and direction of the attack.
    /// </summary>
    /// <returns></returns>
    public byte RequiredDirectionAndAttack()
    {
        return requiredAttack;
    }
    /// <summary>
    /// Get the Physical Attack power to return.
    /// </summary>
    /// <returns></returns>
    public int GetAttackPowerPhysical()
    {
        return attackPowerPhysical;
    }
    /// <summary>
    /// Get the Meter Attack power to return.
    /// </summary>
    /// <returns></returns>
    public int GetAttackPowerMeter()
    {
        return attackPowerMeter;
    }
    /// <summary>
    /// Get the Meter Attack power recoil to return.
    /// </summary>
    /// <returns></returns>
    public int GetAttackPowerMeterSelf()
    {
        return attackPowerMeterSelf;
    }
    /// <summary>
    /// Get the animation ID to return.
    /// </summary>
    /// <returns></returns>
    public int GetAnimationID()
    {
        return animationID;
    }
    /// <summary>
    /// Return the count of branches from the current attack.
    /// </summary>
    /// <returns></returns>
    public int Length()
    {
        return nextAttacks.Count;
    }
    /// <summary>
    /// Get the attack name of the attack.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return attackName;
    }
    /// <summary>
    /// Get the Attack in text form.
    /// </summary>
    /// <returns></returns>
    public string GetAttackText()
    {
        string text = "";
        switch(RequiredDirection())
        {
            case 1:
                text += "↙";
                break;
            case 2:
                text += "↓";
                break;
            case 3:
                text += "↘";
                break;
            case 4:
                text += "←";
                break;
            case 5:
                //Do not add any direction
                break;
            case 6:
                text += "→";
                break;
            case 7:
                text += "↖";
                break;
            case 8:
                text += "↑";
                break;
            case 9:
                text += "↗";
                break;
            default:
                text += "Illegal Movement";
                break;
        }
        switch(RequiredAttack())
        {
            case 0:
                text += "";
                break;
            case 1:
                text += "P ";
                break;
            case 2:
                text += "K ";
                break;
            default:
                text += "Illegal Attack or Not Yet Defined ";
                break;
        }
        return text;
    }
    /// <summary>
    /// Return the dimensions of the hitbox when attacking.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHitboxDimensions()
    {
        return hitboxDimensions;
    }
    /// <summary>
    /// Return the knockback when an attack connects to an enemy.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetKnockback()
    {
        return knockback;
    }
    /// <summary>
    /// Return the speed or jump speed when attacking.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetUnitToMove()
    {
        return unitToMove;
    }
    /// <summary>
    /// Get the next attack of the specificied index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Attack GetNextAttack(int index)
    {
        return nextAttacks[index];
    }
    /// <summary>
    /// Return the next attacks to branch off of.
    /// </summary>
    /// <returns></returns>
    public List<Attack> GetNextAttacks()
    {
        return nextAttacks;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return attackName + ": " + RequiredDirection() + RequiredAttack();
    }
}
