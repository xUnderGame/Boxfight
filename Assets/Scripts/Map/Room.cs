using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    GameObject corridorObject;
    CorridorBlock corridorScript;


    private void Awake()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(enemyList.Count);
        if (collision.CompareTag("Player"))
        {
            if (enemyList.Count != 0)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].SetActive(true);
                }
            }
            else corridorScript.SetPointsWalledOnCorridor();
        }
    }

    public void CheckEnemiesFromRoom()
    {
        if (enemyList.Count == 0) corridorScript.SetPointsWalledOnCorridor();

    }
}
