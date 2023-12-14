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
        GameManager.Instance.bulletPool.transform);

        // Ignore collision
        Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());

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
        projectile = ws.projectile;
        
        SetWeaponSprite(weaponSprite);
    }
}
