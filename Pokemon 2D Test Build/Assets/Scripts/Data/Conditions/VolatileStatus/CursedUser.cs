using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedUser : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.CursedUser; } }
    public override void OnStart(Pokemon pokemon)
    {
        pokemon.UpdateHP(pokemon.maxHitPoints / 2);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} cut its own HP to lay a curse on enemy Pokemon");
        pokemon.CureVolatileStatus(ConditionID.CursedUser);
    }
}
