using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of keeping track of meter drain.
/// </summary>
public class MeterDrain : MonoBehaviour
{
    protected Color drainColor;
    private Image drain;

    private void Awake()
    {
        drain = GetComponent<Image>();
    }

    /// <summary>
    /// Set the color for the drain.
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetColor(float r, float g, float b)
    {
        drainColor = new Color(r, g, b, 1f);
    }
    /// <summary>
    /// Animate drain.
    /// </summary>
    public void Drain()
    {
        drain.fillAmount -= Time.deltaTime * 0.2f;
    }
    /// <summary>
    /// Set the max value for a stat.
    /// </summary>
    /// <param name="maxValue"></param>
    public void SetMaxValue(float maxValue)
    {
        if (drain != null)
        {
            drain.fillAmount = maxValue / 2000;
        }
    }
    /// <summary>
    /// Adjust drain value to specified amount.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        if (drain != null)
        {
            drain.fillAmount = value / 2000;
        }
    }
    /// <summary>
    /// Get the percentage float.
    /// </summary>
    /// <returns></returns>
    public float GetValue()
    {
        return drain.fillAmount;
    }
    /// <summary>
    /// Get the drain.
    /// </summary>
    /// <returns></returns>
    public Image GetDrain()
    {
        return drain;
    }
}
