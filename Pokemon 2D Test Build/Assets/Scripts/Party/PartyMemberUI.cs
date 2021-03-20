using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Pokemon _pokemon;
    bool _isFirstSlot = false;

    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hPBar;
    [SerializeField] Text currentHP;

    [SerializeField] Image pokemonSprite;
    Sprite[] _animatedSprite;
    [SerializeField] Image heldItem;

    [SerializeField] Image background;

    public void SetData(Pokemon currentPokemon,int slotPosition)
    {
        _pokemon = currentPokemon;
        if(slotPosition == 0)
        {
            _isFirstSlot = true;
        }

        nameText.text = currentPokemon.currentName;
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        currentHP.text = $"{currentPokemon.currentHitPoints.ToString()}/{currentPokemon.maxHitPoints.ToString()}";
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot);

        _animatedSprite = currentPokemon.pokemonBase.GetAnimatedSprites();
        //Will be changed later on
        pokemonSprite.sprite = _animatedSprite[0];
    }

    public void OnSelect(BaseEventData eventData)
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, false);
    }

    public Pokemon CurrentPokemon()
    {
        return _pokemon;
    }
}
