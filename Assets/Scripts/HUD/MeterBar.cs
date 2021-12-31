using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of adjusting the visual values.
/// </summary>
public class MeterBar : MonoBehaviour
{
    protected Color maxValueColor;
    protected Color minValueColor;
    protected Image[] images;
    protected float maxStatValue;
    protected const float maxFillValue = 0.58f;

    public Image fillDrain;
    public Image fillActual;
    public TextMeshProUGUI currentValueText;
    public TextMeshProUGUI maxValueText;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// Set the Actual Slider value. Drain Slider will follow suit.
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetSliderValue(float value)
    {
        //To fill in parent classes.
    }
    /// <summary>
    /// Set the Actual Slider value without the Drain animation.
    /// </summary>
    /// <param name="value"></param>
    public void SetSliderValueNoDrain(float value)
    {
        if (value > maxStatValue)
        {
            value = maxStatValue;
        }
        float percentage = value / maxStatValue;
        fillActual.fillAmount = percentage * maxFillValue;
        fillDrain.fillAmount = percentage * maxFillValue;
        if (currentValueText != null)
        {
            currentValueText.text = value.ToString();
        }
    }
    /// <summary>
    /// Set the max value for a stat.
    /// </summary>
    /// <param name="maxValue"></param>
    public void SetMaxValue(float maxValue)
    {
        maxStatValue = maxValue;
        if (maxValueText != null)
        {
            maxValueText.text = maxValue.ToString();
        }
    }
}
