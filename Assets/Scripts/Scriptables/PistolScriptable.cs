using UnityEngine;

[CreateAssetMenu(fileName = "Default Pistol", menuName = "Weapon Scriptables/Pistol")]
public class PistolScriptable : ScriptableObject
{
    public int energyCost;
    public float damage;
    public float firingSpeed;
    public Sprite weaponSprite;
    public GameObject projectile;
}
