using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track if at least 1 of the hitboxes in the group landed.
/// </summary>
public class HitboxGroup : MonoBehaviour
{
    private bool hitboxLanded;
    private Hitbox[] hitboxes;

    private void Awake()
    {
        hitboxes = GetComponentsInChildren<Hitbox>();
    }
    
    /// <summary>
    /// Set hitbox landed to false.
    /// </summary>
    public void ResetHitboxLanded()
    {
        hitboxLanded = false;
    }
    /// <summary>
    /// Check if any one of the hitboxes found an attack.
    /// </summary>
    public void SetHitboxLanded()
    {
        hitboxLanded = true;
    }
    /// <summary>
    /// Has a hitbox registered an attack?
    /// </summary>
    /// <returns></returns>
    public bool HitboxLanded()
    {
        return hitboxLanded;
    }
    /// <summary>
    /// Return the Hitbox meant to hit Grounded Enemies.
    /// </summary>
    /// <returns></returns>
    public Hitbox GroundHitbox()
    {
        foreach(Hitbox hitbox in hitboxes)
        {
            if (!hitbox.ForAirborne())
            {
                return hitbox;
            }
        }
        return null;
    }
    /// <summary>
    /// Return the Hitbox meant to hit Airborne Enemies.
    /// </summary>
    /// <returns></returns>
    public Hitbox AirborneHitbox()
    {
        foreach (Hitbox hitbox in hitboxes)
        {
            if (hitbox.ForAirborne())
            {
                return hitbox;
            }
        }
        return null;
    }
}
