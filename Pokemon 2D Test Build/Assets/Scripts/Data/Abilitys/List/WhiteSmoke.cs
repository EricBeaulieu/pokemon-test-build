using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSmoke : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.WhiteSmoke; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new WhiteSmoke(); }
    public override string Description()
    {
        return "The Pokémon is protected by its white smoke, which prevents other Pokémon from lowering its stats.";
    }
    public override bool PreventStatFromBeingLowered(StatAttribute stat)
    {
        if (stat != StatAttribute.NA)
        {
            return true;
        }
        return base.PreventStatFromBeingLowered(stat);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s White Smoke prevents stat loss";
    }
}
