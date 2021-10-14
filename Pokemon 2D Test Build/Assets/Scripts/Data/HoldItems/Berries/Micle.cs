using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Micle : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Micle; } }
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.Type == ElementType.Bug && move.MoveType != MoveType.Status)
        {
            holder.removeItem = true;
            move = move.Clone();
            move.AdjustedMoveAccuracy(100);
        }
        return base.AlterUserMoveDetails(holder, move);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} raised their attacks accuracy using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
}