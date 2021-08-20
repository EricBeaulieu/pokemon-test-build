using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create New TMHM")]
public class TMHMItem : ItemBase
{
    [SerializeField] MoveBase moveBase;

    public TMHMItem()
    {
        itemType = itemType.TMHM;
    }

    public override bool UseItem(Pokemon pokemon)
    {
        return AbleOrUnableToUseOnPokemon(pokemon.pokemonBase);
    }

    public override bool UseItemOption()
    {
        return !BattleSystem.inBattle;
    }

    public MoveBase GetMove
    {
        get { return moveBase; }
    }

    public override string ItemName
    {
        get { return $"{base.ItemName} {moveBase.MoveName}"; }
    }

    public override string ItemDescription
    {
        get { return moveBase.MoveDescription; }
    }

    public override Sprite ItemSprite
    {
        get { return StatusConditionArt.instance.ReturnTMArt(moveBase.Type); }
    }

    public override bool ShowStandardUI()
    {
        return false;
    }

    public override bool AbleOrUnableToUseOnPokemon(PokemonBase pokemon)
    {
        return pokemon.LearnableTMHMMoves.Exists(x => x == moveBase);
    }
}
