using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of the Move List panel in the Pause Menu.
/// </summary>
public class PauseMenuMoveList : MonoBehaviour
{
    private int moveListIndex;
    private PauseMenuMoveListButton[] buttons;

    public TextMeshProUGUI moveName;
    public TextMeshProUGUI description;
    public GameObject firstMove;

    private void Awake()
    {
        buttons = GetComponentsInChildren<PauseMenuMoveListButton>();
    }
    /// <summary>
    /// Show the move list and hope it shows correctly.
    /// </summary>
    /// <param name="nullAttack"></param>
    public void ShowMoveList(Attack nullAttack)
    {
        moveListIndex = 0;
        foreach(PauseMenuMoveListButton button in buttons) 
        {
            button.gameObject.SetActive(false);
        }
        foreach(Attack attack in nullAttack.GetNextAttacks())
        {
            ShowMoveListHelper(attack, new List<Attack>());
        }
    }
    private void ShowMoveListHelper(Attack currentAttack, List<Attack> stacked)
    {
        //Record the last stack to prevent over-referencing
        List<Attack> newStack = new List<Attack>(5);
        if (stacked.Count > 0)
        {
            foreach (Attack attack in stacked)
            {
                newStack.Add(attack);
            }
        }
        //Add the next attack
        newStack.Add(currentAttack);
        if (currentAttack.HasOptions())
        {
            foreach(Attack nextAttack in currentAttack.GetNextAttacks())
            {
                //If attack string has not been unlocked yet, ignore
                if (!nextAttack.Unlocked())
                {
                    continue;
                }
                if (currentAttack.IsFinalUniqueAttack())
                {
                    buttons[moveListIndex].SetMove(newStack);
                    buttons[moveListIndex].gameObject.SetActive(true);
                    moveListIndex++;
                }
                ShowMoveListHelper(nextAttack, newStack);
            }
        }
        else
        {
            if (currentAttack.IsFinalUniqueAttack())
            {
                buttons[moveListIndex].SetMove(newStack);
                buttons[moveListIndex].gameObject.SetActive(true);
                moveListIndex++;
            }
        }
    }
}
