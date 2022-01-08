using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour
{
    private Image staticText;
    private ComboNumber number;
    private float timer;

    private void Awake()
    {
        number = GetComponentInChildren<ComboNumber>();
        staticText = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        
    }


    /// <summary>
    /// Set the text of this value.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetText(int currentValue)
    {
        number.SetText(currentValue);
    }
}
