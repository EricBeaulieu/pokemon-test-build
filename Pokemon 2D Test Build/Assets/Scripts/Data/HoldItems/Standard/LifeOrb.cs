using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LifeOrb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LifeOrb(); }
    public override bool PlayAnimationWhenUsed()
    {
        return false;
    }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMovePower(0.3f);
        return base.AlterUserMoveDetails(move);
    }
    public override int AlterUserHPAfterAttack(Pokemon holder, MoveBase move, int damageDealt)
    {
        //if(holder.ability.Id == AbilityID.Sheer force)
        //{
        // return 0;
        //}

        if (move.MoveType != MoveType.Status)
        {
            return -Mathf.CeilToInt(holder.maxHitPoints / 10);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} lost some of its HP";
    }
}
