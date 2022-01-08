using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDSpecialGraphic : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Play Meter Burn graphic.
    /// </summary>
    public void MeterBurn()
    {
        animator.SetTrigger("MeterBurn");
    }
    /// <summary>
    /// Turn off the graphic
    /// </summary>
    public void TurnOffGraphic()
    {
        gameObject.SetActive(false);
    }
}
