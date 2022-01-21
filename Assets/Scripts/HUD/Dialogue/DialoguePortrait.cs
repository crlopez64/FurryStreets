using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of the portrait.
/// </summary>
public class DialoguePortrait : MonoBehaviour
{
    private Image portrait;
    private RectTransform rectTransform;

    private void Awake()
    {
        portrait = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        AdjustParameters();
    }

    /// <summary>
    /// Move the portrait direction to either left or right and set it. If null, turn off portrait.
    /// If true, move it to the right; else, move it left.
    /// </summary>
    /// <param name="npcFolder"></param>
    /// <param name="portrait"></param>
    /// <param name="onRight"></param>
    public void SetPortrait(string npcFolder, string portrait, bool onRight)
    {
        if ((npcFolder == null) || (portrait == null))
        {
            this.portrait.enabled = false;
            return;
        }
        if (onRight)
        {
            rectTransform.localPosition = new Vector3(640, 0);
            rectTransform.anchorMin = new Vector2(0.74f, 0);
            rectTransform.anchorMax = new Vector2(0.94f, 0.35f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }
        else
        {
            rectTransform.localPosition = new Vector3(-640, 0);
            rectTransform.anchorMin = new Vector2(0.06f, 0);
            rectTransform.anchorMax = new Vector2(0.26f, 0.35f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }
        this.portrait.sprite = Resources.Load<Sprite>("HUD/Dialogue/" + npcFolder + "/" + portrait);
        if (!this.portrait.enabled)
        {
            this.portrait.enabled = true;
        }
    }

    /// <summary>
    /// Adjust parameters to the rect transform.
    /// </summary>
    private void AdjustParameters()
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.sizeDelta = new Vector2(400, 400);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
}
