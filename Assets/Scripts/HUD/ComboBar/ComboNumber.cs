using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of showing current combo hits.
/// </summary>
public class ComboNumber : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the text of this value.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetText(int currentValue)
    {
        fountainPen.text = currentValue.ToString();
    }
}
