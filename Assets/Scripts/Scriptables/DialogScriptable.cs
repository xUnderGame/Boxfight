using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Default Dialog", menuName = "Dialog Scriptable")]
public class DialogScriptable : ScriptableObject
{
    public string[] dialog = { "Hello, world.", "Programmed to work and not to feel.", "Not even sure that this is real.", "Hello, world." };
    public DialogEvent[] events = { new() };
    public string npcName = "NPC";
    public float textSpeed = 0.05f;
    public bool canBeSkipped = true;
    public bool stopCharacterMovement = true;

    private MovementBehaviour charMovement;
    private bool hasDialogStarted;
    private int dialogIndex;

    public void OnEnable() { ResetScriptablePrivatesToDefaultState(); }

    // Starts the dialog with the NPC
    public void StartDialog(Character caller)
    {
        if (hasDialogStarted) { ProceedChat(caller); return; }

        // Toggles the dialog box on
        GameManager.Instance.gameUI.ToggleDialogBox();
    
        // Resets the dialog box
        GameManager.Instance.gameUI.dialogName.text = npcName;
        GameManager.Instance.gameUI.dialogText.text = string.Empty;
        charMovement = caller.mov;
        hasDialogStarted = true;

        // Stops player movement
        if (stopCharacterMovement) { charMovement.canMove = false; charMovement.canDash = false; }

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

            caller.StartCoroutine(ReadLine());
            return;
        }

        // Ends dialog and gives back movement to the player
        if (stopCharacterMovement) { charMovement.canMove = true; charMovement.canDash = true; }
        GameManager.Instance.gameUI.ToggleDialogBox();
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
        dialogIndex = 0;
    }

    // Toggles the choices sub-ui.
    public void ToggleChoicesSubUI() { }

    // Resets the private scriptable variables to its default state (false, 0)
    private void ResetScriptablePrivatesToDefaultState()
    {
        hasDialogStarted = false;
        dialogIndex = 0;
    }

    // Dialog events
    [Serializable] public class DialogEvent {
        public DialogOptions userDialog = new();
        public int executeAtIndex = 0;
        // public bool executeAfterText = false;

        // Executes event
        public void Execute() { userDialog.Run(); }

        // Dialog user option event
        [Serializable] public class DialogOptions : EventAction
        {
            [SerializeField] public string[] options; // Yes, no, exit
            [SerializeField] public DialogScriptable[] responses; // Uses a new dialog scriptable (forking paths! (this is horrible), use the SAME scriptable (this) to ignore choices)
            public override void Run() {
                // ToggleChoicesSubUI();
            }
        }

        // Required to have Run() or something idk
        [Serializable] public class EventAction {
            public bool enabled = false;
            public virtual void Run() { }
        }
    }
}
