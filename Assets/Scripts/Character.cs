using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour, IDamageable, ILoadScriptable
{
    public CharacterScriptable characterScriptable;
    [HideInInspector] public AnimationBehaviour anim;
    [HideInInspector] public MovementBehaviour mov;

    [HideInInspector] public int maxHP;
    [HideInInspector] public int currentHP;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int currentDmg;

    public void Awake()
    {
        anim = GetComponent<AnimationBehaviour>();
        mov = GetComponent<MovementBehaviour>();
        LoadScriptable();
    }

    // Hurt character
    public virtual void Hurt(int damage, GameObject damageSource)
    {
        if (maxHP == -1) return; // Invulnerable character

        // Hurt the character
        // Debug.Log($"Ow! {gameObject.name} took {damage} damage.");
        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;
    }

    // Kill character
    public virtual void Kill()
    {
       // Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

    // Drops an energy bit near a character
    protected void DropEnergyBit(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(
                GameManager.Instance.energyBitPrefab,
                new Vector3(
                    transform.position.x + Random.Range(-1.5f, 1.5f),
                    transform.position.y + Random.Range(-1.5f, 1.5f),
                    transform.position.z
                ),
                Quaternion.identity,
                GameManager.Instance.pickupPool.transform
            );   
        }
    }

    protected void DropHealthBit(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(
                GameManager.Instance.healthBitPrefab,
                new Vector3(
                    transform.position.x + Random.Range(-1.5f, 1.5f),
                    transform.position.y + Random.Range(-1.5f, 1.5f),
                    transform.position.z
                ),
                Quaternion.identity,
                GameManager.Instance.pickupPool.transform
            );
        }
    }

    // Loads a scriptable
    public void LoadScriptable()
    {
        maxHP = characterScriptable.maxHP;
        currentHP = characterScriptable.baseHP;
        currentSpeed = characterScriptable.baseSpeed;
        currentDmg = characterScriptable.baseDmg;
    }
}
