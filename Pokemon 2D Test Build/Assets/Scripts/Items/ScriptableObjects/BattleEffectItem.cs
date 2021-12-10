using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Battle Item")]
public class BattleEffectItem : ItemBase
{
    [Header("Battle Attributes")]
    [SerializeField] StatAttribute stat;

    public BattleEffectItem()
    {
        itemType = itemType.Battle;
    }

    public override bool UseItem(Pokemon pokemon)
    {
        return false;
    }

    public override bool UseItemOption()
    {
        return BattleSystem.InBattle;
    }

    public StatAttribute GetStatAttribute { get { return stat; } }
}
