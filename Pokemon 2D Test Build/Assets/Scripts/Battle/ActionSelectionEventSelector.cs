using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectionEventSelector : MonoBehaviour
{
    [SerializeField] GameObject _fightButton;
    [SerializeField] GameObject _bagButton;
    [SerializeField] GameObject _pokemonButton;
    [SerializeField] GameObject _runButton;

    public void SelectFirstBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_fightButton);
    }

    public void SelectPokemonButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_pokemonButton);
    }

    public Button ReturnFightButton()
    {
        return _fightButton.GetComponent<Button>();
    }

    public Button ReturnBagButton()
    {
        return _bagButton.GetComponent<Button>();
    }

    public Button ReturnPokemonButton()
    {
        return _pokemonButton.GetComponent<Button>();
    }

    public Button ReturnRunButton()
    {
        return _runButton.GetComponent<Button>();
    }
}
