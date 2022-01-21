using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of Dialogue for player.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    private StreamReader reader;
    private DialogueHolder dialogueHolder;
    private string npcFolder;
    private string currentLine;

    /// <summary>
    /// Set the Dialogue holder.
    /// </summary>
    /// <param name="dialogueHolder"></param>
    public void SetDialogueHolder(DialogueHolder dialogueHolder)
    {
        this.dialogueHolder = dialogueHolder;
    }
    /// <summary>
    /// Set the string for the folder name.
    /// </summary>
    /// <param name="npcFolder"></param>
    public void SetNPCHolder(string npcFolder)
    {
        this.npcFolder = npcFolder;
    }
    /// <summary>
    /// Start the dialogue.
    /// </summary>
    /// <param name="currentPath"></param>
    public void StartDialogue(string currentPath)
    {
        GameManager.Instance.SetDialoguing(true);
        dialogueHolder.gameObject.SetActive(true);
        reader = new StreamReader(currentPath);
        GetNextLine(reader.ReadLine());
    }
    /// <summary>
    /// Advance the dialogue. If no other dialogue can occur, turn the game back on.
    /// </summary>
    public void AdvanceText()
    {
        if (reader == null)
        {
            Debug.LogError("ERROR: Reader has not been instantiate!!");
            return;
        }
        if (dialogueHolder.DialogueAnimating())
        {
            dialogueHolder.SkipDialogue(currentLine);
        }
        else
        {
            if (reader.Peek() >= 0)
            {
                GetNextLine(reader.ReadLine());
            }
            else
            {
                Debug.Log("Stop dialoguing");
                TurnOffDialogue();
            }
        }
    }
    /// <summary>
    /// Turn off the dialogue.
    /// </summary>
    public void TurnOffDialogue()
    {
        if (reader != null)
        {
            reader.Dispose();
            reader.Close();
        }
        dialogueHolder.ClearDialogue();
        dialogueHolder.gameObject.SetActive(false);
        GetComponent<GameManager>().SetDialoguing(false);
    }

    private void GetNextLine(string nextLine)
    {
        //Get dialogue line
        string[] data = nextLine.Split('|');
        currentLine = data[data.Length - 1];
        dialogueHolder.SetName(data[0], bool.Parse(data[2]));
        dialogueHolder.SetPortrait(npcFolder, data[1], bool.Parse(data[2]));
        dialogueHolder.SetDialogue(currentLine);
    }
}
