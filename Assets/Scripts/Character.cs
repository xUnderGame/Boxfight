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
    public virtual void Hurt(float damage) {
        Debug.Log($"Ow! {gameObject.name} took {damage} damage.");
    }

    // Kill character
    public virtual void Kill() {
        Debug.Log($"RIP. {gameObject.name} died!");
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
