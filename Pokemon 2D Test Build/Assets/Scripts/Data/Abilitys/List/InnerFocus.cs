using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerFocus : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.InnerFocus; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new InnerFocus(); }
    public override string Description()
    {
        return "The Pokémon's intensely focused, and that protects the Pokémon from flinching.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD)
    {
        if (iD == ConditionID.Flinch)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName} won't flinch because of its Inner Focus!";
    }
}
