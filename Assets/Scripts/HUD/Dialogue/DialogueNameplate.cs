using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of keeping track of the nameplate, or the one talking.
/// </summary>
public class DialogueNameplate : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image holder;
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        holder = GetComponent<Image>();
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
        AdjustParameters();
    }

    /// <summary>
    /// Set the nameplate. If null, turn it off.
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name, bool onRight)
    {
        if (name == null)
        {
            fountainPen.name = "";
            holder.enabled = false;
            return;
        }
        else
        {
            holder.enabled = true;
            fountainPen.name = name;
        }
        if (onRight)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.21f);
            rectTransform.anchorMax = new Vector2(0.7f, 0.26f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            fountainPen.alignment = TextAlignmentOptions.Right;
        }
        else
        {
            rectTransform.anchorMin = new Vector2(0.3f, 0.21f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.26f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            fountainPen.alignment = TextAlignmentOptions.Left;
        }
    }

    /// <summary>
    /// Adjust parameters to the rect transform.
    /// </summary>
    private void AdjustParameters()
    {
        rectTransform.anchorMin = new Vector2(0.3f, 0.21f);
        rectTransform.anchorMax = new Vector2(0.7f, 0.26f);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.sizeDelta = new Vector2(400, 60);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
}
