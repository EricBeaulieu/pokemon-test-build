using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyHelmet : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.RockyHelmet; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new RockyHelmet(); }
    public override bool HurtsAttacker()
    {
        return true;
    }
    public override int AlterUserHPAfterAttack(Pokemon holder, MoveBase move, int damageDealt)
    {
        if (move.PhysicalContact == true)
        {
            return -Mathf.CeilToInt(holder.maxHitPoints / 6);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{opposingPokemon.currentName} lost some of its HP due to {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
