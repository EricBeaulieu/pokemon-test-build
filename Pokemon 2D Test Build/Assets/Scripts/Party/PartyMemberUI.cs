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
    [SerializeField] Image gender;

    [SerializeField] Image pokemonSprite;
    Sprite[] _animatedSprite;
    float _timer;
    float _animationTimerSwitch = 0.3f;
    int _currentIndex;
    [SerializeField] Image heldItem;

    [SerializeField] Image background;

    [SerializeField] GameObject selectorOn;
    [SerializeField] GameObject selectorOff;

    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _animationTimerSwitch)
        {
            _currentIndex = (_currentIndex + 1) % _animatedSprite.Length;
            pokemonSprite.sprite = _animatedSprite[_currentIndex];
            _timer = 0;
        }
    }

    public void SetData(Pokemon currentPokemon,int slotPosition)
    {
        _pokemon = currentPokemon;
        if(slotPosition == 0)
        {
            _isFirstSlot = true;
        }

        nameText.text = currentPokemon.currentName;
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHPWithoutAnimation(currentPokemon.currentHitPoints,currentPokemon.maxHitPoints);
        currentHP.text = $"{currentPokemon.currentHitPoints.ToString()}/{currentPokemon.maxHitPoints.ToString()}";
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot);
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);

        _animatedSprite = currentPokemon.pokemonBase.GetAnimatedSprites();
        _currentIndex = 0;
        pokemonSprite.sprite = _animatedSprite[_currentIndex];
        //until items are implimented
        heldItem.sprite = StatusConditionArt.instance.Nothing;

        Deselected();
    }

    public void OnSelect(BaseEventData eventData)
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, true);
        EnableSelector(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    public Pokemon CurrentPokemon()
    {
        return _pokemon;
    }

    void Deselected()
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, false);
        EnableSelector(false);
    }

    void EnableSelector(bool enabled)
    {
        selectorOn.SetActive(enabled);
        selectorOff.SetActive(!enabled);
    }
}
