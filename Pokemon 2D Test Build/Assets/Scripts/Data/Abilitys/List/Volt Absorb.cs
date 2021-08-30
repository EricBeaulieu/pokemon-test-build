using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltAbsorb : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.VoltAbsorb; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new VoltAbsorb(); }
    public override string Description()
    {
        return "Restores HP if hit by a Electric-type move instead of taking damage.";
    }
    public override float AlterDamageTaken(Pokemon defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Electric)
        {
            int hpHealed = Mathf.FloorToInt(defendingPokemon.maxHitPoints * 0.25f);
            hpHealed = Mathf.Clamp(hpHealed, 0, defendingPokemon.maxHitPoints - defendingPokemon.currentHitPoints);
            if (hpHealed == 0)
            {
                defendingPokemon.statusChanges.Enqueue($"It Doesn't Affect {defendingPokemon.currentName}");
                return 0;
            }
            defendingPokemon.UpdateHPRestored(hpHealed);
            defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName} restored HP using its {GlobalTools.SplitCamelCase(Id.ToString())}!");
            return 0;
        }

        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}