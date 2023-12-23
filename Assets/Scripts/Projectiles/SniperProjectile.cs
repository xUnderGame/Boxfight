using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperProjectile : Projectile
{
    private Weapon weapon;
    private int currentDamage;
    [HideInInspector] public int bulletPenetration = 0;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Awake()
    {
        // Active weapon...
        weapon = GameManager.Instance.player.inv.activeWeapon;
        
        // Projectile info
        gameObject.name = "Sniper Bullet";
        currentDamage = weapon.damage;
        bulletSpeed = 22;

    }

    public override void LateUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(bulletSpeed * Time.deltaTime * transform.right); }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(currentDamage, gameObject);

        // Can the bullet penetrate the enemy?
        if (bulletPenetration > 0) { bulletPenetration--; currentDamage /= 2; }
        else Destroy(gameObject);
    }
}
