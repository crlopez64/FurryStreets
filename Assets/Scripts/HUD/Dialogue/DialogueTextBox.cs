using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of adjusting the textbox.
/// </summary>
public class DialogueTextBox : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); 
        AdjustParameters();
    }

    /// <summary>
    /// Adjust parameters to the rect transform.
    /// </summary>
    private void AdjustParameters()
    {
        rectTransform.anchorMin = new Vector2(0.29f, 0.1f);
        rectTransform.anchorMax = new Vector2(0.71f, 0.9f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
}
