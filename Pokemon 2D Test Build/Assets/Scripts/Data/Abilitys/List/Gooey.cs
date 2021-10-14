using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gooey : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Gooey; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Gooey(); }
    public override string Description()
    {
        return "Contact with the Pokémon lowers the attacker's Speed stat.";
    }
    public override StatBoost AlterStatAfterTakingDamage(MoveBase move)
    {
        if(move.PhysicalContact == true)
        {
            return new StatBoost(StatAttribute.Speed,-1);
        }
        return base.AlterStatAfterTakingDamage(move);
    }
}
