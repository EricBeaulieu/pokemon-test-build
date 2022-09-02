using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCPokemon : MonoBehaviour, ISelectHandler
{
    [SerializeField] protected Image pokemonSprite;
    public Pokemon currentPokemon { get; private set; }
    [SerializeField] protected GameObject holditemGameobject;
    [SerializeField] Button button;
    public Button GetButton { get {return button;}}

    void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(() => { StartCoroutine(PCSystem.pointer.SelectPokemon(this)); });
            return;
        }
        Debug.Log($"This button is returning as null", gameObject);
    }

    public virtual void DepositPokemon(Pokemon newPokemon)
    {
        currentPokemon = newPokemon;
        UpdateData();
    }

    public virtual Pokemon WithdrawPokemon()
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
            pokemonSprite.sprite = GlobalArt.nothing;
            holditemGameobject.SetActive(false);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        PCSystem.pointer.MoveToPosition(transform.position);
        PCSystem.pCPokemonDataDisplay.SetupData(currentPokemon);
    }
}
