using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundproof : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Soundproof; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Soundproof(); }
    public override string Description()
    {
        return "Soundproofing gives the Pokémon full immunity to all sound-based moves.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.SoundType == true)
        {
            return 0;
        }

        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
