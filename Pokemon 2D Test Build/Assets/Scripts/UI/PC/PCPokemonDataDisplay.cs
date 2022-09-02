using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCPokemonDataDisplay : MonoBehaviour
{
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonNameText;
    [SerializeField] Text pokemonLevelText;
    [SerializeField] Image pokemonGenderSprite;
    [SerializeField] Text pokemonItemText;

    public void SetupData(Pokemon currentPokemon)
    {
        if(PCSystem.pointer.currentPokemon != null)
        {
            return;
        }

        if (currentPokemon == null)
        {
            pokemonSprite.sprite = GlobalArt.nothing;
            pokemonNameText.text = "";
            pokemonLevelText.text = "";
            pokemonGenderSprite.sprite = GlobalArt.nothing;
            pokemonItemText.text = "";
        }
        else
        {
            pokemonSprite.sprite = currentPokemon.pokemonBase.GetFrontSprite(currentPokemon.isShiny, currentPokemon.gender)[0];
            pokemonNameText.text = $"{currentPokemon.currentName} \n /{currentPokemon.pokemonBase.GetPokedexName()}";
            pokemonLevelText.text = $"Lv {currentPokemon.currentLevel.ToString()}";
            pokemonGenderSprite.sprite = GlobalArt.ReturnGenderArt(currentPokemon.gender);
            pokemonItemText.text = (currentPokemon.GetCurrentItem == null) ? "" : currentPokemon.GetCurrentItem.ItemName;
        }
    }
}
