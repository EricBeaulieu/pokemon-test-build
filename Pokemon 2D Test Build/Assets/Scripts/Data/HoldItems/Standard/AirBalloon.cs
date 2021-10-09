using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloon : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AirBalloon; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AirBalloon(); }
    public override bool PlayAnimationWhenUsed() { return false; }
    public override float AlterDamageTaken(MoveBase move)
    {
        if(move.Type == ElementType.Ground)
        {
            return 0;
        }
        else
        {
            RemoveItem = true;
            return 1;
        }
    }
    public override string EntryMessage(Pokemon holder)
    {
        return $"{holder.currentName} floats in the air with its {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} {GlobalTools.SplitCamelCase(HoldItemId.ToString())} popped!";
    }

    //Pokemon name floats with its air balloon
    //pokemon name air balloon popped!
}
