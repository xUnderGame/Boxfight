using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Default Character", menuName = "Character Scriptable")]
public class CharacterScriptable : ScriptableObject
{
    public int maxHP;
    public int baseHP;
    public float baseSpeed;
    public float baseDmg;
}
