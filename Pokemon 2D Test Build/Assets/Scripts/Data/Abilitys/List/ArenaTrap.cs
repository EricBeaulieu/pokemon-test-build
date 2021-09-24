using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrap : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ArenaTrap; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ArenaTrap(); }
    public override string Description()
    {
        return "Prevents opposing Pok�mon from fleeing.";
    }
    public override bool PreventFoeFromEscapingBattle(BattleUnit opposingUnit)
    {
        if(opposingUnit.pokemon.pokemonBase.IsType(ElementType.Flying) || opposingUnit.pokemon.ability.Id == AbilityID.Levitate)
        {
            return false;
        }
        return true;
    }
}
