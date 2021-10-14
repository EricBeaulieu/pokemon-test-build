using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloon : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AirBalloon; } }
    public override bool PlayAnimationWhenUsed() { return false; }
    public override float AlterDamageTaken(BattleUnit holder,MoveBase move, bool superEffective)
    {
        if(move.Type == ElementType.Ground)
        {
            return 0;
        }
        else
        {
            holder.removeItem = true;
            return 1;
        }
    }
    public override string EntryMessage(Pokemon holder)
    {
        return $"{holder.currentName} floats in the air with its {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} {GlobalTools.SplitCamelCase(HoldItemId.ToString())} popped!";
    }
}
