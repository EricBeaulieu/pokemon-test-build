using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPecks : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.BigPecks; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new BigPecks(); }
    public override string Description()
    {
        return "Protects the Pokémon from Defense-lowering effects.";
    }
    public override bool PreventStatFromBeingLowered(StatAttribute stat)
    {
        if (stat == StatAttribute.Defense)
        {
            return true;
        }
        return base.PreventStatFromBeingLowered(stat);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} prevents Defense loss";
    }
}
