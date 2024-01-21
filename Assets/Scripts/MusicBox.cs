using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [HideInInspector] public static MusicBox Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
