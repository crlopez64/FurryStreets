using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHolder : MonoBehaviour
{
    public Image leftPortrait;
    public Image leftPortraitHolder;
    public Image rightPortrait;
    public Image rightPortraitHolder;
    public Image leftName;
    public Image rightName;
    public Image dialogue;

    /// <summary>
    /// Set the portrait of the dialogue and turn it on.
    /// </summary>
    /// <param name="onRight"></param>
    /// <param name="sprite"></param>
    public void SetPortrait(bool onRight, Sprite sprite)
    {
        if (onRight)
        {
            rightPortrait.sprite = sprite;
            rightPortraitHolder.enabled = true;
            leftPortraitHolder.enabled = false;

        }
        else
        {
            leftPortrait.sprite = sprite;
            rightPortraitHolder.enabled = false;
            leftPortraitHolder.enabled = true;
        }
    }
    /// <summary>
    /// Set the Name title on the dialogue.
    /// </summary>
    /// <param name="onRight"></param>
    /// <param name="name"></param>
    public void SetName(bool onRight, string name) 
    {
        if (onRight)
        {
            rightName.GetComponentInChildren<TextMeshProUGUI>().text = name;
            leftName.enabled = false;
            rightName.enabled = true;
        }
        else
        {
            leftName.GetComponentInChildren<TextMeshProUGUI>().text = name;
            rightName.enabled = false;
            leftName.enabled = true;
        }
    }
    /// <summary>
    /// Set the dialogue of the character speaking.
    /// </summary>
    /// <param name="dialogue"></param>
    public void SetDialogue(string dialogue)
    {
        //Set coroutine 
    }

    private IEnumerator AnimateDialogue(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        char[] textChar = text.ToCharArray();
        char[] bracketText = new char[15];
        byte specialCharacterCheck = 0;
        byte ignore = 0;
        for (int i = 0; i < text.Length; i++)
        {
            //Check if needing to change colors
            if (ignore > 0)
            {
                ignore--;
                continue;
            }
            if (textChar[i] == '<')
            {
                if (textChar[i+1] != '\\')
                {
                    //Turn off the color change
                    specialCharacterCheck = 0;
                    ignore = 8;
                    while (specialCharacterCheck < 8)
                    {
                        bracketText[specialCharacterCheck] = textChar[i + specialCharacterCheck];
                    }
                }
                else
                {
                    //Turn on color change
                    specialCharacterCheck = 0;
                    ignore = 15;
                    while (specialCharacterCheck < 15)
                    {
                        bracketText[specialCharacterCheck] = textChar[i + specialCharacterCheck];
                    }
                }
                
            }
            stringBuilder.Append(textChar[i]);
            dialogue.GetComponentInChildren<TextMeshProUGUI>().text = stringBuilder.ToString();
        }
        yield return null;
    }
}
