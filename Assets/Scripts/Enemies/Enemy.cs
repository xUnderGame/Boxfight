using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    private Image hpBar = null;

    public virtual void OnEnable()
    {
        if (currentDmg <= 0) return;
        hpBar = gameObject.transform.Find("Healthbar").Find("Amount").GetComponent<Image>();      
    }

    // Hurt enemy
    public override void Hurt(int damage, GameObject damageSource)
    {
        // Spawn energy bits
        int droplets = Random.Range(0, 4); // 20% base chance to drop energy on hit
        if (droplets == 0 || damageSource.name == "Melee Area") DropEnergyBit(Random.Range(1, 4));
    
        // Hurt the enemy calling the base hurt method
        base.Hurt(damage, damageSource);

        // Draw HP bar
        if (hpBar) hpBar.fillAmount = (float)(currentHP / (float)maxHP);

        // Is HP below 0?
        if (currentHP <= 0) Kill();
    }

    // Kill enemy
    public override void Kill()
    {
        // How many coins to give the player
        JsonManager.Instance.userData.coins += Random.Range(1, 20);
        GameManager.Instance.gameUI.UpdateCoinsUI();

        // Removing enemy from the current room list
        Room room = GetComponentInParent<Room>();
        if (room) {
            room.enemyList.Remove(gameObject);
            if (room.CheckIfAllEnemiesDead()) room.TrapIsOff();
        }

        // Calling base kill
        base.Kill();
    }
}
