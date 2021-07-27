using System;
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

    SelectableBoxUI selectableBox;

    public void Select()
    {
        selectableBox.SelectBox();
    }

    public void Initialization(Action fight,Action bag,Action pokemon, Action run)
    {
        selectableBox = new SelectableBoxUI(fightButton);
        selectableBox.SetLastSelected(null);
        
        fightButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            selectableBox.SetLastSelected(fightButton);
            fight.Invoke();
        });
        bagButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            selectableBox.SetLastSelected(bagButton);
            bag.Invoke();
        });
        pokemonButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            selectableBox.SetLastSelected(pokemonButton);
            pokemon.Invoke();
        });
        runButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            selectableBox.SetLastSelected(runButton);
            run.Invoke();
        });
    }

    public void NewBattle()
    {
        selectableBox.SetLastSelected(null);
    }
}
