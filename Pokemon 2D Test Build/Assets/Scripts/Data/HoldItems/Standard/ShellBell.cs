using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBell : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ShellBell; } }
    public override int AlterUserHPAfterAttack(BattleUnit holder, MoveBase move, int damageDealt)
    {
        //if(holder.ability.Id == AbilityID.Sheer force)
        //{
        // return 0;
        //}

        damageDealt = Mathf.FloorToInt(holder.pokemon.maxHitPoints / 8);

        if(damageDealt <= 0)
        {
            damageDealt = 1;
        }
        
        return damageDealt;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} restored HP using {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
