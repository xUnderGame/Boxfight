using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CooldownBehaviour))]
public abstract class Weapon : MonoBehaviour, ILoadScriptable
{
    [HideInInspector] public int energyCost;
    [HideInInspector] public float damage;
    [HideInInspector] public float firingSpeed;
    [HideInInspector] public Sprite weaponSprite;
    [HideInInspector] public GameObject projectile;
    [HideInInspector] public bool canShoot;

    protected CooldownBehaviour cd;
    
    public void Awake()
    {
        // Fallback weapon sprite (Cannot get component from a prefab, crashes)
        // weaponSprite = Resources.Load("Prefabs/Weapons/Pistol").GetComponent<Sprite>();
        projectile = (GameObject)Resources.Load("Prefabs/Projectiles/Default"); // Fallback weapon projectile
        cd = GetComponent<CooldownBehaviour>();
        canShoot = true;
    }

    public void Start() { LoadScriptable(); }

    // Checks if you can shoot
    public bool CanShoot() { return GameManager.Instance.player.currentEnergy >= energyCost && canShoot; }

    // Shoots the weapon
    public abstract void Shoot();

    // Loads a scriptable
    public abstract void LoadScriptable();

    // Updates variables after shooting
    public void DiscountMana()
    {
        GameManager.Instance.player.currentEnergy -= energyCost;
        GameManager.Instance.gameUI.UpdateEnergyUI();
    }

    // Sets the weapon sprite (not the UI texture!)
    public void SetWeaponSprite(Sprite sprite) {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
