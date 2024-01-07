using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        anim = GetComponent<AnimationBehaviour>();
        mov = GetComponent<MovementBehaviour>();
        LoadScriptable();
    }

    // Hurt character
    public virtual void Hurt(int damage, GameObject damageSource) {
        if (maxHP == -1) return; // Invulnerable character

        // Hurt the character
        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;
        Debug.Log($"Ow! {gameObject.name} took {damage} damage.");

        // Spawn energy bits if the character hit wasn't a player
        if (!gameObject.CompareTag("Player"))
        {
            int droplets = Random.Range(0, 4); // 20% base chance to drop energy on hit
            if (droplets == 0 || damageSource.name == "Melee Area") DropEnergyBit(Random.Range(1, 4));
        }

        // Player hurt actions (move to player later on!)
        else {
            GameManager.Instance.gameUI.UpdateHealthUI();
        }
    }

    // Kill character
    public virtual void Kill() {
        Debug.Log($"RIP. {gameObject.name} died!");
    }

    // Drops an energy bit near a character
    private void DropEnergyBit(int amount = 1)
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

    // Loads a scriptable
    public void LoadScriptable()
    {
        maxHP = characterScriptable.maxHP;
        currentHP = characterScriptable.baseHP;
        currentSpeed = characterScriptable.baseSpeed;
        currentDmg = characterScriptable.baseDmg;
    }
}
