using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    Pokemon _pokemon;

    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hPBar;
    [SerializeField] Text currentHP;

    [SerializeField] Image pokemonSprite;
    Sprite[] _animatedSprite;
    [SerializeField] Image heldItem;

    [SerializeField] Image background;

    

    public void SetData(Pokemon currentPokemon)
    {
        _pokemon = currentPokemon;

        nameText.text = currentPokemon.currentName;
        levelText.text = currentPokemon.currentLevel.ToString();
        hPBar.SetHP((float)currentPokemon.currentHitPoints / currentPokemon.maxHitPoints);
        currentHP.text = $"PP {currentPokemon.currentHitPoints.ToString()}/{currentPokemon.maxHitPoints.ToString()}";
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected", this.gameObject);

        if(_pokemon.currentHitPoints > 0)
        {
            //background.sprite = StaticPartyBackgrounds.firstPartyMemberFaintedSelectedBackground;
        }
    }
}
