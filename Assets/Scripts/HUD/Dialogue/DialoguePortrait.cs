using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of the portrait.
/// </summary>
public class DialoguePortrait : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image portrait; 

    private void Awake()
    {
        portrait = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        AdjustParameters();
    }

    /// <summary>
    /// Move the portrait direction to either left or right. If true, move it to the right; else, move it left.
    /// </summary>
    /// <param name="onRight"></param>
    public void SetDirection(bool onRight)
    {
        Debug.Log("Move portrait direction to either left or right");
        rectTransform.localPosition = onRight ? new Vector3(640, 0) : new Vector3(-640, 0);
    }
    /// <summary>
    /// Change the Portrait set up.
    /// </summary>
    /// <param name="toAddParameter"></param>
    public void SetImage(string toAddParameter)
    {
        Debug.Log("Change image when ready.");
        //portrait.sprite = toAddParameter;
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
    }
}
