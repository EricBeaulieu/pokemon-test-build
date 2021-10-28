using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluffy : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Fluffy; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Fluffy(); }
    public override string Description()
    {
        return "Halves the damage taken from moves that make direct contact, but doubles that of Fire-type moves.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Fire)
        {
            return 2f;
        }

        if(move.MoveType == MoveType.Physical)
        {
            return 0.5f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
