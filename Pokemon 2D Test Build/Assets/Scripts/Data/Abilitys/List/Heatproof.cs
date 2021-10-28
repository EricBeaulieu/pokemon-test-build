using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatproof : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Heatproof; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Heatproof(); }
    public override string Description()
    {
        return "The heatproof body of the Pokémon halves the damage from Fire-type moves that hit it.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if(iD == ConditionID.Burn)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if(move.Type == ElementType.Fire)
        {
            return 0.5f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
