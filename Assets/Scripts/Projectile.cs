using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    public abstract void FixedUpdate();
    public abstract void Travel();
    public abstract void OnTriggerEnter2D(Collider2D hit);
}
