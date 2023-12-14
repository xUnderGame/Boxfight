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
        activeWeapon = null;
        weaponIndex = 0;
    }

    // Swaps current weapon
    public void SwapWeapon()
    {
        if (weapons.Count < 2 || !activeWeapon.canShoot) return;
        int oldIndex = weaponIndex;
        weaponIndex = MirrorWeaponIndex();

        // Swap weapons and enable/disable
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = weapons[weaponIndex];
        activeWeapon.gameObject.SetActive(true);
        Debug.Log($"Weapon swapped with {activeWeapon.name}");
        GameManager.Instance.gameUI.UpdateWeaponsUI(this, oldIndex);
    }

    // Picks up a weapon
    public void PickupWeapon() {
        if (!GameManager.Instance.nearestPickup) return;

        // Decides if it should pick up the weapon OR swap it with the currently equipped one
        if (weapons.Count >= weapons.Capacity) { ChangeWeapon(); return; }

        // Disables the collider
        GameObject pickup = GameManager.Instance.nearestPickup;
        pickup.GetComponent<BoxCollider2D>().enabled = false;
        
        // Adds the weapon
        weapons.Add(pickup.GetComponent<Weapon>());
        pickup.tag = "Equipped";

        // Sets the new position of the weapon
        SetWeaponPosition(pickup);

        // Disables the newly collected weapon or sets the first gun you picked up as active
        if (activeWeapon) pickup.SetActive(false);
        else activeWeapon = weapons[0]; // Might cause problems...? (Does not for now)
        GameManager.Instance.gameUI.UpdateWeaponsUI(this);

        Debug.Log($"Picked up weapon {pickup.name}");
    }

    // Sets the weapon position where the player is
    private void SetWeaponPosition(GameObject pickup)
    {
        pickup.transform.parent = GameManager.Instance.playerObject.transform.Find("Weapons");
        pickup.transform.position = new Vector3(
            GameManager.Instance.playerObject.transform.position.x,
            GameManager.Instance.playerObject.transform.position.y,
            pickup.transform.position.z
        );
    }

    // Changes weapon with the one on the floor.
    public void ChangeWeapon()
    {
        if (!GameManager.Instance.nearestPickup || !activeWeapon.canShoot || weapons.Count < 2) return;
        GameObject pickup = GameManager.Instance.nearestPickup;

        // "Unequip"
        activeWeapon.transform.parent = GameManager.Instance.pickupPool.transform;
        activeWeapon.gameObject.GetComponent<Collider2D>().enabled = true;
        activeWeapon.tag = "Untagged";

        // Equip new weapon
        pickup.transform.parent = GameManager.Instance.playerObject.transform.Find("Weapons");
        activeWeapon = pickup.GetComponent<Weapon>();
        SetWeaponPosition(pickup);
        weapons[weaponIndex] = activeWeapon;
        activeWeapon.tag = "Equipped";

        Debug.Log($"Swapped active weapon with {pickup.name}");
        GameManager.Instance.nearestPickup = null;
        GameManager.Instance.gameUI.UpdateWeaponsUI(this, MirrorWeaponIndex());
    }

    // Mirrors the weapon index (Only used for UI purposes rn)
    public int MirrorWeaponIndex() { return Convert.ToInt32(!Convert.ToBoolean(weaponIndex)); }

}
