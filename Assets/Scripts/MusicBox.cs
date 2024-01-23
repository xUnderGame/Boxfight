using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [HideInInspector] public static MusicBox Instance;

    void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }
        else { Instance = this; }
        DontDestroyOnLoad(transform.root.gameObject);
    }
}
