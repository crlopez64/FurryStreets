using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of moving the Player to another location, within the scene or to a new scene.
/// </summary>
public class LocationTeleport : Interactable
{
    private PlayerMove player;

    public Transform teleportTo;
    public LocationBinds locationBinds;

    protected virtual void Start()
    {
        useBlackFade = true;
        priority = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            player = collision.GetComponentInParent<PlayerMove>();
            if (!mustActionButton)
            {
                player.GetComponentInParent<PlayerAction>().PrepareInteractable(this);
                player.GetComponentInParent<PlayerAction>().InteractAuto();
            }
            else
            {
                if (player.GetComponentInParent<PlayerAction>().NewInteractableHasPriority(this))
                {
                    player.GetComponentInParent<PlayerAction>().PrepareInteractable(this);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6) || (collision.gameObject.layer == 8))
        {
            player.GetComponentInParent<PlayerAction>().PrepareInteractable(null);
            player = null;
        }
    }

    /// <summary>
    /// Move within the scene or to a new scene.
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(TeleportPlayer(player));
    }

    private IEnumerator TeleportPlayer(PlayerMove player)
    {
        GameManager.Instance.BlackFadeOn();
        player.GetComponentInParent<PlayerAction>().PrepareInteractable(null);
        yield return new WaitForSeconds(0.5f);
        player.transform.position = teleportTo.position;
        GameManager.Instance.SetCameraClamps(locationBinds);
        yield return null;
        GameManager.Instance.BlackFadeOff();
    }
}
