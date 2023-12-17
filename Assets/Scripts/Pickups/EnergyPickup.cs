using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour, IInteractable
{
    private int recoveryAmount = 1;

    public void Awake()
    {
        // Sets a random number of energy to recover
        recoveryAmount = Random.Range(1, 6); // 1-5

        // Sets scale and color depending on recovery
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (recoveryAmount)
        {
            // Big recovery
            case >= 4:
                sr.color = new Color(
                    sr.color.r,
                    sr.color.g - 0.25f,
                    sr.color.b,
                    sr.color.a
                );
                transform.localScale = new Vector3(
                    transform.localScale.x + 0.35f,
                    transform.localScale.y + 0.35f,
                    transform.localScale.z
                );
                break;

            // Medium recovery
            case >= 2:
                sr.color = new Color(
                    sr.color.r,
                    sr.color.g - 0.50f,
                    sr.color.b,
                    sr.color.a
                );
                transform.localScale = new Vector3(
                    transform.localScale.x + 0.15f,
                    transform.localScale.y + 0.15f,
                    transform.localScale.z
                );
                break;

            // Default, no changes
            default:
                break;
        }
    }

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
