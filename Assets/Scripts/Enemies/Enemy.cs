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
}
