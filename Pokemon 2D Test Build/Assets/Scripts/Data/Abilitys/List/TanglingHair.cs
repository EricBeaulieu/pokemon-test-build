using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanglingHair : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.TanglingHair; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new TanglingHair(); }
    public override string Description()
    {
        return "Contact with the Pokémon lowers the attacker's Speed stat.";
    }
    public override StatBoost AlterStatAfterTakingDamage(MoveBase move)
    {
        if (move.PhysicalContact == true)
        {
            return new StatBoost() { stat = StatAttribute.Speed, boost = -1 };
        }
        return base.AlterStatAfterTakingDamage(move);
    }
}
