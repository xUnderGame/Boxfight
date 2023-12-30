using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : Projectile
{
    private Weapon weapon;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Awake()
    {
        // Projectile info
        gameObject.name = "Default Projectile";
        bulletSpeed = 6.5f;

        // Active weapon...
        // I forgot this has to be "generic" for every weapon
        // wtf do i do
        // will have to change how projectiles/weapons shoot, for now,
        // enemies will have "null" as damage every time it collisions with something
        // which is bad.
        weapon = GameManager.Instance.player.inv.activeWeapon;
    }

    public override void LateUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(bulletSpeed * Time.deltaTime * transform.right); }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        // weapon.damage is using the wrong damage value for enemy weapons!!!
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(weapon.damage, gameObject);
        Destroy(gameObject);
    }
}
