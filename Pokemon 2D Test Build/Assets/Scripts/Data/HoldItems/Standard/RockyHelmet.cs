using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyHelmet : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.RockyHelmet; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new RockyHelmet(); }
    public override int AlterUserHPAfterAttack(Pokemon holder, MoveBase move, int damageDealt)
    {
        if (move.MoveType == MoveType.Physical)
        {
            return Mathf.CeilToInt(holder.maxHitPoints / 6);
        }
        return base.AlterUserHPAfterAttack(holder, move, damageDealt);
    }
}
