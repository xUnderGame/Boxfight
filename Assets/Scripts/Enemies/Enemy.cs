using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    [HideInInspector] public Weapon equippedWeapon;
    private Image hpBar = null;

    public void OnEnable()
    {
        if (currentDmg <= 0) return;
        hpBar = gameObject.transform.Find("Healthbar").Find("Amount").GetComponent<Image>();
        equippedWeapon = transform.Find("Weapons").GetChild(0).GetComponent<Weapon>();
        
    }

    public void FixedUpdate()
    {
        if (!equippedWeapon) return;
        equippedWeapon.PointWeaponAtPlayer();
        equippedWeapon.Shoot(GameManager.Instance.playerObject.transform.position - equippedWeapon.transform.position);
    }

    // Hurt enemy
    public override void Hurt(int damage, GameObject damageSource)
    {
        base.Hurt(damage, damageSource);

        // Draw HP bar
        if (hpBar) hpBar.fillAmount = (float)(currentHP / (float)maxHP);

        // Spawn energy bits
        int droplets = Random.Range(0, 4); // 20% base chance to drop energy on hit
        if (droplets == 0 || damageSource.name == "Melee Area") DropEnergyBit(Random.Range(1, 4));
    
        // Is HP below 0?
        if (currentHP <= 0) Kill();
    }

    // Kill enemy
    public override void Kill()
    {
        // How many coins to give the player
        JsonManager.Instance.userData.coins += Random.Range(1, 20);
        GameManager.Instance.gameUI.UpdateCoinsUI();

        Room room = GetComponentInParent<Room>();
        room.enemyList.Remove(gameObject);
        room.CheckEnemiesFromRoom();
        base.Kill();
    }
}
