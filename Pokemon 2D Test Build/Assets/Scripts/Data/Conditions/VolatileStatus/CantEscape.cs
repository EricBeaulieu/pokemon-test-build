using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantEscape : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.CantEscape; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new CantEscape(); }
    public override string StartMessage(Pokemon pokemon,Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} can no longer escape";
    }
    public override bool PreventsEscape() { return true; }
}
