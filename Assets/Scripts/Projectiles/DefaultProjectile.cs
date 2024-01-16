using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : Projectile
{

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Start()
    {
        // Projectile info
        gameObject.name = "Default Projectile";
        bulletSpeed = 6.5f;
    }

    public override void LateUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(bulletSpeed * Time.deltaTime * transform.right); }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (!hit.CompareTag("Enemy"))
        {
            if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(bulletDamage, gameObject);
            Destroy(gameObject);
        }
    }
}
