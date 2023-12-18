using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapConnectedSquaresDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileToDraw; // La casilla que usarás para dibujar
    public TileBase door;
    public TileBase wall;
    private int separation = 8; // Separación entre los cuadrados

    void Start()
    {
        if (tilemap == null || tileToDraw == null)
            return;
        
        System.Random rnd = new System.Random();
        int squareSize = rnd.Next(10,20);
        
        Vector3Int centerSquareStart = new Vector3Int(-squareSize / 2, -squareSize / 2, 0);
        DrawSquare(centerSquareStart, squareSize);
        // DrawConnectedSquares();
    }
    void DrawSquare(Vector3Int startPosition, int squareSize)
    {   
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
                        if (y == squareSize - 1)
                        {
                            tilemap.SetTile(tilePosition, door);
                            corridorsPosition.Add(tilePosition);
                            Debug.Log(tilePosition);
                        }
                        else
                        {
                            System.Random rndGenerateDoor = new System.Random();
                            if (rndGenerateDoor.Next(0, 2) == 1)
                            {
                                tilemap.SetTile(tilePosition, door);
                                corridorsPosition.Add(tilePosition);
                                Debug.Log(tilePosition);

                            } else tilemap.SetTile(tilePosition, wall);
                        }
                    }
                    else tilemap.SetTile(tilePosition, wall);
                }
                else tilemap.SetTile(tilePosition, tileToDraw);
            }
        }
        DrawCorridors(corridorsPosition, squareSize);
    }
    
    void DrawCorridors(List<Vector3Int> corridorsPosition, int squareSize)
    {
        for (int i = 0; i < corridorsPosition.Count; i++)
        {
            string direction = "";
            if (corridorsPosition[i].x == 0)
            {
                if (corridorsPosition[i].y > 0)
                {
                    direction = "up";
                }
                else
                {
                    direction = "down";
                }
            }
            else if (corridorsPosition[i].y == 0)
            {
                if (corridorsPosition[i].x > 0)
                {
                    direction = "right";
                }
                else
                {
                    direction = "left";
                }
            }
            DrawCorridor(corridorsPosition[i], direction);
        }   
    }

    void DrawCorridor(Vector3Int corridorPosition, string direction)
    {
        for (int i = 0; i < separation; i++)
        {
            Vector3Int tilePosition = new Vector3Int();
            switch (direction)
            {
                case "left":
                    tilePosition = new Vector3Int(corridorPosition.x -i, corridorPosition.y, corridorPosition.z);
                    break;
                case "right":
                    tilePosition = new Vector3Int(corridorPosition.x +i, corridorPosition.y, corridorPosition.z);
                    break;
                case "up":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y +i, corridorPosition.z);
                    break;
                case "down":
                    tilePosition = new Vector3Int(corridorPosition.x, corridorPosition.y-i, corridorPosition.z);
                    break;
            }
            tilemap.SetTile(tilePosition, door);
        }
       
    }
    void DrawConnectedSquares(Vector3Int centerSquareStart)
    {
         int squareSize = 0; // Tamaño de los cuadrados
        // Distancia total desde el centro hasta el inicio del cuadrado adicional
        int totalDistance = squareSize + separation;

        // Posiciones de inicio para los cuadrados adicionales
        Vector3Int[] additionalSquaresStart = new Vector3Int[]
        {
            new Vector3Int(centerSquareStart.x, centerSquareStart.y + totalDistance, 0), // Arriba
            new Vector3Int(centerSquareStart.x, centerSquareStart.y - totalDistance, 0), // Abajo
            new Vector3Int(centerSquareStart.x + totalDistance, centerSquareStart.y, 0), // Derecha
            new Vector3Int(centerSquareStart.x - totalDistance, centerSquareStart.y, 0)  // Izquierda
        };

        // Dibujar cuadrados adicionales y conectarlos
        foreach (Vector3Int squareStart in additionalSquaresStart)
        {
            System.Random rnd = new System.Random();
            squareSize = rnd.Next(10,20);
            DrawSquare(squareStart, squareSize);
            ConnectSquares(centerSquareStart, squareStart);
        }
    }



    void ConnectSquares(Vector3Int firstSquareStart, Vector3Int secondSquareStart)
    {
        int squareSize = 5;
        // Calcula los puntos medios de los lados de los cuadrados
        Vector3Int midPointFirstSquare = firstSquareStart + new Vector3Int(squareSize / 2, squareSize / 2, 0);
        Vector3Int midPointSecondSquare = secondSquareStart + new Vector3Int(squareSize / 2, squareSize / 2, 0);

        // Dibuja una línea de casillas entre los puntos medios
        Vector3Int direction = (midPointSecondSquare - midPointFirstSquare);
        int steps = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector3Int step = new Vector3Int(direction.x / steps, direction.y / steps, 0);

        for (int i = 1; i < steps; i++) // Comienza en 1 para evitar superponer el punto medio
        {
            Vector3Int tilePosition = midPointFirstSquare + step * i;
            tilemap.SetTile(tilePosition, tileToDraw);
        }
    }
}
