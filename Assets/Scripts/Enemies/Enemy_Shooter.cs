using UnityEngine;
using UnityEngine.UI;

public class Enemy_Shooter : Enemy
{
    [HideInInspector] public Weapon equippedWeapon;

    public override void OnEnable()
    {
        if (currentDmg <= 0) return;
        equippedWeapon = transform.Find("Weapons").GetChild(0).GetComponent<Weapon>();
        base.OnEnable();
    }

    public void FixedUpdate()
    {
        if (!equippedWeapon) return;
        equippedWeapon.PointWeaponAtPlayer();
        equippedWeapon.Shoot(GameManager.Instance.playerObject.transform.position - equippedWeapon.transform.position);
    }
}
