using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaboca : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Jaboca; } }
    public override bool HurtsAttacker()
    {
        return true;
    }
    public override int AlterUserHPAfterAttack(BattleUnit holder, MoveBase move, int damageDealt)
    {
        if (move.MoveType == MoveType.Physical)
        {
            holder.removeItem = true;
            return -Mathf.CeilToInt(holder.pokemon.maxHitPoints / 8);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} used their {GlobalTools.SplitCamelCase(BerryId.ToString())} berry to damage {opposingPokemon.currentName}!";
    }
}