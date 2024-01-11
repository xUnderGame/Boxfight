using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : Projectile
{
    [HideInInspector] public float shoveForce;
    [HideInInspector] public bool shotByPlayer;
    private readonly float bulletTimer = 2.5f;
    private Coroutine tick;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Awake()
    {
        tick = StartCoroutine(BulletTimer());

        // Projectile info
        gameObject.name = "Grenade Projectile";
        bulletSpeed = 2.2f;
    }

    public override void LateUpdate() { Travel(); }

    // Makes the trojectile travel.
    public override void Travel() { transform.Translate(bulletSpeed * Time.deltaTime * transform.right); }

    // Starts the timer for the bullet to explode
    public IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(bulletTimer);
        StartCoroutine(Explode());
    }

    // Stop projectile
    public void OnCollisionEnter2D(Collision2D col)
    {
        StopCoroutine(tick);
        StartCoroutine(Explode());
    }

    // Explodes the projectile and destroys itself after a moment
    public IEnumerator Explode()
    {
        gameObject.transform.Find("DamageArea").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    // When the projectile hits something...
    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable) && !(shotByPlayer && hit.transform.root.CompareTag("Player"))) damageable?.Hurt(bulletDamage, gameObject);

        // Shove enemy
        if (hit.TryGetComponent(out Rigidbody2D shove)) {
            shove.AddForce((transform.position - hit.transform.position).normalized * shoveForce, ForceMode2D.Impulse);
        }
    }
}
