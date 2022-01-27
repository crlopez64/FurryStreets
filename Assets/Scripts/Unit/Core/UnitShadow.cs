using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of the shadow underneath the unit.
/// </summary>
public class UnitShadow : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Turn on the shadow.
    /// </summary>
    public void TurnOnShadow()
    {
        sprite.enabled = true;
    }
    /// <summary>
    /// Turn off the shadow.
    /// </summary>
    public void TurnOffShadow()
    {
        sprite.enabled = false;
    }
}
