using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBubble : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.WaterBubble; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new WaterBubble(); }
    public override string Description()
    {
        return "Lowers the power of Fire-type moves done to the Pokémon and prevents the Pokémon from getting a burn.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD, WeatherEffectID weather)
    {
        if (iD == ConditionID.Burn)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD,weather);
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Water)
        {
            return 0.5f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
