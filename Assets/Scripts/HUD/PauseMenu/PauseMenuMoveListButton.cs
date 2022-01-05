using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script for the button for one Move in the Move List in the Pause Menu.
/// </summary>
public class PauseMenuMoveListButton : PauseMenuButton, IPointerEnterHandler, ISelectHandler
{
    private string moveName;
    private string description;
    public TextMeshProUGUI moveNameVisual;
    public TextMeshProUGUI visualString;

    public void SetMove(List<Attack> currentAttackString)
    {
        moveName = currentAttackString[currentAttackString.Count - 1].GetName();
        moveNameVisual.text = currentAttackString[currentAttackString.Count - 1].GetName();
        description = "Add a description to these attacks";
        visualString.text = "";
        foreach (Attack attack in currentAttackString)
        {
            switch (attack.RequiredDirection())
            {
                case 1:
                    visualString.text += "1";
                    break;
                case 2:
                    visualString.text += "2";
                    break;
                case 3:
                    visualString.text += "3";
                    break;
                case 4:
                    visualString.text += "4";
                    break;
                case 6:
                    visualString.text += "6";
                    break;
                case 7:
                    visualString.text += "7";
                    break;
                case 8:
                    visualString.text += "8";
                    break;
                case 9:
                    visualString.text += "9";
                    break;
                default:
                    break;
            }
            switch (attack.RequiredAttack())
            {
                case 1:
                    visualString.text += "P ";
                    break;
                case 2:
                    visualString.text += "K ";
                    break;
                case 3:
                    visualString.text += "G ";
                    break;
                case 4:
                    visualString.text += "S ";
                    break;
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GetComponent<Button>().IsInteractable())
        {
            GetComponentInParent<PauseMenuMoveList>().moveName.text = moveName;
            GetComponentInParent<PauseMenuMoveList>().description.text = description;
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (GetComponent<Button>().IsInteractable())
        {
            GetComponentInParent<PauseMenuMoveList>().moveName.text = moveName;
            GetComponentInParent<PauseMenuMoveList>().description.text = description;
        }
    }
}
