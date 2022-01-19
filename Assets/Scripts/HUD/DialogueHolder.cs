using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of holding Dialogue UI things.
/// </summary>
public class DialogueHolder : MonoBehaviour
{
    private DialoguePortrait dialoguePortrait;
    private DialogueNameplate dialogueNameplate;
    private DialogueText dialogueText;

    private void Awake()
    {
        dialoguePortrait = GetComponentInChildren<DialoguePortrait>();
        dialogueNameplate = GetComponentInChildren<DialogueNameplate>();
        dialogueText = GetComponentInChildren<DialogueText>();
    }

    /// <summary>
    /// Set the portrait of the dialogue and turn it on.
    /// </summary>
    /// <param name="onRight"></param>
    /// <param name="sprite"></param>
    public void SetPortrait(bool onRight, string spritePath)
    {
        dialoguePortrait.SetDirection(onRight);
        dialoguePortrait.SetImage(spritePath);
    }
    /// <summary>
    /// Set the Name title on the dialogue. If name = null, turn off.
    /// </summary>
    /// <param name="onRight"></param>
    /// <param name="name"></param>
    public void SetName(bool onRight, string name) 
    {
        dialogueNameplate.SetName(name, onRight);
    }
    /// <summary>
    /// Set the dialogue of the character speaking.
    /// </summary>
    /// <param name="dialogue"></param>
    public void SetDialogue(string dialogue)
    {
        dialogueText.SetDialogue(dialogue);
    }

}
