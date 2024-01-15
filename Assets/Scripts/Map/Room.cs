using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    GameObject corridorObject;
    CorridorBlock corridorScript;


    private void Start()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyList.Count != 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].gameObject.SetActive(true);
            }
        }
        else corridorScript.SetPointsWalledOnCorridor();

    }
}
