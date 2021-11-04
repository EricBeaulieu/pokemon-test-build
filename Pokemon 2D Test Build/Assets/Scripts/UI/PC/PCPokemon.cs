using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCPokemon : MonoBehaviour,ISelectHandler
{
    [SerializeField] Image pokemonSprite;
    Pokemon currentPokemon;
    [SerializeField] PCPokemonData pCPokemonData;
    [SerializeField] GameObject holditemGameobject;

    public bool DepositPokemon(Pokemon newPokemon)//Should return pokemon if there is one there
    {
        if (currentPokemon != null)
        {
            return false;
        }

        if(newPokemon == null)
        {
            currentPokemon = null;
            pokemonSprite.sprite = StatusConditionArt.instance.Nothing;
            holditemGameobject.SetActive(false);
            return false;
        }
        currentPokemon = newPokemon;
        pokemonSprite.sprite = newPokemon.pokemonBase.GetAnimatedSprites()[0];
        holditemGameobject.SetActive(currentPokemon.GetCurrentItem != null);
        return true;
    }

    public Pokemon WithdrawPokemon()
    {
        Pokemon withdrawnPokemon = currentPokemon;
        currentPokemon = null;
        pokemonSprite.sprite = StatusConditionArt.instance.Nothing;
        return withdrawnPokemon;
    }

    public void UpdateData()
    {
        if (currentPokemon != null)
        {
            pokemonSprite.sprite = currentPokemon.pokemonBase.GetAnimatedSprites()[0];
            holditemGameobject.SetActive(currentPokemon.GetCurrentItem != null);
        }
        else
        {
            pokemonSprite.sprite = StatusConditionArt.instance.Nothing;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        pCPokemonData.SetupData(currentPokemon);
    }
}
