using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryDisplay : SummaryUIBase
{
    [SerializeField] Text pokemonLevel;
    [SerializeField] Text currentName;
    Sprite[] _pokemonSpriteAnimations;
    [SerializeField] Image pokemonSprite;
    [SerializeField] Image gender;
    [SerializeField] Image status;
    [SerializeField] Image shinyStar;
    [SerializeField] Image pokeballSprite;

    Coroutine _animatedCoroutine = null;

    const float ENTRY_SPRITE_ANIMATION_SPEED = 0.5f;

    public override float offsetXPosDifference()
    {
        return -GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void SetupData(Pokemon pokemon)
    {
        pokemonLevel.text = $"Lv{pokemon.currentLevel.ToString()}";
        currentName.text = $"{pokemon.currentName}";
        _pokemonSpriteAnimations = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender);
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);

        if (pokemon.currentHitPoints <= 0)
        {
            status.sprite = StatusConditionArt.instance.FaintedStatus;
        }
        else if (pokemon.status != null)
        {
            status.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(pokemon.status.Id);
        }
        else
        {
            status.sprite = StatusConditionArt.instance.Nothing;
        }

        shinyStar.sprite = StatusConditionArt.instance.ReturnShinyStatus(pokemon.isShiny);
        pokeballSprite.sprite = pokemon.pokeballCapturedIn.ItemSprite;

        if (_animatedCoroutine != null)
        {
            StopCoroutine(_animatedCoroutine);
        }
        _animatedCoroutine = StartCoroutine(AnimateSprite());
    }
    
    IEnumerator AnimateSprite()
    {
        pokemonSprite.sprite = _pokemonSpriteAnimations[1];
        yield return new WaitForSeconds(ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonSprite.sprite = _pokemonSpriteAnimations[0];
        yield return new WaitForSeconds(ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonSprite.sprite = _pokemonSpriteAnimations[1];
        yield return new WaitForSeconds(ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonSprite.sprite = _pokemonSpriteAnimations[0];
    }
}
