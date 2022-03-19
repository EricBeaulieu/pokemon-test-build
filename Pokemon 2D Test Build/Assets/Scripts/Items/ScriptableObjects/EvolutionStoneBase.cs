using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Evolution Stone")]
public class EvolutionStoneBase : ItemBase
{
    //added in to change trade type items over to evolutionary items
    [SerializeField] HoldItemID holdItemID = HoldItemID.NA;
    public EvolutionStoneBase()
    {
        itemType = itemType.Basic;
    }

    public override bool UseItem(Pokemon pokemon)
    {
        if(pokemon.pokemonBase.Evolutions != null)
        {
            foreach (EvolutionBase evolution in pokemon.pokemonBase.Evolutions)
            {
                if (evolution.CanEvolve(pokemon, this) == true)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public override HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(holdItemID);
    }

    public override bool UseItemOption()
    {
        return !BattleSystem.InBattle;
    }

    public override bool ShowStandardUI()
    {
        return false;
    }
}
