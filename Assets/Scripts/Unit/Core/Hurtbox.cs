using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of the Unit's hurtbox.
/// </summary>
public class Hurtbox : MonoBehaviour
{
    private BoxCollider2D hurtbox;
    private UnitAttack unitAttack;
    private UnitAnimationLayers unitAnimationLayers;

    private void Awake()
    {
        hurtbox = GetComponent<BoxCollider2D>();
        unitAttack = GetComponentInParent<UnitAttack>();
        unitAnimationLayers = GetComponentInParent<UnitAnimationLayers>();
    }
}
