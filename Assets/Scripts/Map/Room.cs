using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public int id;
    public int size;
    public List<GameObject> enemyList = new List<GameObject>();
    public List<Vector3Int> corridorPosition = new List<Vector3Int>();
    public List<GameObject> roomsShared = new List<GameObject>();

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
            TrapIsOn(playerScript);
            if (roomsShared.Count != 0)
            {
                for (int i = 0; i < roomsShared.Count; i++)
                {
                    roomsShared[i].GetComponent<Room>().TrapIsOn(playerScript);
                }
            }
        }
    }

    public bool CheckIfAllEnemiesDead()
    {
        if (enemyList.Count == 0)
        {
            foreach (GameObject room in roomsShared)
            {
                if (room.GetComponent<Room>().enemyList.Count != 0) return false;
            }
            return true;
        }
        else return false;
    }

    public void TrapIsOff()
    {
        corridorScript.PrintCorridors(corridorPosition, "unblock");
        foreach (GameObject room in roomsShared)
        {
            corridorScript.PrintCorridors(room.GetComponent<Room>().corridorPosition, "unblock");
        }
    }

    public void TrapIsOn(Player player)
    {
        if (enemyList.Count != 0 && !player.idRoomsVisited.Contains(id))
        {
            corridorScript.PrintCorridors(corridorPosition, "block");
            player.idRoomsVisited.Add(id);
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].SetActive(true);
            }
        }
    }
}
