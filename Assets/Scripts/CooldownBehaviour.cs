using System;
using System.Collections;
using UnityEngine;

public class CooldownBehaviour : MonoBehaviour
{
    // Starts a cooldown and waits
    public IEnumerator StartCooldown(float cooldownTime, Action<bool> returnMainCD, bool checkCD)
    {
        if (!checkCD) yield break;
        float currentTime = Time.time + cooldownTime;
        returnMainCD(false);

        // Wait until cooldown is finished
        do { yield return new WaitForSeconds(0.1f); }
        while (currentTime > Time.time);
        returnMainCD(true);
    }
}
