using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normalize : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Normalize; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Normalize(); }
    public override string Description()
    {
        return "All the Pokémon's moves become Normal type. The power of those moves is boosted a little.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        move = move.Clone();
        move.AdjustedMoveType(ElementType.Normal);
        move.AdjustedMovePower(0.2f);
        return move;
    }
}
