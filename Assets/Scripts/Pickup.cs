using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public void Interact() {
        Debug.Log("Hello there");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        GameManager.Instance.playerObject.GetComponent<Player>().inv.PickupWeapon(gameObject);
    }
}
