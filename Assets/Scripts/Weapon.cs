using System.Collections;
using System.Collections.Generic;
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

    // Loads the weapon scriptable upon starting, not enabling!
    public void Start() { LoadScriptable(); }

    // Point weapon at cursor position
    public void Update() { if (gameObject.CompareTag("Equipped")) PointWeaponAtCursor(); }

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
    public void SetWeaponSprite(Sprite sprite) { gameObject.GetComponent<SpriteRenderer>().sprite = sprite; }

    // Points weapon to cursor
    public void PointWeaponAtCursor()
    {
        Vector3 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip sprite
        SpriteRenderer flip = gameObject.GetComponent<SpriteRenderer>();
        if (gameObject.transform.rotation.z > 0.68f || gameObject.transform.rotation.z < -0.68f) flip.flipY = true;
        else flip.flipY = false;
    }

    // Points weapon at player
    public void PointWeaponAtPlayer()
    {
        Vector3 lookDir = Camera.main.ScreenToWorldPoint(GameManager.Instance.playerObject.transform.position) - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Checking if the player is near the swappable weapon
    void OnTriggerStay2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestPickup == null && !gameObject.CompareTag("Equipped")) GameManager.Instance.nearestPickup = gameObject; }
    void OnTriggerExit2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestPickup == gameObject && !gameObject.CompareTag("Equipped")) GameManager.Instance.nearestPickup = null; }
}
