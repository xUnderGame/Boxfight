using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [HideInInspector] public static DialogManager Instance;

    public DialogScriptable loadedDial;
    public string[] dialog;
    public DialogEvent[] events = { new() };
    public string npcName = "NPC";
    public float textSpeed = 0.05f;
    public bool canBeSkipped = true;
    public bool stopCharacterMovement = true;

    private MovementBehaviour charMovement;
    private bool ignoreNewChatSource;
    private bool hasDialogStarted;
    private bool canInteract;
    private int dialogIndex;

    public void Awake()
    {
        // DialogManager Instance
        if (Instance) { Destroy(gameObject); return; }
        else Instance = this;
        ResetPrivatesToDefaultState();
    }

    // Starts the dialog with the NPC
    public void StartDialog(DialogScriptable chat, Character caller = null)
    {
        if (!canInteract) return;

        // Turns off the choices UI
        TurnOffDialogChoices();

        // Should we change/load the new scriptable?
        if (!loadedDial || chat != loadedDial && !ignoreNewChatSource) DelegateScriptable(chat);

        // Not the first time?
        if (hasDialogStarted) { ProceedChat(); return; }

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
        NextLine();
    }

    private void ProceedChat(bool forceNext = false)
    {
        // Go to the next line of dialog
        if (GameManager.Instance.gameUI.dialogText.text == dialog[dialogIndex] || forceNext) { dialogIndex++; NextLine(); return; }

        // Stop showing text and show it all instantly
        if (!canBeSkipped) return;
        ShowEntireLine();
    }

    // Changes dialog line to a next one or ends the conversation
    private void NextLine()
    {
        // Goes to the next line of dialog
        if (dialogIndex < dialog.Length)
        {
            // Executes a dialog event
            foreach (DialogEvent ev in events) { if (ev.executeAtIndex == dialogIndex) ev.userDialog.Run(); }
            
            // Reads the line
            StartCoroutine(ReadLine());
            return;
        }

        // Ends dialog and gives back movement to the player
        if (stopCharacterMovement) AllowCharacterControl();
        GameManager.Instance.gameUI.ToggleDialogBox(false);
        ResetPrivatesToDefaultState();
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
    public void DelegateScriptable(DialogScriptable newDialog, bool doDefaults = false) {
        dialog = newDialog.dialog;
        events = newDialog.events;
        npcName = newDialog.npcName;
        textSpeed = newDialog.textSpeed;
        canBeSkipped = newDialog.canBeSkipped;
        stopCharacterMovement = newDialog.stopCharacterMovement;
        loadedDial = newDialog;

        // Reset to defaults?
        if (!doDefaults) return;
        if (!stopCharacterMovement) AllowCharacterControl();
        ResetPrivatesToDefaultState();
        ignoreNewChatSource = true;
    }

    // Removes the movement from the character
    private void DisallowCharacterControl()
    {
        GameManager.Instance.player.inv.globalCanShoot = false; // Using player instead of caller!!
        GameManager.Instance.player.inv.globalCanMelee = false; // Using player instead of caller!!
        charMovement.canMove = false;
        charMovement.canDash = false;
        charMovement.chainDash = false;
    }

    // Gives back movement to the character
    private void AllowCharacterControl()
    {
        GameManager.Instance.player.inv.globalCanShoot = true; // Using player instead of caller!!
        GameManager.Instance.player.inv.globalCanMelee = true; // Using player instead of caller!!
        charMovement.canMove = true;
        charMovement.canDash = true;
        charMovement.chainDash = true;
    }

    // Resets the private scriptable variables to its default state
    private void ResetPrivatesToDefaultState()
    {
        hasDialogStarted = false;
        canInteract = true;
        dialogIndex = 0;
        ignoreNewChatSource = false;
    }

    // Instantly shows the entire line
    private void ShowEntireLine()
    {
        GameManager.Instance.gameUI.dialogText.text = dialog[dialogIndex];
        StopAllCoroutines();
    }

    // Disables all dialog choices
    private void TurnOffDialogChoices()
    {
        GameManager.Instance.gameUI.dialogChoices.ForEach(choice => choice.gameObject.SetActive(false));
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
                // Allows player interaction
                Instance.canInteract = true;

                // Shows the next line of dialog
                if (responses[choiceNum] == Instance.loadedDial)
                {
                    Instance.TurnOffDialogChoices();
                    Instance.ShowEntireLine();
                    Instance.ProceedChat(true);
                }

                // Using player as caller for now, changes scriptable to a new one
                else
                {
                    Instance.ShowEntireLine();
                    Instance.DelegateScriptable(responses[choiceNum], true);
                    Instance.StartDialog(responses[choiceNum], GameManager.Instance.player);
                }
            }
        }

        // Required to have Run() or something idk
        [Serializable] public class EventAction {
            public bool enabled = false;
            public virtual void Run() { }
        }
    }
}
