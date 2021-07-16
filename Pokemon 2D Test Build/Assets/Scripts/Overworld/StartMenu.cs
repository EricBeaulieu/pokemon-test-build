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
    
    GameObject lastSelected;

    public void Initialization()
    {
        //Removes all listeners to prevent the same delegate from being called multiple times
        pokeDexButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pokemonPartyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        bagButton.GetComponent<Button>().onClick.RemoveAllListeners();
        playerButton.GetComponent<Button>().onClick.RemoveAllListeners();
        saveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        optionsButton.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButton.GetComponent<Button>().onClick.RemoveAllListeners();
        lastSelected = null;

        //Setting up the last button pressed in the action button to be the first button pressed
        pokeDexButton.GetComponent<Button>().onClick.AddListener(delegate { lastSelected = pokeDexButton; });
        playerButton.GetComponent<Button>().onClick.AddListener(delegate { lastSelected = playerButton; });
        optionsButton.GetComponent<Button>().onClick.AddListener(delegate { lastSelected = optionsButton; });

        pokemonPartyButton.GetComponent<Button>().onClick.AddListener(delegate 
        {
            lastSelected = pokemonPartyButton;
            EnableStartMenu(false);
            GameManager.instance.GetPartySystem.OpenPartySystem(false);
        });
        bagButton.GetComponent<Button>().onClick.AddListener(delegate 
        {
            lastSelected = bagButton;
            EnableStartMenu(false);
            GameManager.instance.GetInventorySystem.OpenInventorySystem();
        });
        saveButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            lastSelected = saveButton;
            EnableStartMenu(false);
            GameManager.instance.SaveGame();
        });
        exitButton.GetComponent<Button>().onClick.AddListener(delegate { lastSelected = exitButton; EnableStartMenu(false); });
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
        if (lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(pokeDexButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }

    void StartMenuClosed()
    {
        GameManager.SetGameState(GameState.Overworld);
        GameManager.instance.GetPlayerController.ReturnFromStartMenu();
    }
}
