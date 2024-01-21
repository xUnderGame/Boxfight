using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [HideInInspector] public Weapon equippedWeapon;

    public override void OnEnable()
    {
        if (currentDmg <= 0 || equippedWeapon != null) return;
 
        // Equip random weapon
        GameObject weapon = Instantiate(GameManager.Instance.weapons[Random.Range(1, 4)],gameObject.transform);
        equippedWeapon = weapon.GetComponent<Weapon>();
        
        // Assigns a random weapon scriptable to the enemy
        switch (equippedWeapon.GetType().Name)
        {
            case "Pistol":
                ((Pistol)equippedWeapon).ws = GameManager.Instance.pistolVariations[Random.Range(0, GameManager.Instance.pistolVariations.Count)];
                break;

            case "Shotgun":
                ((Shotgun)equippedWeapon).ws = GameManager.Instance.shotgunVariations[Random.Range(0, GameManager.Instance.shotgunVariations.Count)];
                break;

            case "Sniper":
                ((Sniper)equippedWeapon).ws = GameManager.Instance.sniperVariations[Random.Range(0, GameManager.Instance.sniperVariations.Count)];
                break;
            
            case "Launcher":
                ((Launcher)equippedWeapon).ws = GameManager.Instance.launcherVariations[Random.Range(0, GameManager.Instance.launcherVariations.Count)];
                break;

            default:
                Debug.Log(equippedWeapon.GetType().Name);
                break;
        }
        equippedWeapon.LoadScriptable();
        
        base.OnEnable();
    }

    public void FixedUpdate()
    {
        if (!equippedWeapon) return;
        equippedWeapon.PointWeaponAtPlayer();
        equippedWeapon.Shoot(GameManager.Instance.playerObject.transform.position - equippedWeapon.transform.position);
    }
}
