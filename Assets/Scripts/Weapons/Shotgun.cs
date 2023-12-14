using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public ShotgunScriptable ws;
    private int bulletsPerShot;

    public override void Shoot()
    {
        if (!CanShoot()) return;

        // Shoot the bullets!
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject tempBullet = Instantiate(projectile,
            gameObject.transform.position,
            Quaternion.identity,
            GameManager.Instance.bulletPool.transform);
            
            // Ignore collision
            Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
        }

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
        bulletsPerShot = ws.bulletsPerShot;
        projectile = ws.projectile;

        SetWeaponSprite(weaponSprite);
    }
}
