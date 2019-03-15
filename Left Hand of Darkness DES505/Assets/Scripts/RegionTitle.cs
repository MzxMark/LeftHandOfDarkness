using System;
using UnityEngine;
using UnityEngine.UI;

public class RegionTitle : MonoBehaviour
{

    /// <summary>
    /// The text element used for displaying the title.
    /// </summary>
    private Text display;
    /// <summary>
    /// The title of this region.
    /// </summary>
    [Tooltip("The title of this region.")]
    public string title;

    private bool displaying;
    public float displayTime;
    private float displayTimeDefault;

    private bool fadeIn;
    private bool fadeOut;

    void Start()
    {
        try
        {
            display = GameObject.Find("RegionTitle").GetComponent<Text>();
        }
        catch (Exception)
        {
            print("ERROR in RegionTitle.cs on " + name + ": No text element \"RegionTitle\" could be found.  Please check the canvas.");
        }

        if (displayTime == 0)
            displayTime = 4;

        displayTimeDefault = displayTime;
    }

    void Update()
    {
        if (fadeIn)
            FadeIn();
        else if (displaying)
            displayTime -= Time.deltaTime;
        else if (fadeOut)
            FadeOut();

        if (displayTime <= 0)
        {
            displaying = false;
            fadeOut = true;
            displayTime = displayTimeDefault;
        }
    }

    /// <summary>
    /// Triggers the display of this region's title.
    /// </summary>
    public void Display()
    {
        fadeIn = true;

        Color transparent = display.color;
        transparent.a = 0;
        display.color = transparent;

        display.text = title;

        displayTime = displayTimeDefault;
    }

    void FadeIn()
    {
        Color towardOpaque = display.color;

        towardOpaque.a += Time.deltaTime * 2;

        display.color = towardOpaque;

        if (towardOpaque.a >= 1)
        {
            fadeIn = false;
            displaying = true;
        }
    }

    void FadeOut()
    {
        Color towardTransparent = display.color;

        towardTransparent.a -= Time.deltaTime * 2;

        display.color = towardTransparent;

        if (towardTransparent.a <= 0)
            fadeOut = false;
    }
}
