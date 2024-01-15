using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

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
    private int iterationSizeMap = 2;


    private void Awake()
    {
        enemies = Resources.LoadAll<GameObject>("Prefabs/Enemies");
    }
    void Start()
    {

        System.Random rnd = new System.Random();
        int squareSize = rnd.Next(10, 20);

        if (squareSize % 2 != 0)
        {
            squareSize = squareSize + 1;
        }


        DrawSquare(new Vector3Int(0, 0, 0), squareSize, 0, "center");
    }
    void DrawSquare(Vector3Int startPosition, int squareSize, int iterationSizeMapTimes, string direction)
    {
        GameObject room = CreateTriggerForRoom(startPosition, squareSize, direction);
        Room roomScript = room.GetComponent<Room>();

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
                                int corridorSize = rnd.Next(10, 20);
                                if (DrawCorridorPosible(tilePosition, corridorSize, actualdirection))
                                {
                                    tilemapDoor.SetTile(tilePosition, door);
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
                        tilemapWall.SetTile(tilePosition, null);
                        tilemapGround.SetTile(tilePosition, ground);


                        if (iterationSizeMapTimes != 1)
                        {
                            System.Random rnd = new System.Random();
                            if (rnd.Next(0, 50) == 1)
                            {
                                Vector3 instPos = tilemapWall.CellToWorld(tilePosition);
                                GameObject enemy = Instantiate(enemies[1], instPos, Quaternion.identity);
                                enemy.SetActive(false);
                                roomScript.enemyList.Add(enemy);
                            }
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


        int squareSize = rnd.Next(10, 15);
        if (squareSize % 2 != 0)
        {
            squareSize = squareSize + 1;
        }

        DrawSquare(tilePosition, squareSize, iterationSizeMapTimes, direction);
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
        trigger.transform.localScale = new Vector3(TileSize * squareSize, TileSize * squareSize, 0);
        trigger.transform.parent = GameObject.Find("TriggerSquares").transform;
        return trigger;

    }
}
