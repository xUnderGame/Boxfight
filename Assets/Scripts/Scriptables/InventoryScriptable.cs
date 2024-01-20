using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Default Inventory", menuName = "Inventory Scriptable")]
public class InventoryScriptable : ScriptableObject
{
    public List<Powerup> activePowerups;
    public List<Weapon> weapons;
    public Weapon activeWeapon;
    [HideInInspector] public int weaponIndex;
    [HideInInspector] public readonly float swapWeaponsCD = 0.22f;
    [HideInInspector] public bool canSwapWeapons;
    [HideInInspector] public bool globalCanShoot;
    [HideInInspector] public bool globalCanMelee;

    // public void OnEnable() { ResetInventory(); }

    // Swaps current weapon
    public void SwapWeapon(PlayerMovement caller)
    {
        if (weapons.Count < 2 || !canSwapWeapons) return;
        caller.StartCoroutine(caller.cd.StartCooldown(swapWeaponsCD, result => canSwapWeapons = result, canSwapWeapons)); // Witchcraft.
        int oldIndex = weaponIndex;
        weaponIndex = MirrorWeaponIndex();

        // Swap weapons and enable/disable
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = weapons[weaponIndex];
        activeWeapon.gameObject.SetActive(true);
        weapons[weaponIndex].canShoot = true; // No matter what, re-allow shooting

        GameManager.Instance.gameUI.UpdateWeaponsUI(this, oldIndex);
    }

    // Picks up a weapon
    public void PickupWeapon() {
        // if (!GameManager.Instance.nearestInteractable || !GameManager.Instance.nearestInteractable.CompareTag("Weapon")) return;
        GameObject pickup = GameManager.Instance.nearestInteractable;

        // Buy weapon? No? Yes? Seems like a scam.
        if (pickup.transform.childCount > 0) {
            GameObject thePriceIsRight = pickup.transform.Find("InteractBuy").Find("Price").gameObject;

            // Do you have enough money?
            int price = int.Parse(thePriceIsRight.GetComponent<Text>().text);
            if (price >= JsonManager.Instance.userData.coins) return;

            // Deducts user coins (haha gd reference) and updates coins UI
            JsonManager.Instance.userData.coins -= price;
            GameManager.Instance.gameUI.UpdateCoinsUI();

            // Destroy "InteractBuy" object
            Destroy(thePriceIsRight.transform.parent.gameObject);
        }

        // Decides if it should pick up the weapon OR swap it with the currently equipped one
        if (weapons.Count >= weapons.Capacity) { ChangeWeapon(); return; }

        // Disables the collider
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
        if (!activeWeapon.canShoot || weapons.Count < 2) return;
        GameObject pickup = GameManager.Instance.nearestInteractable;

        // "Unequip"
        activeWeapon.transform.parent = GameManager.Instance.pickupPool.transform;
        activeWeapon.gameObject.GetComponent<Collider2D>().enabled = true;
        activeWeapon.tag = "Weapon";

        // Equip new weapon
        pickup.transform.parent = GameManager.Instance.playerObject.transform.Find("Weapons");
        activeWeapon = pickup.GetComponent<Weapon>();
        SetWeaponPosition(pickup);
        weapons[weaponIndex] = activeWeapon;
        activeWeapon.tag = "Equipped";

        Debug.Log($"Swapped active weapon with {pickup.name}");
        GameManager.Instance.nearestInteractable = null;
        GameManager.Instance.gameUI.UpdateWeaponsUI(this, MirrorWeaponIndex());
    }

    // Mirrors the weapon index (Only used for UI purposes rn)
    public int MirrorWeaponIndex() { return Convert.ToInt32(!Convert.ToBoolean(weaponIndex)); }

    // Resets the inventory to its default values
    public void ResetInventory()
    {
        activePowerups = new();
        weapons = new(capacity: 2);
        activeWeapon = null;
        canSwapWeapons = true;
        globalCanShoot = true;
        globalCanMelee = true;
        weaponIndex = 0;
    }

}
