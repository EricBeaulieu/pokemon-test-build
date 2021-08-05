using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSash : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.FocusSash; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FocusSash(); }
    public override bool EndureOHKOAttack(Pokemon defendingPokemon)
    {
        if (defendingPokemon.currentHitPoints == defendingPokemon.maxHitPoints)
        {
            RemoveItem = true;
            return true;
        }

        return false;
    }
}
