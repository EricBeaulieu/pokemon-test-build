using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCPokemon : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField] Image pokemonSprite;
    public Pokemon currentPokemon { get; private set; }
    [SerializeField] GameObject holditemGameobject;
    [SerializeField] Button button;

    public void DepositPokemon(Pokemon newPokemon)//Should return pokemon if there is one there
    {
        if (currentPokemon != null)
        {
            Debug.LogWarning($"tried to deposit pokemon in a position with pokemon included", gameObject);
            return;
        }

        if(newPokemon == null)
        {
            currentPokemon = null;//for the starting setter
            UpdateData();
            return;
        }
        currentPokemon = newPokemon;
        UpdateData();
    }

    public Pokemon WithdrawPokemon()
    {
        if(currentPokemon == null)
        {
            return null;
        }
        Pokemon withdrawnPokemon = currentPokemon;
        currentPokemon = null;
        UpdateData();

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
            holditemGameobject.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        PCSystem.pointer.MoveToPosition(this.transform.position);
        button.onClick.AddListener(() => { StartCoroutine(PCSystem.pointer.SelectPokemon(this)); });
        PCSystem.pCPokemonDataDisplay.SetupData(currentPokemon);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        button.onClick.RemoveAllListeners();
    }
}
