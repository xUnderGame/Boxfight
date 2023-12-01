using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScript", menuName = "WeaponScriptable")]
public class WeaponScriptable : ScriptableObject
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
    public int bulletsPerShot;
    public float bulletSpread;
}
