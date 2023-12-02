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
        weapons = new(capacity: 2);
        activeWeapon = null; // Might cause problems in the future?
        weaponIndex = 0;
    }


    // Swaps current weapon
    public void SwapWeapon()
    {
        if (weapons.Count < 2 || !activeWeapon.canShoot) return;
        weaponIndex = Convert.ToInt32(!Convert.ToBoolean(weaponIndex));

        // Swap weapons and enable/disable
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = weapons[weaponIndex];
        activeWeapon.gameObject.SetActive(true);
        Debug.Log($"Weapon swapped with {activeWeapon.name}");
    }

    // Picks up a weapon
    public bool PickupWeapon(GameObject pickup) {
        if (weapons.Count >= weapons.Capacity) return false;

        // Adds the weapon
        weapons.Add(pickup.GetComponent<Weapon>());
        pickup.tag = "Equipped";

        // Sets the new position of the weapon
        pickup.transform.parent = GameManager.Instance.playerObject.transform.Find("Weapons");
        pickup.transform.position = new Vector3(
            GameManager.Instance.playerObject.transform.position.x,
            GameManager.Instance.playerObject.transform.position.y,
            pickup.transform.position.z
        );
        Debug.Log($"Picked up weapon {pickup.name}");

        // Disables the newly collected weapon or sets the first gun you picked up as active
        if (activeWeapon) pickup.SetActive(false);
        else activeWeapon = weapons[0]; // Might cause problems...? (Does not for now)
        GameManager.Instance.gameUI.UpdateWeaponsUI(this);

        return true;
    }
}
