using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    // private readonly WeaponScriptable ws;
    private int bulletsPerShot;
    private float bulletSpread;

    public override void Shoot()
    {
        Instantiate(projectile, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    }

    public override void LoadScriptable()
    {
        // energyCost = ws.energyCost;
        // damage = ws.damage;
        // firingSpeed = ws.firingSpeed;
        // bulletsPerShot = ws.bulletsPerShot;
        // bulletSpread = ws.bulletSpread;
    }
}
