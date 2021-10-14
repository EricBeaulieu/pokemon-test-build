using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Download : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Download; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Download(); }
    public override string Description()
    {
        return "Compares an opposing Pok�mon's Defense and Sp. Def stats before raising its own Attack or Sp. Atk stat�whichever will be more effective.";
    }
    public override StatBoost OnEntryRaiseStat(Pokemon opposingPokemon)
    {
        if(opposingPokemon.defense > opposingPokemon.specialDefense)
        {
            return new StatBoost(StatAttribute.Attack,1);
        }
        return new StatBoost(StatAttribute.SpecialAttack,1);
    }
}
