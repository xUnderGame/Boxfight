using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public int id;

    public List<GameObject> enemyList = new List<GameObject>();
    public List<Vector3Int> corridorPosition = new List<Vector3Int>();

    GameObject corridorObject;
    CorridorBlock corridorScript;


    private void Awake()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();
            if (enemyList.Count != 0 && !playerScript.idRoomsVisited.Contains(id))
            {
                Debug.Log(corridorPosition);
                corridorScript.PrintCorridors(corridorPosition, "block");
                playerScript.idRoomsVisited.Add(id);
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].SetActive(true);
                }
            }
        }
    }

    public void CheckEnemiesFromRoom()
    {
        if (enemyList.Count == 0) corridorScript.PrintCorridors(corridorPosition,"unblock");
    }
}
