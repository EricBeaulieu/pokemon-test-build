using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSash : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FocusSash; } }
    public override bool EndureOHKOAttack(BattleUnit holder)
    {
        if (holder.pokemon.currentHitPoints == holder.pokemon.maxHitPoints)
        {
            holder.removeItem = true;
            return true;
        }

        return false;
    }
}
