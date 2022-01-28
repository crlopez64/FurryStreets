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
    private float sizeTimer;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (sizeTimer > 0)
        {
            sizeTimer -= Time.deltaTime;
        }
        fountainPen.fontSize = Mathf.Lerp(148, 360, sizeTimer / 0.125f);
    }

    /// <summary>
    /// Set the text of this value.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetText(int currentValue)
    {
        fountainPen.text = currentValue.ToString();
        sizeTimer = 0.125f;
    }
}
