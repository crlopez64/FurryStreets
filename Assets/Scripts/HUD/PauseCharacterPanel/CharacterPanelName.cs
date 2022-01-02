using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of keeping track of the Character name.
/// </summary>
public class CharacterPanelName : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the name to the Character Name.
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        fountainPen.text = name;
    }
}
