using UnityEngine;
using UnityEngine.UI;

public class EnemyShooter : Enemy
{
    [HideInInspector] public Weapon equippedWeapon;
    private GameObject[] weapons;

    public override void OnEnable()
    {
        if (currentDmg <= 0 || equippedWeapon != null) return;
        weapons = Resources.LoadAll<GameObject>("Prefabs/Weapons");
        GameObject weapon = Instantiate(weapons[Random.Range(1, 4)],gameObject.transform);
        equippedWeapon = weapon.GetComponent<Weapon>();
        base.OnEnable();
    }

    public void FixedUpdate()
    {
        if (!equippedWeapon) return;
        equippedWeapon.PointWeaponAtPlayer();
        equippedWeapon.Shoot(GameManager.Instance.playerObject.transform.position - equippedWeapon.transform.position);
    }
}
