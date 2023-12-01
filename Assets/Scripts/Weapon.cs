using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    [HideInInspector] public Sprite weaponSprite;
    [HideInInspector] public GameObject projectile;

    public void Awake()
    {
        weaponSprite = Resources.Load("Prefabs/Weapons/Pistol").GetComponent<Sprite>();
        projectile = (GameObject)Resources.Load("Prefabs/Projectiles/Default");
    }

    // Shoots the weapon
    public virtual void Shoot()
    {
        Instantiate(projectile, gameObject.transform.position, Quaternion.identity, gameObject.transform);
    }
}
