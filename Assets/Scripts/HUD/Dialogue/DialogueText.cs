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
    private WaitForSeconds textTimer;
    private RectTransform rectTransform;
    private TextMeshProUGUI fountainPen;
    private Image holder;
    private bool animatingText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        holder = GetComponent<Image>();
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
        AdjustParameters();
    }
    private void Start()
    {
        SetTextSpeed(2);
    }

    /// <summary>
    /// Set the dialogue to the text.
    /// </summary>
    /// <param name="text"></param>
    public void SetDialogue(string text)
    {
        if (text == null)
        {
            fountainPen.text = "";
            return;
        }
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
        fountainPen.maxVisibleCharacters = text.Length;
    }
    /// <summary>
    /// Set the text speed. 0 = Slow, 2 = Fast.
    /// </summary>
    /// <param name="speed"></param>
    public void SetTextSpeed(int speed)
    {
        switch(speed)
        {
            case 0:
                textTimer = new WaitForSeconds(0.05f);
                break;
            case 1:
                textTimer = new WaitForSeconds(0.02f);
                break;
            default:
                textTimer = new WaitForSeconds(0.01f);
                break;
        }
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
        fountainPen.text = text;
        int currentVisible = 0;
        foreach(char letter in text)
        {
            fountainPen.maxVisibleCharacters = currentVisible;
            currentVisible++;
            yield return textTimer;
        }
        animatingText = false;
    }
}
