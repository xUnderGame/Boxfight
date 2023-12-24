using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Default Dialog", menuName = "Dialog Scriptable")]
public class DialogScriptable : ScriptableObject
{
    public string[] dialog = { "Hello, world.", "Programmed to work and not to feel.", "Not even sure that this is real.", "Hello, world." };
    public string npcName = "NPC";
    public float textSpeed = 0.05f;
    public bool canBeSkipped = true;
    public bool stopCharacterMovement = true;

    private MovementBehaviour charMovement;
    private bool hasDialogStarted;
    private int dialogIndex;

    public void OnEnable()
    {
        hasDialogStarted = false;
        dialogIndex = 0;
    }

    // Starts the dialog with the NPC
    public void StartDialog(Character caller)
    {
        if (hasDialogStarted) { UserActions(caller); return; }

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

    private void UserActions(Character caller)
    {
        // Go to the next line of dialog
        if (GameManager.Instance.gameUI.dialogText.text == dialog[dialogIndex]) { NextLine(caller); return; }

        // Stop showing text and show it all instantly
        if (!canBeSkipped) return;
        GameManager.Instance.gameUI.dialogText.text = dialog[dialogIndex];
        caller.StopAllCoroutines();
    }

    // Changes dialog line to a next one or ends the conversation
    private void NextLine(Character caller)
    {
        // Goes to the next line of dialog
        if (dialogIndex + 1 < dialog.Length)
        {
            dialogIndex++;
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
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }
}
