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
    [SerializeField] Image status;

    [SerializeField] Image pokemonSprite;
    Sprite[] _animatedSprite;
    float _timer;
    float _animationTimerSwitch = 0.3f;
    int _currentIndex;
    [SerializeField] Image heldItem;

    [SerializeField] Image background;

    [SerializeField] GameObject selectorOn;
    [SerializeField] GameObject selectorOff;
    [SerializeField] Button button;

    bool currentlySelected = false;
    bool switching = false;
    Vector3 originalPosition;
    Vector3 offsetPosition;
    const int xOffScreenPosition = 600;
    float _animationTime = 0.75f;

    public void Initialization(int slotPosition)
    {
        if (slotPosition == 0)
        {
            _isFirstSlot = true;
        }

        originalPosition = transform.localPosition;
        offsetPosition = new Vector3(transform.localPosition.x + offsetXPosDifference, transform.localPosition.y);
    }

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

    public void SetData(Pokemon currentPokemon)
    {
        _pokemon = currentPokemon;

        nameText.text = currentPokemon.currentName;
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHPWithoutAnimation(currentPokemon.currentHitPoints,currentPokemon.maxHitPoints);
        currentHP.text = $"{currentPokemon.currentHitPoints.ToString()}/{currentPokemon.maxHitPoints.ToString()}";
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot);
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);

        if(currentPokemon.currentHitPoints <= 0)
        {
            status.sprite = StatusConditionArt.instance.FaintedStatus;
        }
        else if(currentPokemon.status != null)
        {
            status.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(currentPokemon.status.Id);
        }
        else
        {
            status.sprite = StatusConditionArt.instance.Nothing;
        }

        _animatedSprite = currentPokemon.pokemonBase.GetAnimatedSprites();
        _currentIndex = 0;
        pokemonSprite.sprite = _animatedSprite[_currentIndex];
        heldItem.sprite = (currentPokemon.GetCurrentItem != null) ? PartyBackgroundArt.instance.HoldItemSprite() : StatusConditionArt.instance.Nothing;

        Deselected();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(PartySystem.GetCurrentlySwitchingPokemon == true)
        {
            background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, true, true);
        }
        else
        {
            background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, true, switching);
        }
        EnableSelector(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(currentlySelected == false)
        {
            Deselected();
        }
    }

    public Pokemon CurrentPokemon()
    {
        return _pokemon;
    }

    void Deselected()
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot, false, switching);
        EnableSelector(false);
    }

    void EnableSelector(bool enabled)
    {
        selectorOn.SetActive(enabled);
        selectorOff.SetActive(!enabled);
    }

    public Button GetButton
    {
        get { return button; }
    }

    public IEnumerator UpdateAfterItemUse(int hpRecovered = 0)
    {
        background.sprite = PartyBackgroundArt.instance.ReturnBackgroundArt(_pokemon.currentHitPoints, _isFirstSlot,true);
        if (_pokemon.currentHitPoints <= 0)
        {
            status.sprite = StatusConditionArt.instance.FaintedStatus;
        }
        else if (_pokemon.status != null)
        {
            status.sprite = StatusConditionArt.instance.ReturnStatusConditionArt(_pokemon.status.Id);
        }
        else
        {
            status.sprite = StatusConditionArt.instance.Nothing;
        }

        yield return hPBar.SetHPRecoveredAnimation(_pokemon.currentHitPoints,hpRecovered, _pokemon.maxHitPoints, currentHP);
    }

    public bool SwitchingPokemon
    {
        get { return switching; }
        set { switching = value; }
    }

    public bool isCurrentlySelected
    {
        get { return currentlySelected; }
        set { currentlySelected = value; }
    }

    public IEnumerator AnimateSwitchToStandardPosition(bool on)
    {
        if (on == true)
        {
            yield return GlobalTools.SmoothTransitionToPosition(transform, originalPosition, _animationTime);
        }
        else
        {
            yield return GlobalTools.SmoothTransitionToPosition(transform, offsetPosition, _animationTime);
        }
    }

    float offsetXPosDifference
    {
        get
        {
            if (_isFirstSlot == true)
            {
                return -xOffScreenPosition;
            }
            return xOffScreenPosition;
        }
    }
}
