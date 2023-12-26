using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character, IInteractable
{
    public DialogScriptable dialogScriptable;

    // Starts dialog once interacted (PLAYER ONLY)
    public void Interact(GameObject interactionSource) { if (interactionSource == GameManager.Instance.playerObject) dialogScriptable.StartDialog(GameManager.Instance.playerObject.GetComponent<Character>()); }

    // Checking if the player is near the NPC and assigning it to nearest interactable
    void OnTriggerStay2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestInteractable == null) GameManager.Instance.nearestInteractable = gameObject; }
    void OnTriggerExit2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestInteractable == gameObject) GameManager.Instance.nearestInteractable = null; }
}
