using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public InventoryScriptable inv;
    public float currentEnergy;
    public float maxEnergy;

    public void Start() { UpdateEnergyUI(); UpdateHealthUI(); }

    // Updates the energy UI
    public void UpdateEnergyUI() { GameManager.Instance.gameUI.manaValue.text = $"{currentEnergy}/{maxEnergy}"; }

    // Updates the HP UI
    public void UpdateHealthUI() { GameManager.Instance.gameUI.hpValue.text = $"{currentHP}/{maxHP}"; }
}
