using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public InventoryScriptable inv;
    public float currentEnergy;
    public float maxEnergy;

    public void Start()
    {
        GameManager.Instance.gameUI.UpdateEnergyUI();
        GameManager.Instance.gameUI.UpdateHealthUI();
    }
}
