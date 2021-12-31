using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Debug script in charge of showing the entire movelist.
/// </summary>
public class TextMoveListDebug : MonoBehaviour
{
    public TextMeshProUGUI fountainPenNumber;
    public TextMeshProUGUI fountainPen;
    public Image backdrop;
    public bool isActive;
    
    private void Awake()
    {
        backdrop.enabled = false;
        fountainPen.enabled = false;
        fountainPen.text = "";
        fountainPenNumber.text = "";
    }
    private void Start()
    {
        isActive = false;
    }

    /// <summary>
    /// Clear out the text and backdrop.
    /// </summary>
    public void ClearText()
    {
        isActive = false;
        backdrop.enabled = false;
        fountainPen.enabled = false;
        fountainPen.text = "";
        fountainPenNumber.text = "";
    }
    /// <summary>
    /// Make an entire list of the Player's (normal) attack movelist.
    /// </summary>
    /// <param name="playerAttack"></param>
    public void MakeText(Attack rootAttack)
    {
        isActive = true;
        backdrop.enabled = true;
        fountainPen.enabled = true;
        fountainPen.text = "";
        fountainPenNumber.text = "";
        MakeTextHelper("", rootAttack);
        TotalMoveCount(rootAttack);
    }
    public void TotalMoveCount(Attack rootAttack)
    {
        fountainPenNumber.text = TotalMoveCountHelper(rootAttack).ToString();
    }
    private int TotalMoveCountHelper(Attack currentAttack)
    {
        if (currentAttack.HasOptions())
        {
            foreach (Attack attack in currentAttack.GetNextAttacks())
            {
                if (currentAttack.GetName() != "Starting Null")
                {
                    return TotalMoveCountHelper(attack) + 1;
                }
                else
                {
                    return TotalMoveCountHelper(attack) + 0;
                }
            }
        }
        return 1;
    }
    private void MakeTextHelper(string currentString, Attack currentAttack)
    {
        if (currentAttack == null)
        {
            Debug.LogError("ERROR: Root is empty.");
            return;
        }
        if (currentAttack.GetName() != "Starting Null")
        {
            currentString = currentString + currentAttack.GetAttackText() + " ";
            if (currentAttack.HasOptions())
            {
                fountainPen.text += currentAttack.GetName() + " Partial: " + currentString + "\n";
            }
            else
            {
                fountainPen.text += currentAttack.GetName() + ": " + currentString + "\n";
            }
        }
        if (currentAttack.HasOptions())
        {
            foreach (Attack child in currentAttack.GetNextAttacks())
            {
                MakeTextHelper(currentString, child);
            }
        }
    }
}
