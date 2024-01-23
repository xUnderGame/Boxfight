using UnityEngine;

[CreateAssetMenu(fileName = "Default Pistol", menuName = "Weapon Scriptables/Pistol")]
public class PistolScriptable : ScriptableObject
{
    public int energyCost;
    public int damage;
    public float firingSpeed;
    public float timeToLive;
    public Sprite weaponSprite;
    public GameObject projectile;
    public Color color;
}
