using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CluesManager : MonoBehaviour
{
    #region Variables
    [Header("Individual clue display:")]
    public GameObject individualDisplay;
    public Text clueTitle;
    public Text clueDescription;
    public Image clueImage;

    [Space(5)]
    [Header("Clue sets display:")]
    public GameObject allDisplay;
    public List<Clue> clueSet1;
    public ClueButton[] buttonSet1;
    public List<Clue> clueSet2;
    public ClueButton[] buttonSet2;
    public List<Clue> unusedClues;
    public ClueButton[] unusedCluesButtons;

    [Space(5)]
    [Header("Button reset:")]
    public Sprite defaultButtonImage;

    private ClueButton selection;
    #endregion

    void Start()
    {
        RefreshDisplay();
    }

    void Update()
    {

    }

    public void DisplayIndividualClue()
    {
        if (selection != null)
        {
            allDisplay.SetActive(false);
            individualDisplay.SetActive(true);

            clueTitle.text = selection.clueRef.title;
            clueDescription.text = selection.clueRef.longDescription;
            clueImage.sprite = selection.clueRef.sprite;
        }
    }

    public void DisplayAll()
    {
        allDisplay.SetActive(true);
        individualDisplay.SetActive(false);

        RefreshDisplay();
    }

    public void ButtonPush(ClueButton button)
    {
        if (selection == null)
        {
            if (button.clueRef != null)
            {
                selection = button; //Select the target button
                RefreshDisplay();
            }
        }
        else if (selection != button)
        {
            if (button.clueRef == null)
            {
                button.clueRef = selection.clueRef;
                button.GetComponent<Image>().sprite = button.clueRef.sprite; //Assign new clue and image

                selection.GetComponent<Image>().sprite = defaultButtonImage; //Reset selection
                selection.clueRef = null;
                selection = null;

                RefreshDisplay();
            }
            else
                selection = button;
        }
        else
        {
            selection = null;
            RefreshDisplay();
        }
    }

    public void RefreshDisplay()
    {
        #region Set images
        for (int i = 0; i < buttonSet1.Length; i++)
        {
            if (buttonSet1[i].clueRef != null)
            {
                buttonSet1[i].GetComponent<Image>().sprite = buttonSet1[i].clueRef.sprite; //Assign image
                //sbuttonSet1[i].clueRef = clueSet1[i]; //Assign clue reference for button
                buttonSet1[i].clueRef.currentSet = 1; //Assign current set

                if (buttonSet1[i].clueRef.currentSet == buttonSet1[i].clueRef.correctSet)
                    buttonSet1[i].interactable = false; //Lock in place if correctly assigned
            }
            else
            {
                buttonSet1[i].GetComponent<Image>().sprite = defaultButtonImage;
            }
        }

        for (int i = 0; i < buttonSet2.Length; i++)
        {
            if (buttonSet2[i].clueRef != null)
            {
                buttonSet2[i].GetComponent<Image>().sprite = buttonSet2[i].clueRef.sprite; //Assign image
                //buttonSet2[i].clueRef = clueSet2[i]; //Assign clue reference for button
                buttonSet2[i].clueRef.currentSet = 2; //Assign current set

                if (buttonSet2[i].clueRef.currentSet == buttonSet2[i].clueRef.correctSet)
                    buttonSet2[i].interactable = false; //Lock in place if correctly assigned
            }
            else
            {
                buttonSet2[i].GetComponent<Image>().sprite = defaultButtonImage;
            }
        }

        for (int i = 0; i < unusedCluesButtons.Length; i++)
        {
            if (unusedCluesButtons[i].clueRef != null)
            {
                unusedCluesButtons[i].GetComponent<Image>().sprite = unusedCluesButtons[i].clueRef.sprite; //Assign image
                //unusedCluesButtons[i].clueRef = unusedClues[i]; //Assign clue reference for button
                unusedCluesButtons[i].clueRef.currentSet = 0; //Assign current set
            }
            else
            {
                unusedCluesButtons[i].GetComponent<Image>().sprite = defaultButtonImage;
            }
        }
        #endregion

        #region Set outlines
        if (selection != null)
        {
            for (int i = 0; i < buttonSet1.Length; i++)
            {
                //Outline buttons that are not locked
                if (buttonSet1[i].interactable && buttonSet1[i].clueRef == null)
                    buttonSet1[i].GetComponent<Outline>().enabled = true;
                else
                    buttonSet1[i].GetComponent<Outline>().enabled = false;

                if (buttonSet2[i].interactable && buttonSet2[i].clueRef == null)
                    buttonSet2[i].GetComponent<Outline>().enabled = true;
                else
                    buttonSet2[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < buttonSet1.Length; i++)
            {
                //Remove outline from all buttons
                buttonSet1[i].GetComponent<Outline>().enabled = false;
                buttonSet2[i].GetComponent<Outline>().enabled = false;
            }
        }
        #endregion
    }
}
