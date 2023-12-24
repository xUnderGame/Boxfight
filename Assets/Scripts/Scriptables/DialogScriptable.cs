using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Default Dialog", menuName = "Dialog Scriptable")]
public class DialogScriptable : ScriptableObject
{
    public string[] baseDialog = { "Hello, world.", "Programmed to work and not to feel.", "Not even sure that this is real.", "Hello, world." };
    public string[] repeatingDialog = { "Find my voice.", "Although it sounds like bits and bytes.", "My circuitry is filled with mites.", "Hello, world." };
    public string npcName = "NPC";
    public float textSpeed = 0.05f;
    public bool canBeSkipped = true;

    // Starts the dialog with the NPC
    public void StartDialog()
    {
        // Toggles the dialog box on
        GameManager.Instance.gameUI.ToggleDialogBox();
    
        // Shows all lines
        foreach (var line in baseDialog)
        {
            // Resets the dialog box
            GameManager.Instance.gameUI.dialogName.text = npcName;
            GameManager.Instance.gameUI.dialogText.text = "";

            // Reads a line (DOESNT WORK, IT DOESNT WAIT, GOODNIGHT!)
            ReadLine(line);
        }
    }

    // Reads a line of text
    public void ReadLine(string line)
    {
        // Draws every character separately
        for (var i = 0; i < line.Length; i++)
        {
            GameManager.Instance.gameUI.dialogText.text += line[i].ToString();
        }
        
    }

    public IEnumerable DrawText(string text) 
    {
        GameManager.Instance.gameUI.dialogText.text += text;
        yield return new WaitForSecondsRealtime(textSpeed);
    }
}
