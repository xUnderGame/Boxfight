using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InventoryScript", menuName = "InventoryScriptable")]
public class InventoryScriptable : ScriptableObject
{
    public List<Powerup> activePowerups;
    [HideInInspector] public List<Weapon> weapons;
    [HideInInspector] public Weapon activeWeapon;
    [HideInInspector] public int weaponIndex = 0;

    // Swaps current weapon (MUST change later)
    public void SwapWeapon() { activeWeapon = weapons[weaponIndex]; }

    // Picks up a weapon
    public void PickupWeapon(GameObject gameObject) {
        // if (weapons.Count >= weapons.Capacity) return;

        // Adds the weapon!
        weapons.Add(gameObject.GetComponent<Weapon>());
        activeWeapon = weapons[0]; // HARDCODED
        Debug.Log("added");
    }
}
