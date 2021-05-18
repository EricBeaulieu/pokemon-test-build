using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperCutter : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.HyperCutter; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new HyperCutter(); }
    public override string Description()
    {
        return "The Pokémon's proud of its powerful pincers. They prevent other Pokémon from lowering its Attack stat.";
    }
    public override bool PreventStatFromBeingLowered(StatAttribute stat)
    {
        if (stat == StatAttribute.Attack)
        {
            return true;
        }
        return base.PreventStatFromBeingLowered(stat);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s Hyper Cutter prevents Attack loss";
    }
}
