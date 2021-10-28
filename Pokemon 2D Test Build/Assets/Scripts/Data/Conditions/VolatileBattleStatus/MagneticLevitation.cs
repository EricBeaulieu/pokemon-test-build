using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticLevitation : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.MagneticLevitation; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new MagneticLevitation(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = 5;
        return $"{pokemon.currentName} levitated with electromagnetism";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if (StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName}'s electromagnetism wore off");
        }
        return base.OnEndTurn(pokemon);
    }
}
