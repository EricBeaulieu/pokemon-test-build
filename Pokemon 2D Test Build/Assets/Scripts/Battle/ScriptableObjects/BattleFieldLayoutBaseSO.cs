using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Create New Battle Environment Art Layout")]
public class BattleFieldLayoutBaseSO : ScriptableObject
{
    [SerializeField] Sprite background;
    [SerializeField] Sprite playerPosition;
    [SerializeField] Sprite enemyPosition;

    public Sprite GetBackground
    {
        get { return background; }
    }

    public Sprite GetPlayerPosition
    {
        get { return playerPosition; }
    }

    public Sprite GetEnemyPosition
    {
        get { return enemyPosition; }
    }
}
