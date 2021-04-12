using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject pokeDexButton;
    [SerializeField] GameObject pokemonPartyButton;
    [SerializeField] GameObject bagButton;
    [SerializeField] GameObject playerButton;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject exitButton;

    //public event Action<bool, bool> OpenPokeDex;
    public event Action OpenPokemonParty;
    public event Action StartMenuClosed;
    GameObject _lastSelected;

    public void HandleAwake()
    {
        //Removes all listeners to prevent the same delegate from being called multiple times
        pokeDexButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pokemonPartyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        bagButton.GetComponent<Button>().onClick.RemoveAllListeners();
        playerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        optionsButton.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _lastSelected = null;

        //Setting up the last button pressed in the action button to be the first button pressed
        pokeDexButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = pokeDexButton; });
        pokemonPartyButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = pokemonPartyButton; });
        bagButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = bagButton; });
        playerButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = playerButton; });
        saveButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = saveButton; });
        optionsButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = optionsButton; });
        exitButton.GetComponent<Button>().onClick.AddListener(delegate { _lastSelected = exitButton; });

        pokemonPartyButton.GetComponent<Button>().onClick.AddListener(delegate 
        {
            EnableStartMenu(false);
            OpenPokemonParty();
        });
        exitButton.GetComponent<Button>().onClick.AddListener(delegate { EnableStartMenu(false); });
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            EnableStartMenu(false);
        }
    }

    public void EnableStartMenu(bool enabled)
    {
        gameObject.SetActive(enabled);

        if(enabled == true)
        {
            SelectBox();
        }
        else
        {
            StartMenuClosed();
        }
    }
    
    public void SelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(pokeDexButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }
}
