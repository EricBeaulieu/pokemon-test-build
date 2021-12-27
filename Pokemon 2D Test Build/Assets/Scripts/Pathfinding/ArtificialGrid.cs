using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArtificialGrid
{
    static int width;
    static int height;
    static Vector2 offset;

    public static Pathfinding Pathfinding { get; } = new Pathfinding();
    static LevelManager levelManager;

    static bool[,] gridArray;

    public static void SetupGrid(bool waterOnly = false)
    {
        if(levelManager == SceneSystem.currentLevelManager)
        {
            return;
        }

        levelManager = SceneSystem.currentLevelManager;

        width = Mathf.CeilToInt(levelManager.transform.localScale.x);
        height = Mathf.CeilToInt(levelManager.transform.localScale.y);
        offset = new Vector2(levelManager.transform.position.x - ((float)width / 2), levelManager.transform.position.y - ((float)height / 2));
        if (GameManager.instance.debugGrid == true)
        {
            Debug.DrawLine(offset, new Vector2(offset.x + width, offset.y), Color.red, 50f);//bottom left to bottom right
            Debug.DrawLine(new Vector2(offset.x + width, offset.y), new Vector2(offset.x + width, offset.y + height), Color.red, 50f);//bottom right to top right
            Debug.DrawLine(new Vector2(offset.x + width, offset.y + height), new Vector2(offset.x, offset.y + height), Color.red, 50f);//top right to top left
            Debug.DrawLine(new Vector2(offset.x, offset.y + height), offset, Color.red, 50f);//top left to bottom left
        }

        gridArray = new bool[width, height];
        if (GameManager.instance.debugGrid == true)
        {
            GameManager.instance.ClearGrid();
        }

        int[] walkableLocations = levelManager.GetGrid.WalkableLocation(width,waterOnly);
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x,y] = walkableLocations.Contains(x * width + y);
                if(GameManager.instance.debugGrid == true)
                {
                    GameManager.instance.CreateGridCell(x, y, new Vector2(offset.x + x, offset.y + y),gridArray[x,y]);
                }
            }
        }
    }

    public static bool[,] GetGrid
    {
        get { return gridArray; }
    }

    public static Vector2 GetWorldPositionOffset()
    {
        return offset;
    }
}
