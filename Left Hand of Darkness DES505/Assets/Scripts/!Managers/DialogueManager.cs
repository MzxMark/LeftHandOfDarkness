using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    #region Variables
    public GameObject dialogueBox;
    [Space(5)]
    public TextPrinter player;
    public TextPrinter npc;
    public TextPrinter[] choices;
    [Space(5)]
    public Text playerNameDisplay;
    public Text npcNameDisplay;

    private NPCInteraction currentNPC;

    private int choicesOffered;
    private int choice = 0;
    private bool choiceMade;
    #endregion

    public void PrintPlayerText(string textToPrint)
    {
        if (!dialogueBox.activeSelf) //Display UI if it is not already displaying
            DisplayUIElements();

        player.gameObject.SetActive(true); //Reset dialogue boxes
        npc.gameObject.SetActive(false);

        playerNameDisplay.text = "Player"; //Reset names
        npcNameDisplay.text = "";

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false); //Reset choices
        }

        player.PrintText(textToPrint); //Trigger printing and translation
    }

    public void PrintNPCText(string textToPrint, string npcName)
    {
        if (!dialogueBox.activeSelf) //Display UI if it is not already displaying
            DisplayUIElements();

        player.gameObject.SetActive(false); //Reset dialogue boxes
        npc.gameObject.SetActive(true);

        playerNameDisplay.text = ""; //Reset names
        npcNameDisplay.text = npcName;

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false); //Reset choices
        }

        npc.PrintText(textToPrint); //Trigger printing and translation
    }

    public void PromptChoice(string choiceOne, string choiceTwo)
    {
        if (!dialogueBox.activeSelf) //Display UI if it is not already displaying
            DisplayUIElements();

        player.gameObject.SetActive(false); //Reset dialogue boxes
        npc.gameObject.SetActive(false);

        playerNameDisplay.text = ""; //Reset names
        npcNameDisplay.text = "";

        choices[0].gameObject.SetActive(true); //Reset choices
        choices[1].gameObject.SetActive(true);
        choices[2].gameObject.SetActive(true);
        choices[3].gameObject.SetActive(false);

        choices[0].PrintText("1. " + choiceOne);
        choices[1].PrintText("2. " + choiceTwo);
        choices[2].PrintText("3. Goodbye.");

        choicesOffered = 3;

        StartCoroutine(WaitForChoice()); //Await input
    }

    public void PromptChoice(string choiceOne, string choiceTwo, string choiceThree)
    {
        if (!dialogueBox.activeSelf) //Display UI if it is not already displaying
            DisplayUIElements();

        player.gameObject.SetActive(false); //Reset dialogue boxes and use left-anchored player box for choices
        npc.gameObject.SetActive(false);

        playerNameDisplay.text = ""; //Reset names
        npcNameDisplay.text = "";

        choices[0].gameObject.SetActive(true); //Reset choices
        choices[1].gameObject.SetActive(true);
        choices[2].gameObject.SetActive(true);
        choices[3].gameObject.SetActive(true);

        choices[0].PrintText("1. " + choiceOne); //Print choices
        choices[1].PrintText("2. " + choiceTwo);
        choices[2].PrintText("3. " + choiceThree);
        choices[3].PrintText("4. Goodbye.");

        choicesOffered = 4;

        StartCoroutine(WaitForChoice()); //Await input
    }

    public void DisplayUIElements()
    {
        dialogueBox.SetActive(true);

        player.gameObject.SetActive(false);
        npc.gameObject.SetActive(false);

        playerNameDisplay.text = "";
        npcNameDisplay.text = "";
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }

    //void ProcessChoice()
    //{
    //    if (choice == 1)
    //    {
    //        PrintNPCText("Choice 1, eh?  I liked that one too.", "Test NPC");
    //    }
    //    else if (choice == 2)
    //    {
    //        PrintNPCText("Choice 2?  Kinda boring, but okay.", "Test NPC");
    //    }
    //    else if (choice == 3 && choicesOffered == 4)
    //    {
    //        PrintNPCText("Choice 3...  Wow.  You've got some nerve.", "Test NPC");
    //    }
    //    else if (choice == 4 || (choice == 3 && choicesOffered == 3))
    //    {
    //        PrintNPCText("Okay, see you later.", "Test NPC");
    //    }

    //    choiceMade = false;
    //    choice = 0;
    //}

    IEnumerator WaitForChoice()
    {
        while (!choiceMade)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                choice = 1;
                choiceMade = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                choice = 2;
                choiceMade = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                choice = 3;
                choiceMade = true;
            }
            else if (choicesOffered == 4 && (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)))
            {
                choice = 4;
                choiceMade = true;
            }

            yield return null;
        }

        //ProcessChoice();
    }
}
