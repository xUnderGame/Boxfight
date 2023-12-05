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
        Instantiate(projectile,
        gameObject.transform.position,
        Quaternion.identity,
        gameObject.transform);

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
