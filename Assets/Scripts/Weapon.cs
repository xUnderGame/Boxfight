using UnityEngine;

[RequireComponent(typeof(CooldownBehaviour))]
public abstract class Weapon : MonoBehaviour, ILoadScriptable
{
    [HideInInspector] public int energyCost;
    [HideInInspector] public int damage;
    [HideInInspector] public float firingSpeed;
    [HideInInspector] public Sprite weaponSprite;
    [HideInInspector] public GameObject projectile;
    [HideInInspector] public bool canShoot;
    [HideInInspector] public AudioSource audioSource;

    protected CooldownBehaviour cd;
    
    public void Awake()
    {
        // Fallback weapon sprite (Cannot get component from a prefab, crashes)
        // weaponSprite = Resources.Load("Prefabs/Weapons/Pistol").GetComponent<Sprite>();
        projectile = (GameObject)Resources.Load("Prefabs/Projectiles/Default"); // Fallback weapon projectile
        cd = GetComponent<CooldownBehaviour>();
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
    }

    // Loads the weapon scriptable upon starting, not enabling!
    public void Start() { LoadScriptable(); }

    // Point weapon at cursor position
    public void Update() { if (gameObject.CompareTag("Equipped")) PointWeaponAtCursor(); }

    // Checks if you can shoot
    public bool CanShoot()
    {
        if (!transform.parent.parent.CompareTag("Player")) return canShoot;
        return GameManager.Instance.player.currentEnergy >= energyCost && canShoot;
    }

    // Shoots the weapon
    public abstract void Shoot(Vector2 direction);

    // Loads a scriptable
    public abstract void LoadScriptable();

    // Updates variables after shooting
    public void DiscountMana()
    {
        GameManager.Instance.player.currentEnergy -= energyCost;
        GameManager.Instance.gameUI.UpdateEnergyUI();
    }

    // Sets the weapon sprite (not the UI texture!)
    public void SetWeaponSprite(Sprite sprite, Color color)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
    }

    // Points weapon to cursor
    public void PointWeaponAtCursor()
    {
        Vector3 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        FlipSprite();
    }

    // Points weapon at player
    public void PointWeaponAtPlayer()
    {
        Vector3 lookDir = GameManager.Instance.playerObject.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        FlipSprite();
    }

    public void FlipSprite()
    {
        // Flip sprite
        SpriteRenderer flip = gameObject.GetComponent<SpriteRenderer>();
        if (transform.localEulerAngles.z > 90f && transform.localEulerAngles.z < 270f) flip.flipY = true;
        else flip.flipY = false;
    }

    // Checking if the player is near the swappable weapon
    void OnTriggerStay2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestInteractable == null && !gameObject.CompareTag("Equipped")) GameManager.Instance.nearestInteractable = gameObject; }
    void OnTriggerExit2D(Collider2D other) { if (other.gameObject.name == "Pickup Area" && GameManager.Instance.nearestInteractable == gameObject && !gameObject.CompareTag("Equipped")) GameManager.Instance.nearestInteractable = null; }
}
