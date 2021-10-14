using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyHelmet : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.RockyHelmet; } }
    public override bool PlayAnimationWhenUsed() { return false; }
    public override bool HurtsAttacker()
    {
        return true;
    }
    public override int AlterUserHPAfterAttack(BattleUnit holder, MoveBase move, int damageDealt)
    {
        if (move.PhysicalContact == true)
        {
            return -Mathf.CeilToInt(holder.pokemon.maxHitPoints / 6);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{opposingPokemon.currentName} lost some of its HP due to {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
