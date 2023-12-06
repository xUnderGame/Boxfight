using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : Projectile
{
    private Weapon weapon;
    private Vector2 moveTowards;
    private readonly float fixedSpeedDecrease = 0.9f;

    // Has a funny bug where if you switch weapon the same frame after shooting,
    // it gains the stats of the other weapon.
    // I'm leaving it in as a "mechanic" now.
    void Awake()
    {
        weapon = GameManager.Instance.player.inv.activeWeapon;
        moveTowards = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
    }

    // Update is called once per frame
    public override void FixedUpdate() { Travel(); }

    public override void Travel() { transform.Translate(moveTowards.normalized * (bulletSpeed - fixedSpeedDecrease), Space.World); }

    public override void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable)) damageable?.Hurt(weapon.damage);
        Destroy(gameObject);
    }
}
