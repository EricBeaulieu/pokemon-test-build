using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dazzling : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Dazzling; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Dazzling(); }
    public override string Description()
    {
        return "Surprises the opposing Pokémon, making it unable to attack using priority moves.";
    }
    public override bool RemovesSpeedPriorityOfOpposingPokemon()
    {
        return true;
    }
}
