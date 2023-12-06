using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public PistolScriptable ws;

    public override void Shoot()
    {
        if (!CanShoot()) return;

        // Shoot the bullet
        GameObject tempBullet = Instantiate(projectile,
        gameObject.transform.position,
        Quaternion.identity,
        gameObject.transform);
        Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
        tempBullet.transform.parent = GameObject.Find("Bullet Pool").transform;
        tempBullet.GetComponent<Projectile>().bulletSpeed = projectileSpeed;

        // Discount the player mana and start cooldown coroutine
        StartCoroutine(cd.StartCooldown(firingSpeed, result => canShoot = result, canShoot));
        DiscountMana();
    }

    public override void LoadScriptable()
    {
        weaponSprite = ws.weaponSprite;
        energyCost = ws.energyCost;
        damage = ws.damage;
        firingSpeed = ws.firingSpeed;
        projectileSpeed = ws.projectileSpeed;
        
        SetWeaponSprite(weaponSprite);
    }
}
