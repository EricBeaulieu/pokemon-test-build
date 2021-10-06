using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedBody : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.CursedBody; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new CursedBody(); }
    public override string Description()
    {
        return "May disable a move used on the Pokémon.";
    }
    public override bool DisableMove(BattleUnit sourceUnit, Move move)
    {
        if(sourceUnit.disabledDuration == 0 && (Random.value < 0.3f))
        {
            sourceUnit.disabledDuration = 5;
            move.disabled = true;
            return true;
        }
        return base.DisableMove(sourceUnit,move);
    }
}
