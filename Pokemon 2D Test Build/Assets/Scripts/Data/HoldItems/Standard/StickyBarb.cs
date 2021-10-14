using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBarb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.StickyBarb; } }
    public override void OnTurnEnd(Pokemon defendingPokemon)
    {
        int damageDealt = Mathf.CeilToInt(defendingPokemon.maxHitPoints / 8);

        if (damageDealt <= 0)
        {
            damageDealt = 1;
        }

        defendingPokemon.UpdateHPDamage(damageDealt);
        defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName} was damaged by Sticky Barb");
    }
    public override bool TransferToPokemon(MoveBase move)
    {
        return move.PhysicalContact;
    }
}
