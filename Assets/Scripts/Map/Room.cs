using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool tutorial = false;
    public int id;
    public int size;
    [SerializeField] public List<GameObject> enemyList = new();
    public List<Vector3Int> corridorPosition = new();
    public List<GameObject> roomsShared = new();
    public List<Vector3Int> lostCorridors = new();

    private GameObject corridorObject;
    private CorridorBlock corridorScript;

    private void Awake()
    {
        corridorObject = GameObject.Find("Puente");
        corridorScript = corridorObject.GetComponent<CorridorBlock>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        Player playerScript = collision.GetComponent<Player>();

        if (playerScript.idRoomsVisited.Contains(id)) return;
        if (CheckIfAllEnemiesDead()) 
        {
            playerScript.idRoomsVisited.Add(id);
            foreach (var room in roomsShared)
            {
                playerScript.idRoomsVisited.Add(room.GetComponent<Room>().id);
            }
            string numRooms = GameManager.Instance.gameUI.roomsVisited.text.Split("/")[1];
            GameManager.Instance.gameUI.roomsVisited.text = Convert.ToInt32(GameManager.Instance.player.idRoomsVisited.Count) + "/" + numRooms;
            return;
        }
        TrapIsOn();
        if (roomsShared.Count == 0) return;
        foreach(var room in roomsShared)
        {
            room.GetComponent<Room>().TrapIsOn();
        }
    }

    public bool CheckIfAllEnemiesDead()
    {
        if (enemyList.Count != 0) return false;
        return !roomsShared.Any(room => { return room.GetComponent<Room>().enemyList.Count != 0; });

    }

    public void TrapIsOff()
    {
        if (tutorial == true) 
        {
            return;
        }

        corridorScript.PrintCorridors(corridorPosition, "unblock");
        GameManager.Instance.player.idRoomsVisited.Add(id);
        foreach (GameObject room in roomsShared)
        {
            corridorScript.PrintCorridors(room.GetComponent<Room>().corridorPosition, "unblock");
            GameManager.Instance.player.idRoomsVisited.Add(room.GetComponent<Room>().id);
        }

        string numRooms = GameManager.Instance.gameUI.roomsVisited.text.Split("/")[1];
        GameManager.Instance.gameUI.roomsVisited.text = Convert.ToInt32(GameManager.Instance.player.idRoomsVisited.Count) + "/" + numRooms;
    }

    public void TrapIsOn()
    {
        if (tutorial == true) return;
        if (lostCorridors.Count != 0)
        {
            Vector3Int lostcorr = GetCorridorLost();
            corridorPosition.Add(lostcorr);
        }
        corridorScript.PrintCorridors(corridorPosition, "block");

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetActive(true);
        }
    }

    private Vector3Int GetCorridorLost()
    {
    
        if (lostCorridors[0].x == lostCorridors[lostCorridors.Count -1].x)
        {
            if (corridorScript.TileIn(lostCorridors[0].x, lostCorridors[0].y + 1, 0, "corridor"))
            {
                return new Vector3Int(lostCorridors[0].x, lostCorridors[0].y + 1, 0);
            }
            else if (corridorScript.TileIn(lostCorridors[0].x, lostCorridors[0].y - 1, 0, "corridor"))
            {
                return new Vector3Int(lostCorridors[0].x, lostCorridors[0].y - 1, 0);
            }
            else if (corridorScript.TileIn(lostCorridors[lostCorridors.Count - 1].x, lostCorridors[lostCorridors.Count - 1].y + 1, 0, "corridor"))
            {
                return new Vector3Int(lostCorridors[lostCorridors.Count - 1].x, lostCorridors[lostCorridors.Count - 1].y + 1, 0);
            }
            else if (corridorScript.TileIn(lostCorridors[lostCorridors.Count - 1].x, lostCorridors[lostCorridors.Count - 1].y - 1, 0, "corridor"))
            {
                return new Vector3Int(lostCorridors[lostCorridors.Count - 1].x, lostCorridors[lostCorridors.Count - 1].y - 1, 0);
            }
            else return lostCorridors[0];
        }
        else if (lostCorridors[0].y == lostCorridors[lostCorridors.Count -1].y)
        {
            if (corridorScript.TileIn(lostCorridors[0].x + 1, lostCorridors[0].y, 0, "corridor")) { 
                return new Vector3Int(lostCorridors[0].x + 1, lostCorridors[0].y, 0); 
            }
            else if (corridorScript.TileIn(lostCorridors[0].x - 1, lostCorridors[0].y, 0, "corridor")) { 
                return new Vector3Int(lostCorridors[0].x - 1, lostCorridors[0].y, 0); 
            }
            else if (corridorScript.TileIn(lostCorridors[lostCorridors.Count - 1].x + 1, lostCorridors[lostCorridors.Count - 1].y, 0, "corridor")) { 
                return new Vector3Int(lostCorridors[lostCorridors.Count - 1].x + 1, lostCorridors[lostCorridors.Count - 1].y, 0); 
            }
            else if (corridorScript.TileIn(lostCorridors[lostCorridors.Count - 1].x - 1, lostCorridors[lostCorridors.Count - 1].y, 0, "corridor")) { 
                return new Vector3Int(lostCorridors[lostCorridors.Count - 1].x - 1, lostCorridors[lostCorridors.Count - 1].y, 0); 
            }
            else return lostCorridors[0];
        }
        else return lostCorridors[0];
    }


}
