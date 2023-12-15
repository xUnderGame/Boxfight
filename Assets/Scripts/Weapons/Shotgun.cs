using System;
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
        int offset = -20;

        // Shoot the bullets!
        for (int i = 0; i < bulletsPerShot; i++)
        {
            var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
            float ang = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + offset;

            GameObject tempBullet = Instantiate(projectile,
            gameObject.transform.position,
            Quaternion.Euler(new Vector3(0, 0, ang - 90f)),
            GameManager.Instance.bulletPool.transform);

            // Ignore collision
            Physics2D.IgnoreCollision(transform.root.GetComponent<Collider2D>(), tempBullet.GetComponent<Collider2D>());
            offset += 10;
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
