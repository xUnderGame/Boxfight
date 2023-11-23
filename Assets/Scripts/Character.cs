using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(PolygonCollider2D), typeof(SpriteRenderer))]
abstract class Character : MonoBehaviour
{
    public CharacterScriptable Cs;

    private int currentHP;
    private float currentSpeed;
    private float currentDmg;

    public AnimationBehaviour anim;

    public MovementBehaviour mov;

    private void Awake()
    {
        currentHP = Cs.baseHP;
        currentSpeed = Cs.baseSpeed;
        currentDmg = Cs.baseDmg;
        anim = GetComponent<AnimationBehaviour>();
        mov = GetComponent<MovementBehaviour>();
    }

}