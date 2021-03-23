using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSelectionEventSelector : MonoBehaviour
{
    [SerializeField] GameObject fightButton;
    [SerializeField] GameObject bagButton;
    [SerializeField] GameObject pokemonButton;
    [SerializeField] GameObject runButton;

    GameObject _lastSelected;

    public void SelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(fightButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }

    public void SetUp()
    {
        //Removes all listeners to prevent the same delegate from being called multiple times
        fightButton.GetComponent<Button>().onClick.RemoveAllListeners();
        bagButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pokemonButton.GetComponent<Button>().onClick.RemoveAllListeners();
        runButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _lastSelected = null;

        //Setting up the last button pressed in the action button to be the first button pressed
        fightButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = fightButton; });
        bagButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = bagButton; });
        pokemonButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = pokemonButton; });
        runButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = runButton; });
    }

    public Button ReturnFightButton()
    {
        return fightButton.GetComponent<Button>();
    }

    public Button ReturnBagButton()
    {
        return bagButton.GetComponent<Button>();
    }

    public Button ReturnPokemonButton()
    {
        return pokemonButton.GetComponent<Button>();
    }

    public Button ReturnRunButton()
    {
        return runButton.GetComponent<Button>();
    }


}
