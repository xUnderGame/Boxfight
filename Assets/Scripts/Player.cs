using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public InventoryScriptable inv;
    public float currentEnergy;
    public float maxEnergy;
    public List<int> idRoomsVisited = new();

    public void Start()
    {
        GameManager.Instance.gameUI.UpdateEnergyUI();
        GameManager.Instance.gameUI.UpdateHealthUI();
        GameManager.Instance.gameUI.UpdateCoinsUI();
    }

    // Hurt player
    public override void Hurt(int damage, GameObject damageSource)
    {
        base.Hurt(damage, damageSource);
        GameManager.Instance.gameUI.UpdateHealthUI();

        // Is HP below 0?
        if (currentHP <= 0) Kill();
    }

    // Kill player
    public override void Kill()
    {
        GameManager.Instance.gameUI.goText.text = GameManager.Instance.gameUI.goMessages[Random.Range(0, GameManager.Instance.gameUI.goMessages.Length)];
        GameManager.Instance.gameUI.ToggleGameOverUI(true);
        GameManager.Instance.playerObject.GetComponent<Collider2D>().enabled = false;
        GameManager.Instance.player.inv.globalCanShoot = false;
        GameManager.Instance.player.inv.canSwapWeapons = false;
        GameManager.Instance.player.inv.globalCanMelee = false;
        GameManager.Instance.player.mov.canMove = false;
        GameManager.Instance.player.mov.chainDash = false;
        GameManager.Instance.player.mov.canDash = false;
    }
}
