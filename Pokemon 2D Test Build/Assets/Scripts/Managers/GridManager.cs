using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    Tilemap background;
    Tilemap water;
    Tilemap solidObjects;

    void Start()
    {
        background = transform.Find("Background").GetComponent<Tilemap>();
        water = transform.Find("Water").GetComponent<Tilemap>();
        solidObjects = transform.Find("SolidObjects").GetComponent<Tilemap>();
    }

    List<Vector2Int> GetPosition(Tilemap currentTilemap)
    {
        List<Vector2Int> tilemapPosition = new List<Vector2Int>();
        foreach (var position in currentTilemap.cellBounds.allPositionsWithin)
        {
            if (!currentTilemap.HasTile(position))
            {
                continue;
            }
            tilemapPosition.Add(new Vector2Int(position.x, position.y));
        }
        return tilemapPosition;
    }

    public List<Vector2Int> GetSolidObjectPosition()
    {
        return GetPosition(solidObjects);
    }

    public List<Vector2Int> GetWaterPosition()
    {
        return GetPosition(water);
    }

    public List<Vector2Int> GetStandardPosition()
    {
        return GetPosition(background);
    }

    public int[] WalkableLocation(int width,bool waterOnly = false)
    {
        Vector2 worldPosOffset = ArtificialGrid.GetWorldPositionOffset();

        Tilemap currentTilemap;
        if (waterOnly == true)
        {
            currentTilemap = water;
        }
        else
        {
            currentTilemap = background;
        }

        List<int> walkablePositions = new List<int>();
        foreach (var position in currentTilemap.cellBounds.allPositionsWithin)
        {
            Vector3 posOffset = new Vector3(position.x + 0.25f, position.y, 0);
            if(GameManager.instance.debugGrid == true)
            {
                Debug.DrawLine(position, posOffset,Color.red,50f);
            }
            if (currentTilemap.HasTile(position) == false || solidObjects.HasTile(position) == true)
            {
                continue;
            }
            walkablePositions.Add((Mathf.FloorToInt(position.x - worldPosOffset.x) * width) + Mathf.FloorToInt(position.y - worldPosOffset.y));
        }
        return walkablePositions.ToArray();
    }
}
