using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour, IInteractable
{
    private int recoveryAmount = 1;

    // Sets a random number of energy to recover
    public void Awake() { recoveryAmount = Random.Range(1, 6); }

    // Player picks up the energy bit
    public void Interact()
    {
        // Updates energy
        GameManager.Instance.player.currentEnergy += recoveryAmount;
        if (GameManager.Instance.player.currentEnergy > GameManager.Instance.player.maxEnergy)
        { GameManager.Instance.player.currentEnergy = GameManager.Instance.player.maxEnergy; }

        // Updates UI and self destructs
        GameManager.Instance.gameUI.UpdateEnergyUI();
        Destroy(gameObject);
    }
}
