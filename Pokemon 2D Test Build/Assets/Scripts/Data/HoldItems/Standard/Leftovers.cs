using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leftovers : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.Leftovers; } }
    public override void OnTurnEnd(Pokemon defendingPokemon)
    {
        if (defendingPokemon.currentHitPoints == defendingPokemon.maxHitPoints)
        {
            return;
        }

        int hpHealed = Mathf.FloorToInt(defendingPokemon.maxHitPoints / 16);

        if (hpHealed <= 0)
        {
            hpHealed = 1;
        }

        defendingPokemon.UpdateHPRestored(hpHealed);
        defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName} restored HP using Leftovers!");
    }
}
