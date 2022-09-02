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
        return pokemon.pokemonBase.LearnableTMHMMoves.Exists(x => x == moveBase);
    }

    public override bool UseItemOption()
    {
        return !BattleSystem.InBattle;
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
        get { return GlobalArt.ReturnTMArt(moveBase.Type); }
    }

    public override bool ShowStandardUI()
    {
        return false;
    }
}
