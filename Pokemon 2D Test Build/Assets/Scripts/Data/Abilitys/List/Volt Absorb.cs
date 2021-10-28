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
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Electric)
        {
            int hpHealed = Mathf.FloorToInt(defendingPokemon.pokemon.maxHitPoints * 0.25f);
            hpHealed = Mathf.Clamp(hpHealed, 0, defendingPokemon.pokemon.maxHitPoints - defendingPokemon.pokemon.currentHitPoints);
            if (hpHealed == 0)
            {
                defendingPokemon.pokemon.statusChanges.Enqueue($"It Doesn't Affect {defendingPokemon.pokemon.currentName}");
                return 0;
            }
            defendingPokemon.pokemon.UpdateHPRestored(hpHealed);
            defendingPokemon.pokemon.statusChanges.Enqueue($"{defendingPokemon.pokemon.currentName} restored HP using its {Name}!");
            return 0;
        }

        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
