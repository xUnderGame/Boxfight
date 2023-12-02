using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryScript", menuName = "InventoryScriptable")]
public class InventoryScriptable : ScriptableObject
{
    public List<Powerup> activePowerups;
    public List<Weapon> weapons;
    public Weapon activeWeapon;
    [HideInInspector] public int weaponIndex;

    // Reset the inventory scriptable every time the game is run
    public void OnEnable()
    {
        activePowerups = new();
        weapons = new();
        activeWeapon = null; // Change with melee weapon
        weaponIndex = 0;
    }


    // Swaps current weapon
    public void SwapWeapon()
    {
        weaponIndex = 0; // CHANGE LATER, WONT SWAP.
        activeWeapon = weapons[weaponIndex];
        Debug.Log($"Weapon swapped with {activeWeapon.name}");
    }

    // Picks up a weapon
    public void PickupWeapon(GameObject gameObject) {
        // if (weapons.Count >= weapons.Capacity) return;

        // Adds the weapon and swaps to it!
        weapons.Add(gameObject.GetComponent<Weapon>());
        // SwapWeapon();
        Debug.Log($"Weapon added!");
    }
}
