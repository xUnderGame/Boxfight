using UnityEngine;

[CreateAssetMenu(fileName = "Default Sniper", menuName = "Weapon Scriptables/Sniper")]
public class SniperScriptable : ScriptableObject
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
}
