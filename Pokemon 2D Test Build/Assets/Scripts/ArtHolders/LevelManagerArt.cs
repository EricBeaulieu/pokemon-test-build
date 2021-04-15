using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerArt : MonoBehaviour
{
    static LevelManagerArt _instance = null;

    [Header("Grassy Field")]
    [SerializeField] Sprite grassyFieldBackground;
    [SerializeField] Sprite grassyFieldPlayerPosition;
    [SerializeField] Sprite grassyFieldEnemyPosition;

    public static LevelManagerArt instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public LevelArtDetails GetArtDetails(levelArea currentArea)
    {
        return new LevelArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //switch (currentArea)
        //{
        //    case levelArea.GrassyField:
        //        return new LevelArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //    default:
        //        return new LevelArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //}
    }
}
