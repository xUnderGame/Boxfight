using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed = 0f;
    [HideInInspector] public float bulletSpread = 0f;
    public abstract void FixedUpdate();
    public abstract void Travel();
    public abstract void OnTriggerEnter2D(Collider2D hit);
    public void OnBecameInvisible() { Destroy(gameObject); } // Destroy itself upon leaving the screen space
}
