using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of doing normal "Actions" for the Player.
/// </summary>
public class PlayerAction : MonoBehaviour
{
    private Interactable currentInteractable;

    public SpriteRenderer actionSprite;

    private void Start()
    {
        actionSprite.enabled = false;
    }
    private void Update()
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.GetMustActionButton())
            {
                actionSprite.enabled = true;
            }
            else
            {
                actionSprite.enabled = false;
            }
        }
        else
        {
            actionSprite.enabled = false;
        }
    }

    /// <summary>
    /// Prepare the interactable should the Player want to interact with it.
    /// </summary>
    /// <param name="interactable"></param>
    public void PrepareInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }
    /// <summary>
    /// Unprepare the interactable, provided the Interactable given is the same as the one prepared.
    /// </summary>
    /// <param name="interactable"></param>
    public void UnprepareInteractable(Interactable interactable)
    {
        if (interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
    /// <summary>
    /// Interact with the interactable close by when needing to press a button.
    /// </summary>
    public void Interact()
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.GetMustActionButton())
            {
                currentInteractable.Interact();
            }
        }
    }
    /// <summary>
    /// Interact with the interactable close by automatically.
    /// </summary>
    public void InteractAuto()
    {
        if (currentInteractable != null)
        {
            if (!currentInteractable.GetMustActionButton())
            {
                currentInteractable.Interact();
            }
        }
    }
    /// <summary>
    /// Return if the new interactable has a higher priority than the current one. Will return false if both are equal.
    /// </summary>
    /// <param name="interactable"></param>
    /// <returns></returns>
    public bool NewInteractableHasPriority(Interactable interactable)
    {
        if (currentInteractable == null)
        {
            return true;
        }
        return interactable.GetPriority() > currentInteractable.GetPriority();
    }
    /// <summary>
    /// Return if the Player has an Interactable in front of them.
    /// </summary>
    /// <returns></returns>
    public bool HasInteractable()
    {
        return currentInteractable != null;
    }
}
