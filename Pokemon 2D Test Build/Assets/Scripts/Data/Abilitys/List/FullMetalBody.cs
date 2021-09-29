using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMetalBody : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FullMetalBody; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FullMetalBody(); }
    public override string Description()
    {
        return "Prevents other Pok�mon's moves or Abilities from lowering the Pok�mon's stats.";
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
        return $"{pokemon.currentName}'s {Name} Body prevents stat loss";
    }
}
