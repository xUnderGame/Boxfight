using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapConnectedSquaresDrawer : MonoBehaviour
{
    public GameObject triggerPrefab;

    public Tilemap tilemapGround;
    public Tilemap tilemapWall;
    public Tilemap tilemapDoor;

    private GameObject[] enemies;


    public TileBase ground; // La casilla que usarás para dibujar
    public TileBase door;
    public TileBase wall;


    //Apartir de 7 da errores
    public int iterationSizeMap = 3;

    private int numSquares = 0;
    private List<GameObject> roomsCreated = new();
    private CorridorBlock corridorScript;

    void Awake()
    {   
        corridorScript = GameObject.Find("Puente").GetComponent<CorridorBlock>();
        enemies = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        System.Random rnd = new System.Random();
        int squareSize = rnd.Next(10, 20);

        if (squareSize % 2 == 0)
        {
            squareSize = squareSize + 1;
        }

        DrawSquare(new Vector3Int(0, 0, 0), squareSize, 0, "center");
        GameManager.Instance.gameUI.roomsVisited.text = "0/" + numSquares.ToString();
    }
    void DrawSquare(Vector3Int startPosition, int squareSize, int iterationSizeMapTimes, string direction)
    {
        numSquares++;
        GameObject room = CreateTriggerForRoom(startPosition, squareSize, direction);
        Room roomScript = room.GetComponent<Room>();
        Vector3Int positionAdjusted = AdjustCorridorToList(startPosition, direction);
        roomScript.corridorPosition.Add(positionAdjusted);
        roomScript.id = numSquares;
        roomScript.size = squareSize;
        roomsCreated.Add(room);

        if (iterationSizeMapTimes < iterationSizeMap)
        {
            iterationSizeMapTimes++;
            startPosition = SetSquareInLine(startPosition, direction, squareSize);

            for (int x = 0; x < squareSize; x++)
            {
                for (int y = 0; y < squareSize; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(startPosition.x + x, startPosition.y + y, 0);

                    if (x == 0 || y == 0 || y == squareSize - 1 || x == squareSize - 1)
                    {
                        if ((x == squareSize / 2 && y == 0) || (x == squareSize / 2 && y == squareSize - 1) ||
                            (x == 0 && y == squareSize / 2) || (x == squareSize - 1 && y == squareSize / 2))
                        {

                            string actualdirection = "";
                            if (x == squareSize / 2 && y == squareSize - 1)
                            {
                                if (direction != "down")
                                {
                                    actualdirection = "up";
                                }
                            }
                            else if (x == squareSize - 1 && y == squareSize / 2)
                            {
                                if (direction != "left")
                                {
                                    actualdirection = "right";
                                }
                            }
                            else if (x == 0 && y == squareSize / 2)
                            {
                                if (direction != "right")
                                {
                                    actualdirection = "left";
                                }
                            }
                            else if (x == squareSize / 2 && y == 0)
                            {
                                if (direction != "up")
                                {
                                    actualdirection = "down";
                                }
                            }

                            if (iterationSizeMapTimes < iterationSizeMap && actualdirection != "")
                            {
                                System.Random rnd = new System.Random();
                                int corridorSize = rnd.Next(6, 12);
                                if (DrawCorridorPosible(tilePosition, corridorSize, actualdirection))
                                {
                                    tilemapDoor.SetTile(tilePosition, door);
                                    roomScript.corridorPosition.Add(tilePosition);
                                    DrawCorridor(tilePosition, corridorSize, actualdirection, iterationSizeMapTimes);
                                }
                                else
                                {
                                    tilemapWall.SetTile(tilePosition, wall);
                                }
                            }
                            else if (IsCellEmpty(tilePosition)) tilemapWall.SetTile(tilePosition, wall);
                        }
                        else if (IsCellEmpty(tilePosition)) tilemapWall.SetTile(tilePosition, wall);
                    }
                    else
                    {
                        if (corridorScript.TileIn(tilePosition.x, tilePosition.y, 0, "corridor"))
                        {
                            Debug.Log(room.name);

                            roomScript.lostCorridors.Add(tilePosition);
                        }
                        if (corridorScript.TileIn(tilePosition.x, tilePosition.y, 0, "corridor"))
                        {
                            Debug.Log(room.name);

                            roomScript.lostCorridors.Add(tilePosition);
                        }
                        tilemapDoor.SetTile(tilePosition, null);
                        tilemapWall.SetTile(tilePosition, null);
                        tilemapGround.SetTile(tilePosition, ground);


                        if (iterationSizeMapTimes != 1)
                        {
                            SpawnEnemy(tilePosition, room);
                        }
                    }
                }
            }
        }
        else return;
    }
    void DrawCorridor(Vector3Int corridorPosition, int corridorSize, string direction, int iterationSizeMapTimes)
    {
        Vector3Int tilePosition = new Vector3Int();
        Vector3Int[] walls = new Vector3Int[2];
        System.Random rnd = new System.Random();

        for (int i = 0; i < corridorSize; i++)
        {
            switch (direction)
            {
                case "left":
                    tilePosition = new Vector3Int(corridorPosition.x - i, corridorPosition.y, corridorPosition.z);
                    walls[0] = new Vector3Int(corridorPosition.x - i, corridorPosition.y + 1, corridorPosition.z);
                    walls[1] = new Vector3Int(corridorPosition.x - i, corridorPosition.y - 1, corridorPosition.z);

                    break;
                case "right":
                    tilePosition = new Vector3Int(corridorPosition.x + i, corridorPosition.y, corridorPosition.z);
                    walls[0] = new Vector3Int(corridorPosition.x + i, corridorPosition.y + 1, corridorPosition.z);
                    walls[1] = new Vector3Int(corridorPosition.x + i, corridorPosition.y - 1, corridorPosition.z);
                    break;
                case "up":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y + i, corridorPosition.z);
                    walls[0] = new Vector3Int(corridorPosition.x + 1, corridorPosition.y + i, corridorPosition.z);
                    walls[1] = new Vector3Int(corridorPosition.x - 1, corridorPosition.y + i, corridorPosition.z);
                    break;
                case "down":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y - i, corridorPosition.z);
                    walls[0] = new Vector3Int(corridorPosition.x + 1, corridorPosition.y - i, corridorPosition.z);
                    walls[1] = new Vector3Int(corridorPosition.x - 1, corridorPosition.y - i, corridorPosition.z);
                    break;
            }
            tilemapDoor.SetTile(tilePosition, door);
            tilemapWall.SetTile(walls[0], wall);
            tilemapWall.SetTile(walls[1], wall);
        }
        if (direction == "left") tilePosition = new Vector3Int(tilePosition.x + 1, tilePosition.y, tilePosition.z);
        else if (direction == "down") tilePosition = new Vector3Int(tilePosition.x, tilePosition.y + 1, tilePosition.z);


        int NewSquareSize = rnd.Next(10, 15);
        if (NewSquareSize % 2 == 0)
        {
            NewSquareSize = NewSquareSize + 1;
        }

        DrawSquare(tilePosition, NewSquareSize, iterationSizeMapTimes, direction);
    }

    Vector3Int SetSquareInLine(Vector3Int v3, string direction, int squareSize)
    {
        switch (direction)
        {
            case "up":
                v3 = new Vector3Int(v3.x - squareSize / 2, v3.y, 0);
                break;
            case "down":
                v3 = new Vector3Int(v3.x - squareSize / 2, v3.y - squareSize, 0);
                break;
            case "left":
                v3 = new Vector3Int(v3.x - squareSize, v3.y - squareSize / 2, 0);
                break;
            case "right":
                v3 = new Vector3Int(v3.x, v3.y - squareSize / 2, v3.z);
                break;
            case "center":
                v3 = new Vector3Int(-squareSize / 2, -squareSize / 2, 0);
                break;
        }
        return v3;
    }

    bool IsCellEmpty(Vector3Int cellPosition)
    {
        TileBase tileGround = tilemapGround.GetTile(cellPosition);
        TileBase tileWall = tilemapWall.GetTile(cellPosition);
        TileBase tileDoor = tilemapDoor.GetTile(cellPosition);

        return tileGround == null && tileWall == null && tileDoor == null;
    }

    bool DrawCorridorPosible(Vector3Int corridorPosition, int corridorSize, string direction)
    {
        Vector3Int tilePosition = new Vector3Int();

        for (int i = 1; i < corridorSize; i++)
        {
            switch (direction)
            {
                case "left":
                    tilePosition = new Vector3Int(corridorPosition.x - i, corridorPosition.y, corridorPosition.z);
                    break;
                case "right":
                    tilePosition = new Vector3Int(corridorPosition.x + i, corridorPosition.y, corridorPosition.z);
                    break;
                case "up":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y + i, corridorPosition.z);
                    break;
                case "down":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y - i, corridorPosition.z);
                    break;
            }
            if (!IsCellEmpty(tilePosition)) return false;
        }
        return true;
    }


    Vector3 GetCenterSquareToWorld(Vector3Int startPosition, int squareSize, string direction)
    {
        Vector3Int centerOfSquare = new Vector3Int();
        float squareSizeReal = squareSize / 2;

        if (direction == "left")
        {
            centerOfSquare = new Vector3Int(startPosition.x - squareSize / 2, startPosition.y, 0);
        }
        else if (direction == "right")
        {
            centerOfSquare = new Vector3Int(startPosition.x + squareSize / 2, startPosition.y, 0);
        }
        else if (direction == "up")
        {
            centerOfSquare = new Vector3Int(startPosition.x, startPosition.y + squareSize / 2, 0);
        }
        else if (direction == "down")
        {
            centerOfSquare = new Vector3Int(startPosition.x, startPosition.y - squareSize / 2, 0);
        }
        else
        {
            centerOfSquare = new Vector3Int(startPosition.x, startPosition.y, 0);
        }


        Vector3 pointOnTheWorld = tilemapWall.CellToWorld(centerOfSquare);
        return pointOnTheWorld;
    }

    public GameObject CreateTriggerForRoom(Vector3Int startPosition, int squareSize, string direction)
    {
        int TileSize = 2;
        Vector3 spawnPosition = GetCenterSquareToWorld(startPosition, squareSize, direction);
        GameObject trigger = Instantiate(triggerPrefab, spawnPosition, Quaternion.identity);
        trigger.name = trigger.name.Replace("(Clone)", $" {roomsCreated.Count}");

        if (direction == "right")
        {
            trigger.transform.position = new Vector3(trigger.transform.position.x + 1, trigger.transform.position.y + 1, trigger.transform.position.z);
        }
        else if (direction == "left")
        {
            trigger.transform.position = new Vector3(trigger.transform.position.x - 1, trigger.transform.position.y + 1, trigger.transform.position.z);
        }
        else if (direction == "up")
        {
            trigger.transform.position = new Vector3(trigger.transform.position.x + 1, trigger.transform.position.y + 1, trigger.transform.position.z);
        }
        else if (direction == "down")
        {
            trigger.transform.position = new Vector3(trigger.transform.position.x + 1, trigger.transform.position.y - 1, trigger.transform.position.z);
        }
        else
        {
            trigger.transform.position = new Vector3(trigger.transform.position.x + 1, trigger.transform.position.y + 1, trigger.transform.position.z);
        }

        trigger.transform.localScale = new Vector3(TileSize * squareSize - 6.25f, TileSize * squareSize - 6.25f, 0);
        trigger.transform.parent = GameObject.Find("TriggerSquares").transform;
        return trigger;

    }

    public Vector3Int AdjustCorridorToList(Vector3Int startPosition, string direction)
    {
        if (direction == "left")
        {
            return new Vector3Int(startPosition.x - 1, startPosition.y, startPosition.z);
        }
        else if (direction == "down")
        {
            return new Vector3Int(startPosition.x, startPosition.y - 1, startPosition.z);
        }
        else return startPosition;
    }

    private void SpawnEnemy(Vector3Int tilePosition, GameObject room)
    {
        int rndSpawn = Random.Range(0, 51);
        var i = rndSpawn switch
        {
            1 => 2,
            > 48 => 0,
            > 47 => 1,
            _ => -1,
        };
     
        if (i == -1) return;

        Vector3 instPos = tilemapWall.CellToWorld(tilePosition);
        Vector3 instPosCorre = new Vector3(instPos.x + 1, instPos.y + 1, 0);

        GameObject enemy = Instantiate(enemies[i], instPosCorre, Quaternion.identity);
        enemy.transform.parent = room.transform;

        if (enemy.CompareTag("Enemy")) {
            enemy.SetActive(false);
            room.GetComponent<Room>().enemyList.Add(enemy); 
        }
        return;
    }
}
