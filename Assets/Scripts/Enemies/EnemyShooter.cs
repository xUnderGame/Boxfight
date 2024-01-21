using UnityEngine;
using UnityEngine.UI;

public class EnemyShooter : Enemy
{
    [HideInInspector] public Weapon equippedWeapon;
    private GameObject[] weapons;
    public GameObject weaponsObject;

    private void Awake()
    {
        base.Awake();
        weapons = Resources.LoadAll<GameObject>("Prefabs/Weapons");
    }
    public override void OnEnable()
    {
        if (currentDmg <= 0) return;
        if (equippedWeapon != null) return;
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
