using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCPokemonData : MonoBehaviour
{
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonNameText;
    [SerializeField] Text pokemonLevelText;
    [SerializeField] Image pokemonGenderSprite;
    [SerializeField] Text pokemonItemText;

    public void SetupData(Pokemon currentPokemon)
    {
        if(currentPokemon == null)
        {
            pokemonSprite.sprite = StatusConditionArt.instance.Nothing;
            pokemonNameText.text = "";
            pokemonLevelText.text = "";

            pokemonGenderSprite.sprite = StatusConditionArt.instance.Nothing;
            pokemonItemText.text = "";
        }
        else
        {
            pokemonSprite.sprite = currentPokemon.pokemonBase.GetFrontSprite(currentPokemon.isShiny, currentPokemon.gender)[0];
            pokemonNameText.text = $"{currentPokemon.currentName} \n /{currentPokemon.pokemonBase.GetPokedexName()}";
            pokemonLevelText.text = $"Lv {currentPokemon.currentLevel.ToString()}";

            pokemonGenderSprite.sprite = StatusConditionArt.instance.ReturnGenderArt(currentPokemon.gender);
            pokemonItemText.text = (currentPokemon.GetCurrentItem == null) ? "" : currentPokemon.GetCurrentItem.ItemName;
        }
    }
}
