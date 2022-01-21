using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of Interacting with NPC.
/// </summary>
public class NPCInteract : Interactable
{
    public Vector2 chapterTabs;
    public string filePathNames;
    /// <summary>
    /// The Folder Name both in Script/NPC Stuff to get scripts, and Resources/Dialoguej to get portraits, if any.
    /// </summary>
    public string folderName;
    public string filePathName;

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
        GameManager.Instance.SetNPCFolder(folderName);
        GameManager.Instance.StartDialogue("Assets\\Scripts\\Unit\\NPC\\NotEnemy\\NPC Dialogue\\" + folderName + "\\" + filePathName + ".txt");
    }
}
