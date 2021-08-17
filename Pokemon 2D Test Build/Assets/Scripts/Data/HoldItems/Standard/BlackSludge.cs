using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSludge : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BlackSludge; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BlackSludge(); }
    public override void OnTurnEnd(Pokemon defendingPokemon)
    {
        int damage = Mathf.FloorToInt(defendingPokemon.maxHitPoints / 16);

        if (damage <= 0)
        {
            damage = 1;
        }

        if (defendingPokemon.pokemonBase.IsType(ElementType.Poison) == true)
        {
            if (defendingPokemon.currentHitPoints == defendingPokemon.maxHitPoints)
            {
                return;
            }
            defendingPokemon.UpdateHPRestored(damage);
            defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName} restored HP using Black Sludge!");
        }
        else
        {
            defendingPokemon.UpdateHPDamage(damage);
            defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName} is hurt by Black Sludge");
        }
    }
}
