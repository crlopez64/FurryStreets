using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of holding Dialogue UI things.
/// </summary>
public class DialogueHolder : MonoBehaviour
{
    private DialogueText dialogueText;
    private DialoguePortrait dialoguePortrait;
    private DialogueNameplate dialogueNameplate;

    private void Awake()
    {
        dialogueText = GetComponentInChildren<DialogueText>();
        dialoguePortrait = GetComponentInChildren<DialoguePortrait>();
        dialogueNameplate = GetComponentInChildren<DialogueNameplate>();
        FindObjectOfType<DialogueManager>().SetDialogueHolder(this);
    }

    /// <summary>
    /// Set the portrait of the dialogue and turn it on.
    /// </summary>
    /// <param name="spritePath"></param>
    /// <param name="onRight"></param>
    public void SetPortrait(string folderName, string spritePath, bool onRight)
    {
        dialoguePortrait.SetPortrait(folderName, spritePath, onRight);
    }
    /// <summary>
    /// Set the Name title on the dialogue. If name = null, turn off.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onRight"></param>
    public void SetName(string name, bool onRight) 
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
    /// <summary>
    /// If dialogue is animating, skip it.
    /// </summary>
    /// <param name="dialogue"></param>
    public void SkipDialogue(string dialogue)
    {
        dialogueText.SkipDialogueAnimating(dialogue);
    }
    /// <summary>
    /// Turn off dialogue.
    /// </summary>
    public void ClearDialogue()
    {
        dialogueNameplate.SetName(null, true);
        dialoguePortrait.SetPortrait(null, null, true);
        dialogueText.SetDialogue(null);
    }
    /// <summary>
    /// Is the dialogue currently animating?
    /// </summary>
    /// <returns></returns>
    public bool DialogueAnimating()
    {
        return dialogueText.AnimatingText();
    }
}
