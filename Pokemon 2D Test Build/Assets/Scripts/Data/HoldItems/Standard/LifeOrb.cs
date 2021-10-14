using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LifeOrb; } }
    public override bool PlayAnimationWhenUsed()
    {
        return false;
    }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMovePower(0.3f);
        return base.AlterUserMoveDetails(holder, move);
    }
    public override int AlterUserHPAfterAttack(BattleUnit holder, MoveBase move, int damageDealt)
    {
        //if(holder.ability.Id == AbilityID.Sheer force)
        //{
        // return 0;
        //}

        if (move.MoveType != MoveType.Status)
        {
            return -Mathf.CeilToInt(holder.pokemon.maxHitPoints / 10);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} lost some of its HP";
    }
}
