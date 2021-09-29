using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeenEye : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.KeenEye; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new KeenEye(); }
    public override string Description()
    {
        return "Keen eyes prevent other Pokémon from lowering this Pokémon's accuracy.";
    }
    public override bool PreventStatFromBeingLowered(StatAttribute stat)
    {
        if (stat == StatAttribute.Accuracy)
        {
            return true;
        }
        return base.PreventStatFromBeingLowered(stat);
    }
    public override bool IgnoreStatIncreases(StatAttribute stat)
    {
        if (stat == StatAttribute.Evasion)
        {
            return true;
        }
        return base.IgnoreStatIncreases(stat);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} prevents Accuracy loss";
    }
}
