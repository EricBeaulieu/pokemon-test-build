using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCoat : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.MagicCoat; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new MagicCoat(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} braced itself";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        pokemon.CureVolatileStatus(ConditionID.MagicCoat);
        return base.OnEndTurn(pokemon);
    }
    public override bool ReflectsMove() { return true; }
}
