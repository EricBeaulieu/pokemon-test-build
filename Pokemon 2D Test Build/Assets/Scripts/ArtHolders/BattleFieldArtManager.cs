using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldArtManager : MonoBehaviour
{
    static BattleFieldArtManager _instance = null;

    [Header("Grassy Field")]
    [SerializeField] Sprite grassyFieldBackground;
    [SerializeField] Sprite grassyFieldPlayerPosition;
    [SerializeField] Sprite grassyFieldEnemyPosition;

    public static BattleFieldArtManager instance
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

    public BattleFieldArtDetails GetArtDetails(levelArea currentArea)
    {
        return new BattleFieldArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //switch (currentArea)
        //{
        //    case levelArea.GrassyField:
        //        return new LevelArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //    default:
        //        return new LevelArtDetails(grassyFieldBackground, grassyFieldPlayerPosition, grassyFieldEnemyPosition);
        //}
    }
}
