using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of a basic attack.
/// </summary>
public class Attack
{
    private readonly List<Attack> nextAttacks;
    private bool canAddAttack;
    private readonly bool finalUniqueAttack;
    private readonly Vector2 knockback;
    private readonly Vector2 unitToMove;
    private readonly Vector2 hitboxDimensions;
    private readonly byte hitType;
    private readonly byte requiredAttack; //1 = Punch, 2 = Kick; 5 is Idle, Numbers 1-9 is direction input
    private readonly byte attributes; //1st: Grab, 2: Heavy Stun, 3: Knockback, 4: Knockback Far, 5: Pop Up, 
    private readonly int damage;
    private readonly int animationID;
    private readonly int meterCost;
    private readonly string attackName;

    /// <summary>
    /// Constructor for Attack.
    /// </summary>
    /// <param name="attackName"></param>
    /// <param name="requiredDirection"></param>
    /// <param name="requiredAttack"></param>
    /// <param name="damage"></param>
    /// <param name="meterCost"></param>
    /// <param name="animationID"></param>
    /// <param name="hitboxWidth"></param>
    /// <param name="hitboxHeight"></param>
    /// <param name="hitType"></param>
    /// <param name="knockbackDistance"></param>
    /// <param name="knockbackHeight"></param>
    /// <param name="moveX"></param>
    /// <param name="moveY"></param>
    /// <param name="finalUniqueAttack"></param>
    /// <param name="attributes"></param>
    public Attack(string attackName, byte requiredDirection, string requiredAttack,
        int damage, int meterCost, int animationID,
        float hitboxWidth, float hitboxHeight, byte hitType, float knockbackDistance, float knockbackHeight, float moveX, float moveY,
        bool finalUniqueAttack, string attribute)
    {
        canAddAttack = true;
        nextAttacks = new List<Attack>(2);
        if ((requiredDirection == 0) || (requiredDirection > 9))
        {
            requiredDirection = 5;
        }
        this.requiredAttack = (byte)(requiredDirection << 4);
        this.requiredAttack |= CheckAttack(requiredAttack);
        this.damage = damage;
        this.meterCost = meterCost;
        this.animationID = animationID;
        this.attackName = attackName;
        hitboxDimensions = new Vector2(hitboxWidth, hitboxHeight);
        this.hitType = hitType;
        this.finalUniqueAttack = finalUniqueAttack;
        knockback = new Vector2(knockbackDistance, knockbackHeight);
        unitToMove = new Vector2(moveX, moveY);
        this.attributes = AddAttributes(attribute);
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
    /// Does this move have any Meter cost to them?
    /// </summary>
    /// <returns></returns>
    public bool HasMeterCost()
    {
        return meterCost > 0;
    }
    /// <summary>
    /// Can the Unit use this move?
    /// </summary>
    /// <param name="currentMeter"></param>
    /// <returns></returns>
    public bool CanUseMove(int currentMeter)
    {
        return currentMeter >= meterCost;
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
    /// Does this Attack have a Grab attribute? Grab follow up attacks should not have this attribute.
    /// </summary>
    /// <returns></returns>
    public bool AttributeGrab()
    {
        return (attributes & 0x1) == 0x1;
    }
    /// <summary>
    /// Does this Attack make more Stun damage than normally? If so, Stun Damage = 1.3 * Damage
    /// </summary>
    /// <returns></returns>
    public bool AttributeHeavyStun()
    {
        return ((attributes >> 1) & 0x1) == 0x1;
    }
    /// <summary>
    /// Does this Attack push back the Enemy?
    /// </summary>
    /// <returns></returns>
    public bool AttributeKnockback()
    {
        return ((attributes >> 2) & 0x1) == 0x1;
    }
    /// <summary>
    /// Does this Attack push back the Enemy really far?
    /// </summary>
    /// <returns></returns>
    public bool AttributeKnockbackFar()
    {
        return ((attributes >> 3) & 0x1) == 0x1;
    }
    /// <summary>
    /// Does this Attack pop the Enemy up into the air?
    /// </summary>
    /// <returns></returns>
    public bool AttributePopUp()
    {
        return ((attributes >> 4) & 0x1) == 0x1;
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
    /// Return the required attack button of the attack. 1(01): Punch, 2(10): Kick, 3(11): Throw, 4(100): Special
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
    /// Return the Attributes byte.
    /// </summary>
    /// <returns></returns>
    public byte GetAttributesByte()
    {
        return attributes;
    }
    /// <summary>
    /// Get the Damage to give incoming Enemies to return.
    /// </summary>
    /// <returns></returns>
    public int Damage()
    {
        return damage;
    }
    /// <summary>
    /// The Meter cost it takes to use this move.
    /// </summary>
    /// <returns></returns>
    public int MeterCost()
    {
        return meterCost;
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

    /// <summary>
    /// Get Attack Byte from string specified.
    /// </summary>
    /// <param name="attackString"></param>
    /// <returns></returns>
    private byte CheckAttack(string attackString)
    {
        if (attackString.Equals("punch", System.StringComparison.OrdinalIgnoreCase))
        {
            return 1;
        }
        if (attackString.Equals("kick", System.StringComparison.OrdinalIgnoreCase))
        {
            return 2;
        }
        if (attackString.Equals("grab", System.StringComparison.OrdinalIgnoreCase))
        {
            return 3;
        }
        if (attackString.Equals("throw", System.StringComparison.OrdinalIgnoreCase))
        {
            return 3;
        }
        if (attackString.Equals("special", System.StringComparison.OrdinalIgnoreCase))
        {
            return 4;
        }
        return 0;
    }
    /// <summary>
    /// Add attributes to this attack if able to branch off of attacks.
    /// </summary>
    /// <param name="attributes"></param>
    private byte AddAttributes(string attribute)
    {
        if (!canAddAttack)
        {
            return 0;
        }
        if (attribute == null)
        {
            return 0;
        }
        byte currentAttributes = 0;
        if (attribute.Contains("none", System.StringComparison.OrdinalIgnoreCase))
        {
            return 0;
        }
        if (attribute.Contains("grab", System.StringComparison.OrdinalIgnoreCase))
        {
            currentAttributes |= 0x1;
        }
        if (attribute.Contains("heavyStun", System.StringComparison.OrdinalIgnoreCase))
        {
            currentAttributes |= (0x1 << 1);
        }
        if (attribute.Contains("knockback", System.StringComparison.OrdinalIgnoreCase))
        {
            currentAttributes |= (0x1 << 2);
        }
        if (attribute.Contains("knockbackFar", System.StringComparison.OrdinalIgnoreCase))
        {
            currentAttributes |= (0x1 << 3);
        }
        if (attribute.Contains("popUp", System.StringComparison.OrdinalIgnoreCase))
        {
            currentAttributes |= (0x1 << 4);
        }
        return currentAttributes;
    }
}
