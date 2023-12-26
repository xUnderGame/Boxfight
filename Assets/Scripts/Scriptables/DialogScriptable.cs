using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Default Dialog", menuName = "Dialog Scriptable")]
public class DialogScriptable : ScriptableObject
{
    // Scriptable settings
    public string[] dialog = { "Hello, world.", "Programmed to work and not to feel.", "Not even sure that this is real.", "Hello, world." };
    public DialogEvent[] events = { new() };
    public string npcName = "NPC";
    public float textSpeed = 0.05f;
    public bool canBeSkipped = true;
    public bool stopCharacterMovement = true;

    // Actual REAL settings (bear with me, this isnt optimal, but its WAYY practical.)
    private static DialogScriptable Instance; // Used for events, using "this" does not work while inside a class to reference the scriptable
    private MovementBehaviour charMovement;
    private bool hasDialogStarted;
    private bool canInteract;
    private int dialogIndex;

    public void OnEnable() { Instance = this; ResetScriptablePrivatesToDefaultState(); }

    // Starts the dialog with the NPC
    public void StartDialog(Character caller)
    {
        CreateInstance<DialogScriptable>();
        Debug.Log($"{dialog[dialogIndex]}\n{npcName}\n{dialogIndex}\n{hasDialogStarted}\n{name}");
        if (!canInteract) return;

        // Turns off the choices UI
        GameManager.Instance.gameUI.dialogChoices.ForEach(choice => choice.gameObject.SetActive(false));

        // Not the first time?
        if (hasDialogStarted) { ProceedChat(caller); return; }

        // Toggles the dialog box on
        GameManager.Instance.gameUI.ToggleDialogBox(true);
    
        // Resets the dialog box
        GameManager.Instance.gameUI.dialogName.text = npcName;
        GameManager.Instance.gameUI.dialogText.text = string.Empty;
        charMovement = caller.mov;
        hasDialogStarted = true;

        // Stops character movement
        if (stopCharacterMovement) DisallowCharacterControl();

        // Reads a line
        NextLine(caller);
    }

    private void ProceedChat(Character caller)
    {
        // Go to the next line of dialog
        if (GameManager.Instance.gameUI.dialogText.text == dialog[dialogIndex]) { dialogIndex++; NextLine(caller); return; }

        // Stop showing text and show it all instantly
        if (!canBeSkipped) return;
        GameManager.Instance.gameUI.dialogText.text = dialog[dialogIndex];
        caller.StopAllCoroutines();
    }

    // Changes dialog line to a next one or ends the conversation
    private void NextLine(Character caller)
    {
        // Goes to the next line of dialog
        if (dialogIndex < dialog.Length)
        {
            // Executes a dialog event
            foreach (DialogEvent ev in events) { if (ev.executeAtIndex == dialogIndex) ev.userDialog.Run(); }
            
            // Reads the line
            caller.StartCoroutine(ReadLine());
            return;
        }

        // Ends dialog and gives back movement to the player
        if (stopCharacterMovement) AllowCharacterControl();
        GameManager.Instance.gameUI.ToggleDialogBox(false);
        hasDialogStarted = false;
        dialogIndex = 0;
    }

    // Reads a line of text
    public IEnumerator ReadLine()
    {
        // Resets dialogbox text
        GameManager.Instance.gameUI.dialogText.text = string.Empty;

        // Draws every character on the dialogbox
        foreach (char c in dialog[dialogIndex])
        {
            GameManager.Instance.gameUI.dialogText.text += c;
            if (c.ToString() != "." && c.ToString() != ",") yield return new WaitForSecondsRealtime(textSpeed);
            else yield return new WaitForSecondsRealtime(textSpeed + 0.1f);
        }
    }

    // Change dialog scriptable (delegate or fucking whatever its called idk man)
    public void DelegateScriptable(DialogScriptable newDialog) {
        dialog = newDialog.dialog;
        events = newDialog.events;
        npcName = newDialog.npcName;
        textSpeed = newDialog.textSpeed;
        canBeSkipped = newDialog.canBeSkipped;
        stopCharacterMovement = newDialog.stopCharacterMovement;
        ResetScriptablePrivatesToDefaultState();
        if (!stopCharacterMovement) AllowCharacterControl();
    }

    // Removes the movement from the character
    private void DisallowCharacterControl() { charMovement.canMove = false; charMovement.canDash = false; }

    // Gives back movement to the character
    private void AllowCharacterControl() { charMovement.canMove = true; charMovement.canDash = true; }

    // Resets the private scriptable variables to its default state
    private void ResetScriptablePrivatesToDefaultState()
    {
        hasDialogStarted = false;
        canInteract = true;
        dialogIndex = 0;
    }

    // Dialog events
    [Serializable] public class DialogEvent {
        public DialogOptions userDialog = new();
        public int executeAtIndex = 0;
        // public bool executeAfterText = false;

        // Dialog user option event
        [Serializable] public class DialogOptions : EventAction
        {
            [SerializeField] public string[] options; // Yes, no, exit, etc. Max 4
            [SerializeField] public DialogScriptable[] responses; // Uses a new dialog scriptable (forking paths! (this is horrible), use the SAME scriptable (this) to ignore choices)
            public override void Run() {
                // Remove player interaction (except buttons)
                Instance.canInteract = false;
                
                // Disables all options
                foreach (GameManager.GameUI.Choice choice in GameManager.Instance.gameUI.dialogChoices)
                { choice.gameObject.SetActive(false); }

                // Sets the new options
                for (int i = 0; i < options.Length; i++)
                {
                    // Set the choices responses on/off
                    GameManager.Instance.gameUI.dialogChoices[i].gameObject.SetActive(true);

                    // Remove ALL listeners
                    GameManager.Instance.gameUI.dialogChoices[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

                    // Set the onclick attributes
                    // it gets the reference "i", we must pass it as a number.
                    // ok fuck it fuck pointers fuck everything just let me test
                    if (i == 0) GameManager.Instance.gameUI.dialogChoices[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Choice(0));
                    else if (i == 1) GameManager.Instance.gameUI.dialogChoices[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Choice(1));
                    else if (i == 2) GameManager.Instance.gameUI.dialogChoices[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Choice(2));
                    else if (i == 3) GameManager.Instance.gameUI.dialogChoices[i].gameObject.GetComponent<Button>().onClick.AddListener(() => Choice(3));
                    
                    // Set the text
                    GameManager.Instance.gameUI.dialogChoices[i].text.text = options[i];
                }

                // Enable the UI
                GameManager.Instance.gameUI.ToggleChoicesSubUI(true);
            }

            // User selects a choice, runs the scriptable response
            public void Choice(int choiceNum)
            {
                // Using player for now until i figure out a better caller
                Instance.canInteract = true;
                if (responses[choiceNum] == Instance) Instance.ProceedChat(GameManager.Instance.player);
                else { Instance.DelegateScriptable(responses[choiceNum]); Instance.StartDialog(GameManager.Instance.player); }
            }
        }

        // Required to have Run() or something idk
        [Serializable] public class EventAction {
            public bool enabled = false;
            public virtual void Run() { }
        }
    }
}
