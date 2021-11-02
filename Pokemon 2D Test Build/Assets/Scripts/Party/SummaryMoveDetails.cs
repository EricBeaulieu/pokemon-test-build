using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryMoveDetails : SummaryUIBase
{
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonName;
    [SerializeField] Image gender;
    [SerializeField] Image type1;
    [SerializeField] Image type2;

    protected override void Awake()
    {
        base.Awake();
        _animationTime /= 2;
    }

    public override float offsetXPosDifference()
    {
        return -GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void SetupData(Pokemon pokemon)
    {
        pokemonSprite.sprite = pokemon.pokemonBase.GetAnimatedSprites()[0];
        pokemonName.text = pokemon.currentName;
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);
        type1.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType1);
        type2.sprite = (pokemon.pokemonBase.pokemonType2 != ElementType.NA) ?
            StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType2) : StatusConditionArt.instance.Nothing;
    }
}
