using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletproof : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Bulletproof; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Bulletproof(); }
    public override string Description()
    {
        return "Protects the Pokémon from some ball and bomb moves.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        //List will be implimented
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
