using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public void Interact() {
        // Picks up weapon and disables collider if it succeeds
        if (GameManager.Instance.playerObject.GetComponent<Player>().inv.PickupWeapon(gameObject))
        { gameObject.GetComponent<BoxCollider2D>().enabled = false; }
    }
}
