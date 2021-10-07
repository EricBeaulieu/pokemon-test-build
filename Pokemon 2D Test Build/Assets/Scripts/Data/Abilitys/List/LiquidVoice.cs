using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidVoice : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.LiquidVoice; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new LiquidVoice(); }
    public override string Description()
    {
        return "All sound-based moves become Water-type moves.";
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if (move.SoundType == true)
        {
            move = move.Clone();
            move.AdjustedMoveType(ElementType.Water);
        }
        return base.AlterMoveDetails(move);
    }
}