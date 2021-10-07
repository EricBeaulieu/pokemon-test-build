using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongReach : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.LongReach; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new LongReach(); }
    public override string Description()
    {
        return "The Pokémon uses its moves without making contact with the target.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.PhysicalContact == true)
        {
            move = move.Clone();
            move.RemoveContact();
        }
        return base.AlterMoveDetails(move);
    }
}