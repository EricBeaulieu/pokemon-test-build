using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracing : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Bracing; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Bracing(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} braced itself";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        pokemon.CureVolatileStatus(ConditionID.Bracing);
        return base.OnEndTurn(pokemon);
    }
    public override bool LeavesTargetWithOneHP() { return true; }//pokemon name endured the hit!
    
}
