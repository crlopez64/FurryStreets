using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of marking the text for the one speaking.
/// </summary>
public class DialogueText : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI fountainPen;
    private Image holder;
    private bool animatingText;
    private string currentText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        holder = GetComponent<Image>();
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
        AdjustParameters();
    }

    /// <summary>
    /// Set the dialogue to the text.
    /// </summary>
    /// <param name="text"></param>
    public void SetDialogue(string text)
    {
        fountainPen.text = "";
        StartCoroutine(AnimateText(text));
    }
    /// <summary>
    /// If text is still animating, skip animating the text.
    /// </summary>
    /// <param name="text"></param>
    public void SkipDialogueAnimating(string text)
    {
        StopAllCoroutines();
        animatingText = false;
        fountainPen.text = text;
    }
    /// <summary>
    /// Is the text currently animating?
    /// </summary>
    /// <returns></returns>
    public bool AnimatingText()
    {
        return animatingText;
    }

    /// <summary>
    /// Adjust parameters to the rect transform.
    /// </summary>
    private void AdjustParameters()
    {
        rectTransform.anchorMin = new Vector2(0f, 0.02f);
        rectTransform.anchorMax = new Vector2(1f, 0.2f);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
    private IEnumerator AnimateText(string text)
    {
        animatingText = true;
        currentText = "";
        foreach(char letter in text)
        {
            currentText += letter;
            fountainPen.text = currentText;
            yield return null;
        }
        animatingText = false;
    }
    private IEnumerator AnimateDialogue(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        char[] bracketText = new char[15];
        int currentIndex = 0;
        string richText = "";

        for (int i = 0; i < text.Length; i++)
        {
            //Check if needing to change colors
            if (text[i] == '<')
            {
                currentIndex = i;
                while (text[currentIndex] != '>')
                {
                    richText += text[currentIndex];
                    currentIndex++;
                }
            }
            //Else, add to text
            currentIndex++;
            stringBuilder.Append(text[i]);
            fountainPen.text = stringBuilder.ToString();
        }
        yield return null;
    }
}
