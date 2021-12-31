using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBlackPanel : MonoBehaviour
{
    private Image blackPanel;
    private bool fadeIn;
    private float alpha;

    private void Awake()
    {
        blackPanel = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FadeIn();
        }
        if (fadeIn)
        {
            if (alpha < 1f)
            {
                alpha += Time.deltaTime * 4f;
            }
        }
        else
        {
            if (alpha > 0f)
            {
                alpha -= Time.deltaTime * 4f;
            }
        }
        alpha = Mathf.Clamp(alpha, 0f, 1f);
        blackPanel.color = new Color(0, 0, 0, alpha);
    }

    /// <summary>
    /// Turn on the black panel.
    /// </summary>
    public void FadeIn()
    {
        fadeIn = true;
    }
    /// <summary>
    /// Turn off the black panel.
    /// </summary>
    public void FadeOut()
    {
        fadeIn = false;
    }
    /// <summary>
    /// Wipe out the panel.
    /// </summary>
    public void WipeOutPanel()
    {
        fadeIn = false;
        alpha = 0f;
        blackPanel.color = new Color(0, 0, 0, alpha);
    }
    /// <summary>
    /// Returns if the panel is fully black.
    /// </summary>
    /// <returns></returns>
    public bool MaxFade()
    {
        return alpha >= 1f;
    }
}
