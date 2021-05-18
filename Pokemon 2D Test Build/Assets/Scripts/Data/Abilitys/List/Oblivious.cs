using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oblivious : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Oblivious; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Oblivious(); }
    public override string Description()
    {
        return "The Pokémon is oblivious, and that keeps it from being infatuated or falling for taunts.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD)
    {
        if (iD == ConditionID.Infatuation)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is Oblivious to Inflantuation and Taunts";
    }
}
