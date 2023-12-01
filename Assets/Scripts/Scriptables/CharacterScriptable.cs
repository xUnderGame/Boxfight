using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharScript", menuName = "CharacterScriptable")]
public class CharacterScriptable : ScriptableObject
{
    public int maxHP;
    public int baseHP;
    public float baseSpeed;
    public float baseDmg;
}
