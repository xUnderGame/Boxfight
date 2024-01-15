using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed = 0f;
    [HideInInspector] public int bulletDamage = 1;
    [HideInInspector] public float ttl = -1f;
    public void Awake() { StartCoroutine(DespawnTimer()); }
    public abstract void LateUpdate();
    public abstract void Travel();
    public IEnumerator DespawnTimer()
    {
        while (ttl == -1f) { yield return new WaitForSeconds(0.01f); }
        yield return new WaitForSeconds(ttl);
        Destroy(gameObject);
    }
    public abstract void OnTriggerEnter2D(Collider2D hit);
    public void OnBecameInvisible() { Destroy(gameObject); } // Destroy itself upon leaving the screen space
}
