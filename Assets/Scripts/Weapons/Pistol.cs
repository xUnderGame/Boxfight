using UnityEngine;

public class Pistol : Weapon
{
    public PistolScriptable ws;

    public override void Shoot(Vector2 direction)
    {
        if (!CanShoot()) return;

        // Defines the angle
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg / 2;

        // Shoot the bullet
        GameObject tempBullet = Instantiate(projectile,
        gameObject.transform.position,
        Quaternion.Euler(new Vector3(0, 0, ang)),
        GameManager.Instance.bulletPool.transform);

        // Ignore collision
        Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());

        // Set projectile damage
        DefaultProjectile gunProjectile = tempBullet.GetComponent<DefaultProjectile>();
        gunProjectile.bulletDamage = damage;
        gunProjectile.ttl = ws.timeToLive;
        if (transform.root.CompareTag("Player")) gunProjectile.shotByPlayer = true;

        // Discount the player mana and start cooldown coroutine
        StartCoroutine(cd.StartCooldown(firingSpeed, result => canShoot = result, canShoot));
        if (transform.root.CompareTag("Player")) DiscountMana();
    }

    public override void LoadScriptable()
    {
        weaponSprite = ws.weaponSprite;
        energyCost = ws.energyCost;
        damage = ws.damage;
        firingSpeed = ws.firingSpeed;
        projectile = ws.projectile;
        
        SetWeaponSprite(weaponSprite);
    }
}
