using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour, IDamageable, ILoadScriptable
{
    public CharacterScriptable cs;
    [HideInInspector] public AnimationBehaviour anim;
    [HideInInspector] public MovementBehaviour mov;

    private int maxHP;
    private int currentHP;
    private float currentSpeed;
    private float currentDmg;

    private void Awake()
    {
        maxHP = cs.baseHP;
        currentHP = cs.baseHP;
        currentSpeed = cs.baseSpeed;
        currentDmg = cs.baseDmg;
        anim = GetComponent<AnimationBehaviour>();
        mov = GetComponent<MovementBehaviour>();
    }

    // Hurt character
    public virtual void Hurt() {

    }

    // Kill character
    public virtual void Kill() {

    }

    // Loads a scriptable
    public void LoadScriptable() {
        
    }
}
