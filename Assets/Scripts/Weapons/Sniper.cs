using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public SniperScriptable ws;

    private int bulletPenetration;

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

        // Add penetration
        tempBullet.GetComponent<SniperProjectile>().bulletPenetration = bulletPenetration;

        // Ignore collision
        Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());

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
        bulletPenetration = ws.bulletPenetration;
        
        SetWeaponSprite(weaponSprite);
    }
}
