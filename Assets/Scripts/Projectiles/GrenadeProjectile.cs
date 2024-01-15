using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : Projectile
{
    [HideInInspector] public float shoveForce;
    [HideInInspector] public bool shotByPlayer;
    private Coroutine tick;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Start()
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
        yield return new WaitForSeconds(ttl - 0.25f);
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
        if (hit.TryGetComponent(out Rigidbody2D shove)) Shove(shove, hit);

        // Custom hit handler for player's melee attack (very fun)
        if (hit.transform.root.CompareTag("Player") && !hit.CompareTag("Player")) { StartCoroutine(Explode()); }
    }

    public void Shove(Rigidbody2D rb, Collider2D hit) {
        rb.AddForce((transform.position - hit.transform.position).normalized * shoveForce, ForceMode2D.Impulse);
    }
}
