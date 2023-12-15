using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectile : Projectile
{
    private Weapon weapon;
    private readonly float fixedSpeedDecrease = 0.9f;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Awake()
    {
        // Projectile info
        gameObject.name = "Shotgun Pellet";
        bulletSpeed = 1;
        bulletSpread = 2.5f;

        // Active weapon...
        weapon = GameManager.Instance.player.inv.activeWeapon;
    }

    public override void FixedUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(transform.right * (bulletSpeed - fixedSpeedDecrease)); }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(weapon.damage);
        Destroy(gameObject);
    }
}
