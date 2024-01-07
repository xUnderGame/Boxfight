using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;

public class TilemapConnectedSquaresDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileToDraw; // La casilla que usarás para dibujar
    public TileBase door;
    public TileBase wall;
    private int separation = 10; // Separación entre los cuadrados
    private int iterationSizeMap = 50;

    void Start()
    {
        if (tilemap == null || tileToDraw == null)
            return;

        System.Random rnd = new System.Random();
        int squareSize = rnd.Next(10, 20);


        DrawSquare(new Vector3Int(0, 0, 0), squareSize, 0, "center");
    }
    void DrawSquare(Vector3Int startPosition, int squareSize, int iterationSizeMapTimes, string direction)
    {
        if (iterationSizeMapTimes < iterationSizeMap)
        {
            iterationSizeMapTimes++;
            startPosition = SetSquareInLine(startPosition, direction, squareSize);
            List<Vector3Int> corridorsPosition = new List<Vector3Int>();

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
                            if (x == squareSize/2 && y == squareSize - 1)
                            {
                                if(direction != "down")
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
                            else if (x == squareSize/2 && y == 0)
                            {
                                if(direction != "up")
                                {
                                    actualdirection = "down";
                                }
                            }

                            if (iterationSizeMapTimes < iterationSizeMap && actualdirection != "")
                            {
                                System.Random rnd = new System.Random();      
                                int corridorSize = rnd.Next(10, 50);
                                if (DrawCorridorPosible(tilePosition, corridorSize, actualdirection)) 
                                {
                                    tilemap.SetTile(tilePosition, door);
                                    DrawCorridor(tilePosition, corridorSize, actualdirection, iterationSizeMapTimes); 
                                }
                                else
                                {
                                    tilemap.SetTile(tilePosition, wall);
                                }
                            }
                            else if (IsCellEmpty(tilePosition)) tilemap.SetTile(tilePosition, wall);
                        }
                        else if(IsCellEmpty(tilePosition)) tilemap.SetTile(tilePosition, wall);
                    }
                    else tilemap.SetTile(tilePosition, tileToDraw);
                }
            }
        }
        else return;
    }
    void DrawCorridor(Vector3Int corridorPosition, int corridorSize ,string direction, int iterationSizeMapTimes)
    {
        Vector3Int tilePosition = new Vector3Int();
        System.Random rnd = new System.Random();

        for (int i = 0; i < corridorSize; i++)
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
            tilemap.SetTile(tilePosition, door);
        }
        DrawSquare(tilePosition, rnd.Next(10,30), iterationSizeMapTimes, direction);
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
        TileBase tile = tilemap.GetTile(cellPosition);
        return tile == null;
    }

    bool DrawCorridorPosible(Vector3Int corridorPosition, int corridorSize ,string direction)
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
}
