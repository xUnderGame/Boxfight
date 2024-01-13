using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    [HideInInspector] public Weapon equippedWeapon;

    public void OnEnable()
    {
        if (currentDmg <= 0) return;
        equippedWeapon = transform.Find("Weapons").GetChild(0).GetComponent<Weapon>();
    }

    public void FixedUpdate()
    {
        if (!equippedWeapon) return;
        equippedWeapon.PointWeaponAtPlayer();
        equippedWeapon.Shoot(GameManager.Instance.playerObject.transform.position - equippedWeapon.transform.position);
    }

    // Hurt enemy
    public override void Hurt(int damage, GameObject damageSource)
    {
        base.Hurt(damage, damageSource);
        // Spawn energy bits
        int droplets = Random.Range(0, 4); // 20% base chance to drop energy on hit
        if (droplets == 0 || damageSource.name == "Melee Area") DropEnergyBit(Random.Range(1, 4));
    
        // Is HP below 0?
        if (currentHP <= 0) Kill();
    }
}
