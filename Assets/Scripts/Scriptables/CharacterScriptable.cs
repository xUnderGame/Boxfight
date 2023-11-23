using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharScript", menuName = "CharacterScriptable")]
public class CharacterScriptable : ScriptableObject
{
    public int baseHP {  get; set; }
    public float baseSpeed {  get; set; }
    public float baseDmg {  get; set; }
}
