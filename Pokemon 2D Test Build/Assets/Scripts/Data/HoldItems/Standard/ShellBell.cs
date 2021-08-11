using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBell : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ShellBell; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ShellBell(); }
    public override int AlterUserHPAfterAttack(Pokemon holder, MoveBase move, int damageDealt)
    {
        //if(holder.ability.Id == AbilityID.Sheer force)
        //{
        // return 0;
        //}

        damageDealt = Mathf.FloorToInt(holder.maxHitPoints / 8);

        if(damageDealt <= 0)
        {
            damageDealt = 1;
        }
        
        return damageDealt;
    }
}
