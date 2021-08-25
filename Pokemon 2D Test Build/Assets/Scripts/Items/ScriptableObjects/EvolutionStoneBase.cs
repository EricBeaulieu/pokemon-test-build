using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New Evolution Stone")]
public class EvolutionStoneBase : ItemBase
{
    public EvolutionStoneBase()
    {
        itemType = itemType.Basic;
    }

    public override bool UseItem(Pokemon pokemon)
    {
        if(pokemon.pokemonBase.EvolutionsByStone != null)
        {
            foreach (EvolutionStone evolution in pokemon.pokemonBase.EvolutionsByStone)
            {
                if (evolution.CanEvolve(pokemon, this) == true)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public override bool UseItemOption()
    {
        return !BattleSystem.inBattle; ;
    }

    public override bool ShowStandardUI()
    {
        return false;
    }
}
