using UnityEngine;

public class ShotgunProjectile : Projectile
{
    [HideInInspector] public bool shotByPlayer;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Start()
    {
        // Projectile info
        gameObject.name = "Shotgun Pellet";
        bulletSpeed = 5;
    }

    public override void LateUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(bulletSpeed * Time.deltaTime * transform.right); }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Enemy") && !shotByPlayer) return;
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(bulletDamage, gameObject);
        Destroy(gameObject);
    }
}
