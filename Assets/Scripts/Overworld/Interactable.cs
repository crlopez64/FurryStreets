using UnityEngine;

/// <summary>
/// Script in charge of being able to interact with certain things.
/// </summary>
public class Interactable : MonoBehaviour
{ 
    protected bool mustActionButton;
    protected bool useBlackFade;
    protected byte priority;

    /// <summary>
    /// Return if the Interactable requires using the Black Fade.
    /// </summary>
    /// <returns></returns>
    public bool GetUseBlackFade()
    {
        return useBlackFade;
    }
    /// <summary>
    /// Return if the Player has to press as button to interact.
    /// </summary>
    /// <returns></returns>
    public bool GetMustActionButton()
    {
        return mustActionButton;
    }
    /// <summary>
    /// Return the priority of the Interactable.
    /// </summary>
    /// <returns></returns>
    public byte GetPriority()
    {
        return priority;
    }
    /// <summary>
    /// Interact with this Interactable.
    /// </summary>
    public virtual void Interact()
    {
        //Nothing
    }
}
