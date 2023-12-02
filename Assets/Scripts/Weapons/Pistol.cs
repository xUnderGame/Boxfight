using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public PistolScriptable ws;

    public override void Shoot()
    {
        Instantiate(projectile,
        gameObject.transform.position,
        Quaternion.identity,
        gameObject.transform);
    }

    public override void LoadScriptable()
    {
        energyCost = ws.energyCost;
        damage = ws.damage;
        firingSpeed = ws.firingSpeed;
    }
}
