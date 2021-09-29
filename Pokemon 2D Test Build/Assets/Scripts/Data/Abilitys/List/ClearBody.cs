using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBody : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ClearBody; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ClearBody(); }
    public override string Description()
    {
        return "Prevents other Pokémon's moves or Abilities from lowering the Pokémon's stats.";
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
        return $"{pokemon.currentName}'s {Name} prevents stat loss";
    }
}
