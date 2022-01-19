using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of Interacting with NPC.
/// </summary>
public class NPCInteract : Interactable
{

    private void Start()
    {
        priority = 1;
        mustActionButton = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            if (collision.gameObject.GetComponentInParent<PlayerAction>().NewInteractableHasPriority(this))
            {
                collision.gameObject.GetComponentInParent<PlayerAction>().PrepareInteractable(this);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            collision.gameObject.GetComponentInParent<PlayerAction>().PrepareInteractable(null);
        }
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Talk with NPC.");
    }
}