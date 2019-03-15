using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrinter : MonoBehaviour
{

    #region Variables
    public bool npc;

    /// <summary>
    /// The text element used to display text in the alien font.
    /// </summary>
    [Space(5)]
    [Header("Text boxes:")]
    public Text alienText;
    /// <summary>
    /// The text element used to display text in the normal font.
    /// </summary>
    public Text englishText;

    [Space(5)]
    [Header("Fonts:")]
    public Font alienFont, englishFont;

    /// <summary>
    /// The amount of characters that will print before translation runs.
    /// </summary>
    [Space(5)]
    [Header("Print manipulation:")]
    [Tooltip("The amount of characters that will print before translation runs.")]
    public int staggerAmount;
    [Tooltip("The smaller the number, the faster the print.")]
    [Range(.05f, .5f)]
    public float printSpeed;

    /// <summary>
    /// Modifier applied when deleting characters in order to complete the string.  Default value is 2.
    /// </summary>
    private int deletionBuffer = 2;
    /// <summary>
    /// The number of characters that should be deleted on the next iteration of the translation.  Default value is 1.
    /// </summary>
    private int deletionCount = 1;
    private bool translating;

    /// <summary>
    /// The text scheduled for printing and subsequent translation.
    /// </summary>
    string printText = "This text is for testing only and should never be seen in-game.";
    #endregion

    public void PrintText(string textToPrint)
    {
        StopAllCoroutines();

        printText = textToPrint;
        //printText += "NPC";

        alienText.text = "";
        englishText.text = "";

        deletionBuffer = 2;
        deletionCount = 1;
        translating = false;

        if (printText.Length >= staggerAmount)
            StartCoroutine(Typewriter());
        else
            StartCoroutine(ShortTypewriter());
    }

    IEnumerator Typewriter()
    {
        for (int i = 0; i < (printText.Length + staggerAmount); i++)
        {
            if (translating == true)
            {
                if (i <= printText.Length)
                {
                    alienText.text = new string(' ', deletionCount) + printText.Substring(deletionCount, i - deletionCount); //Start printing string with first chars deleted
                }
                else
                {
                    alienText.text = new string(' ', deletionCount) + printText.Substring(deletionCount, i - (deletionCount + deletionBuffer)); //Start printing string with first chars deleted
                    deletionBuffer++;
                }

                if (deletionCount <= printText.Length)
                    deletionCount++;
            }
            else if (i == staggerAmount)
            { //This coroutine runs when the staggerAmount is shorter than the printText's length, so check the value against staggerAmount
                StartCoroutine(Translator()); //Trigger translation
                translating = true;
                alienText.text = printText.Substring(0, i);
            }
            else if (printText.Length < staggerAmount && i == printText.Length)
            {
                StartCoroutine(Translator()); //Trigger translation
                translating = true;
                alienText.text = printText.Substring(0, i);
            }
            else
                alienText.text = printText.Substring(0, i); //Start printing string until translation is triggered

            yield return new WaitForSeconds(.03f);
        }

        deletionCount = 1;
        deletionBuffer = 2;
        translating = false;
    }

    IEnumerator ShortTypewriter()
    {
        for (int i = 0; i < (printText.Length * 2); i++)
        {
            if (translating == true)
            {
                if (i <= printText.Length)
                {
                    alienText.text = new string(' ', deletionCount) + printText.Substring(deletionCount, i - deletionCount); //Start printing string with first chars deleted
                }
                else
                {
                    alienText.text = new string(' ', deletionCount) + printText.Substring(deletionCount, i - (deletionCount + deletionBuffer)); //Start printing string with first chars deleted
                    deletionBuffer++;
                }

                if (deletionCount <= printText.Length)
                    deletionCount++;
            }
            else if (i == printText.Length)
            { //This coroutine runs when the printText's length is shorter than the stagger amount, so it becomes the new stagger amount
                StartCoroutine(Translator()); //Trigger translation
                translating = true;
                alienText.text = printText.Substring(0, i);
            }
            else if (printText.Length < staggerAmount && i == printText.Length)
            {
                StartCoroutine(Translator()); //Trigger translation
                translating = true;
                alienText.text = printText.Substring(0, i);
            }
            else
                alienText.text = printText.Substring(0, i); //Start printing string until translation is triggered

            yield return new WaitForSeconds(.03f);
        }

        deletionCount = 1;
        deletionBuffer = 2;
        translating = false;
    }

    IEnumerator Translator()
    {
        string temp = alienText.text;

        for (int i = 0; i < (printText.Length + 1); i++)
        {
            englishText.text = printText.Substring(0, i);
            yield return new WaitForSeconds(.03f);
        }
    }
}
