using UnityEngine;
using UnityEngine.UI;

public class Enemy_Shooter : Enemy
{
    [HideInInspector] public Weapon equippedWeapon;

    public void OnEnable()
    {
        base.OnEnable();
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
