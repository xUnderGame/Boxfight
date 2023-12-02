using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, ILoadScriptable
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

    public void Start() { LoadScriptable(); }

    // Shoots the weapon
    public abstract void Shoot();

    // Loads a scriptable
    public abstract void LoadScriptable();
}
