using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedUser : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.CursedUser; } }
    public override string StartMessage(Pokemon pokemon)
    {
        pokemon.UpdateHP(pokemon.maxHitPoints / 2);
        pokemon.CureVolatileStatus(Id);
        return $"{pokemon.currentName} cut its own HP to lay a curse on enemy Pokemon";
    }
}