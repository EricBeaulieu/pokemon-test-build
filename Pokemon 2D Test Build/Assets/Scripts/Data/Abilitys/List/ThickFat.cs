using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThickFat : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ThickFat; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ThickFat(); }
    public override string Description()
    {
        return "The Pokémon is protected by a layer of thick fat, which halves the damage taken from Fire- and Ice-type moves.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Fire || move.Type == ElementType.Ice)
        {
            return 0.5f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
