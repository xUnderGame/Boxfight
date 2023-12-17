using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour, IDamageable, ILoadScriptable
{
    public CharacterScriptable cs;
    [HideInInspector] public AnimationBehaviour anim;
    [HideInInspector] public MovementBehaviour mov;

    [HideInInspector] public int maxHP;
    [HideInInspector] public int currentHP;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentDmg;

    private void Awake()
    {
        anim = GetComponent<AnimationBehaviour>();
        mov = GetComponent<MovementBehaviour>();
        LoadScriptable();
    }

    // Hurt character
    public virtual void Hurt(float damage, GameObject damageSource) {
        // Hurt the character
        Debug.Log($"Ow! {gameObject.name} took {damage} damage.");

        // Spawn energy bits if the character hit wasn't a player
        if (!gameObject.CompareTag("Player")) {
            int droplets = Random.Range(0, 4); // 20% base chance to drop energy on hit
            if (droplets == 0 || damageSource.name == "Melee Area") DropEnergyBit(Random.Range(1, 4));
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
        maxHP = cs.baseHP;
        currentHP = cs.baseHP;
        currentSpeed = cs.baseSpeed;
        currentDmg = cs.baseDmg;
    }
}
