using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The name used in conversation with this NPC.
    /// </summary>
    [Tooltip("The name used in conversation with this NPC.")]
    public string npcName;

    [Space(5)]
    [Header("Trade:")]
    public bool vendor;
    private bool vendorTrading;
    private VendorInventory inventory;
    private TradeManager tradeManager;

    [Space(5)]
    [Header("Navigation:")]
    public bool mobile;
    private NPCMovement movement;
    private Transform player;
    private PlayerController playerController;

    [Space(5)]
    [Header("Dialogue:")]
    /// <summary>
    /// The full dialogue set for this character, including player dialogue.
    /// </summary>
    [Tooltip("The full dialogue set for this character, including player dialogue.")]
    public TextAsset dialogue;
    public string[] dialogueLines;
    public string[] dialogueFormatting;
    private DialogueManager dialogueManager;
    private int currentLine = 0;
    private bool conversing = false;
    private bool waitingForChoice = false;
    private int numChoices = 0;
    private bool lastLine = false;
    private int[] destinationLine = new int[3];
    private bool[] choiceOpensTrade = new bool[3];

    #region Dialogue formatting reference
    //The following are the formatting rules for dialogue.  These rules are presented to the designers and
    //influence the structure of this script.  These should be strictly adhered to by designers and coders.

    //Formatting:

    //Every line should start with its own line number and end with the number of the next line in its sequence.  The last line in a sequence should have no ending number.  For example:
    //1 This is line one. 2
    //2 This is line two. 3
    //3 This is line three.

    //If a line is to be said by the player, preface it with a 'P'.  For example:
    //1P I am the player character.

    //If a line is to be said by the NPC, preface it with an 'N'.  For example:
    //1N I am an NPC.

    //If a line is to be one of multiple choices, preface it with a 'C' and add decimals to its line number.  For example:
    //1.1C This is your first choice.
    //1.2C This is your second choice.
    //DO NOT create an option for goodbye.  This will be automatically populated as an option whenever choices are prompted.
    //Do not give more than three options.  The maximum number of options that can be displayed is four, and the last one will always automatically be "Goodbye."

    //Here is an example dialogue tree:
    //1P Hello, NPC! 2
    //2N Hello, player.  Would you like to trade? 3
    //3.1C Yes! 4
    //3.2C No! 5
    //4 Okay, let's trade.
    //5N Okay, goodbye!
    //Note: The code will automatically add a "Goodbye" option to the choices as line 3.3.
    #endregion
    #endregion

    void Awake()
    {
        //Player reference initialisation
        try
        {
            player = GameObject.Find("Player").transform;
            playerController = player.GetComponent<PlayerController>();
        }
        catch (Exception)
        {
            print("ERROR in NPCInteraction.cs: No object \"Player\" exists in the current scene or it does not have a PlayerController.cs script component attached to it.");
        }

        //Dialogue initialisation
        try
        {
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        }
        catch (Exception)
        {
            print("ERROR in NPCInteraction.cs: No object \"DialogueManager\" exists in the scene or it does not contain an instance of the DialogueManager.cs script.  Have you instantiated the DialogueManager prefab?");
        }

        //Trade initialisation
        if (vendor)
        {
            try
            {
                tradeManager = GameObject.Find("TradeManager").GetComponent<TradeManager>();
            }
            catch (Exception)
            {
                print("ERROR in NPCInteraction.cs: No object \"TradeManager\" exists in the scene or it does not contain an instance of the TradeManager.cs script.  Have you instantiated the TradeManager prefab?");
            }

            try
            {
                inventory = GetComponent<VendorInventory>();
            }
            catch (MissingComponentException)
            {
                print("ERROR in NPCInteraction.cs: Character " + name + " does not have a VendorInventory.cs script component attached to it.");
            }
        }

        //Motility initialisation
        if (mobile)
        {
            try
            {
                movement = GetComponent<NPCMovement>();
            }
            catch (MissingComponentException)
            {
                print("ERROR in NPCInteraction.cs: Character " + name + " does not have a NPCMovement.cs script component attached to it.");
            }
        }
    }

    void Start()
    {
        if (dialogue != null)
        {
            ProcessDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && dialogue != null && !conversing)
        {
            if (Vector3.Distance(transform.position, player.position) <= 5)
            {
                conversing = true;
                playerController.FreezePlayer(true);
                Camera.main.GetComponent<CameraOrbit>().ChangeTradeState(true);
                ConversationStep();
            }
        }

        if (conversing)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !lastLine)
            {
                currentLine = dialogueLines.Length - 1;
                waitingForChoice = false;
                lastLine = true;

                ConversationStep();
            }
            else if (!waitingForChoice)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (lastLine)
                    {
                        EndConversation();
                    }
                    else
                        ConversationStep();
                }
            }
            else if (numChoices == 2)
            {
                #region Parse choice input
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    if (choiceOpensTrade[0] == true) //Open trade menu
                    {
                        EndConversation();
                        OpenTrade();
                    }
                    else //Continue conversation
                    {
                        currentLine = destinationLine[0] - 1;
                        numChoices = 0;
                        waitingForChoice = false;

                        ConversationStep();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    if (choiceOpensTrade[1] == true) //Open trade menu
                    {
                        EndConversation();
                        OpenTrade();
                    }
                    else //Continue conversation
                    {
                        currentLine = destinationLine[1] - 1;
                        numChoices = 0;
                        waitingForChoice = false;

                        ConversationStep();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    //Say goodbye
                    currentLine = dialogueLines.Length - 1;
                    numChoices = 0;
                    waitingForChoice = false;
                    lastLine = true;

                    ConversationStep();
                }
                #endregion
            }
            else if (numChoices == 3)
            {
                #region Parse choice input
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    if (choiceOpensTrade[0] == true) //Open trade menu
                    {
                        EndConversation();
                        OpenTrade();
                    }
                    else //Continue conversation
                    {
                        currentLine = destinationLine[0] - 1;
                        numChoices = 0;
                        waitingForChoice = false;

                        ConversationStep();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    if (choiceOpensTrade[1] == true) //Open trade menu
                    {
                        EndConversation();
                        OpenTrade();
                    }
                    else //Continue conversation
                    {
                        currentLine = destinationLine[1] - 1;
                        numChoices = 0;
                        waitingForChoice = false;

                        ConversationStep();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    if (choiceOpensTrade[2] == true) //Open trade menu
                    {
                        EndConversation();
                        OpenTrade();
                    }
                    else //Continue conversation
                    {
                        currentLine = destinationLine[2] - 1;
                        numChoices = 0;
                        waitingForChoice = false;

                        ConversationStep();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    //Say goodbye
                    currentLine = dialogueLines.Length - 1;
                    numChoices = 0;
                    waitingForChoice = false;
                    lastLine = true;

                    ConversationStep();
                }
                #endregion
            }
            else
                print("ERROR in NPCInteraction.cs: NPC is waiting for choice, but number of choices is incompatible with formatting.");
        }

        if (Input.GetKeyDown(KeyCode.R) && !conversing)
        {
            if (vendor && Vector3.Distance(transform.position, player.position) <= 5)
                OpenTrade();
        }
    }

    void ProcessDialogue()
    {
        #region ProcessDialogue() README
        /* NOTE: 
         * In its current state, this function supports text files with up to 99 dialogue lines.
         * "Line" refers to anything displayed at one time, so a set of three choices in this context is
         * considered a single "line."
         * This means that the text file itself can have 149 actual lines so long as 50 of those are choices.
         * There is currently no need for an overflow to collect more lines than that because no conversation
         * in this game should carry on that long.  This can be revisited at a later date if it becomes
         * necessary.
         */
        #endregion

        dialogueLines = (dialogue.text.Split('\n'));
        dialogueFormatting = new string[dialogueLines.Length];

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            char[] line = dialogueLines[i].ToCharArray();

            if (i == dialogueLines.Length - 1) //This line is the last in the dialogue file
            {
                #region Formatting for last line in file
                //The last character on most lines is '\n', but the last line differs.
                //The last line must be processed separately after all other lines.

                if (line[2] == ' ') //This line number is single-digit
                {
                    char[] format = { line[0], line[1], ' ', line[line.Length - 1] };
                    dialogueFormatting[i] = new string(format); //Set dialogue formatting

                    char[] content = new char[line.Length - 4];
                    Array.Copy(line, 3, content, 0, line.Length - 4);
                    dialogueLines[i] = new string(content); //Set dialogue content

                    //e.g. "9P #"
                }
                else
                {
                    char[] format = { line[0], line[1], line[2], ' ', line[line.Length - 1] };
                    dialogueFormatting[i] = new string(format); //Set dialogue formatting

                    char[] content = new char[line.Length - 5];
                    Array.Copy(line, 3, content, 0, line.Length - 5);
                    dialogueLines[i] = new string(content); //Set dialogue content

                    //e.g. "10P #"
                }
                #endregion
                //print(i + ": " + dialogueLines[i] + "\n" + i + ": " + dialogueFormatting[i]);
            }
            else if (line[2] == ' ' || (line[1] == 'C' && line[3] == ' ')) //This line number is single-digit
            {
                #region Formatting for single-digit line numbers
                if (line[1] == 'C') //This line is a choice
                {
                    #region Formatting for choices
                    if (line[line.Length - 3] == ' ') //Next sequential dialogue is on a single-digit line number
                    {
                        char[] format = { line[0], line[1], line[2], ' ', line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 6];
                        Array.Copy(line, 4, content, 0, line.Length - 6);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "5C- 6"
                    }
                    else //Next sequential dialogue is on a double-digit line number
                    {
                        char[] format = { line[0], line[1], line[2], ' ', line[line.Length - 3], line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 7];
                        Array.Copy(line, 4, content, 0, line.Length - 7);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "9C- 10"
                    }
                    #endregion
                }
                else //This line is either the player character or NPC speaking
                {
                    #region Formatting for speech
                    if (line[line.Length - 3] == ' ') //Next sequential dialogue is on a single-digit line number
                    {
                        char[] format = { line[0], line[1], ' ', line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 5];
                        Array.Copy(line, 3, content, 0, line.Length - 5);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "5P 6"
                    }
                    else //Next sequential dialogue is on a double-digit line number
                    {
                        char[] format = { line[0], line[1], ' ', line[line.Length - 3], line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 6];
                        Array.Copy(line, 3, content, 0, line.Length - 6);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "9P 10"
                    }
                    #endregion
                }
                #endregion

                //print(i + ": " + dialogueLines[i] + "\n" + i + ": " + dialogueFormatting[i]);
            }
            else if (line[3] == ' ' || (line[2] == 'C' && line[4] == ' ')) //This line number is double-digit
            {
                #region Formatting for double-digit line numbers
                if (line[2] == 'C') //This line is a choice
                {
                    #region Formatting for choices
                    char[] format = { line[0], line[1], line[2], line[3], ' ', line[line.Length - 3], line[line.Length - 2] };
                    dialogueFormatting[i] = new string(format); //Set dialogue formatting

                    char[] content = new char[line.Length - 8];
                    Array.Copy(line, 4, content, 0, line.Length - 8);
                    dialogueLines[i] = new string(content); //Set dialogue content

                    //e.g. "10C- 11"
                    #endregion
                }
                else //This line is either the player character or NPC speaking
                {
                    #region Formatting for speech
                    if (line[line.Length - 2] == '#') //This line terminates the conversation
                    {
                        char[] format = { line[0], line[1], line[2], ' ', line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 6];
                        Array.Copy(line, 3, content, 0, line.Length - 6);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "10P #"
                    }
                    else //The conversation continues past this line
                    {
                        char[] format = { line[0], line[1], line[2], ' ', line[line.Length - 3], line[line.Length - 2] };
                        dialogueFormatting[i] = new string(format); //Set dialogue formatting

                        char[] content = new char[line.Length - 7];
                        Array.Copy(line, 3, content, 0, line.Length - 7);
                        dialogueLines[i] = new string(content); //Set dialogue content

                        //e.g. "10P 11"
                    }
                    #endregion
                }
                #endregion

                //print(i + ": " + dialogueLines[i] + "\n" + i + ": " + dialogueFormatting[i]);
            }
            else
            {
                print("ERROR in NPCInteraction.cs, ProcessDialogue(): This function does not currently support dialogue with a triple-digit number of lines.");
            }
        }
    }

    void ConversationStep()
    {
        if (dialogueFormatting[currentLine].Contains("#"))
        {
            waitingForChoice = false;
            lastLine = true;
        }

        if (dialogueFormatting[currentLine].Contains("P"))
        {
            dialogueManager.PrintPlayerText(dialogueLines[currentLine]); //Print player dialogue
        }
        else if (dialogueFormatting[currentLine].Contains("N"))
        {
            dialogueManager.PrintNPCText(dialogueLines[currentLine], npcName); //Print NPC dialogue
        }
        else if (dialogueFormatting[currentLine].Contains("C"))
        {
            #region Prompt player for choice
            waitingForChoice = true;
            string[] choices = new string[3];
            int iteration = 0;
            destinationLine = new int[3];
            choiceOpensTrade = new bool[3];

            for (int i = currentLine; i < dialogueFormatting.Length; i++)
            {
                if (dialogueFormatting[i].Contains("C"))
                {
                    if (dialogueLines[i].ToLower().Contains("trade")) //Check if this choice prompts trade
                    {
                        choiceOpensTrade[iteration] = true;
                        choices[iteration] = dialogueLines[i]; //Collect choice text
                    }
                    else
                    {
                        choiceOpensTrade[iteration] = false;
                        choices[iteration] = dialogueLines[i]; //Collect choice text
                        string[] temp = dialogueFormatting[i].Split(' ');
                        Int32.TryParse(temp[1], out destinationLine[iteration]); //Add the target line to the array for later reference
                    }
                }
                else
                    break; //Break after collecting all necessary values

                iteration++;
            }

            if (choices[2] == null)
            {
                dialogueManager.PromptChoice(choices[0], choices[1]);
                numChoices = 2;
                currentLine++; //Skip one line (the second choice)
            }
            else
            {
                dialogueManager.PromptChoice(choices[0], choices[1], choices[2]);
                numChoices = 3;
                currentLine += 2; //Skip two lines (the second and third choices)
            }
            #endregion
        }
        else
        {
            print("ERROR in NPCInteraction.cs: Dialogue could not be formatted for NPC " + name.ToUpper() + " using file " + dialogue.name.ToUpper() + ".");
        }

        currentLine++;
    }

    void EndConversation()
    {
        dialogueManager.EndDialogue();
        playerController.FreezePlayer(false);
        Camera.main.GetComponent<CameraOrbit>().ChangeTradeState(false);

        currentLine = 0;
        conversing = false;
        waitingForChoice = false;
        numChoices = 0;
        lastLine = false;
        destinationLine = new int[3];
        choiceOpensTrade = new bool[3];
    }

    public void OpenTrade()
    {
        if (!vendorTrading)
        {
            tradeManager.OpenTrade(inventory);
        }

        vendorTrading = !vendorTrading;
        Camera.main.GetComponent<CameraOrbit>().ChangeTradeState(vendorTrading);
    }
}
