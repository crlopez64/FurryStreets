using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of showing the current value of some stat.
/// </summary>
public class CharacterPanelCurrentValue : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the Current Value.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetCurrentHealthValue(int currentValue)
    {
        fountainPen.text = currentValue.ToString();
    }
}
