using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of showing the max value of some stat.
/// </summary>
public class CharacterPanelMaxValue : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the Max Health Value.
    /// </summary>
    /// <param name="maxValue"></param>
    public void SetMaxValue(int maxValue)
    {
        fountainPen.text = "/" + maxValue;
    }
}
