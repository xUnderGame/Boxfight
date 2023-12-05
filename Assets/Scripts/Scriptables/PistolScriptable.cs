using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PistolScript", menuName = "PistolScriptable")]
public class PistolScriptable : ScriptableObject
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    public float projectileSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
}
