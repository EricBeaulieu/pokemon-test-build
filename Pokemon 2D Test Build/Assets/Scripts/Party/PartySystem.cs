using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySystem : MonoBehaviour
{
    [SerializeField] Text messageBox;

    PartyMemberUI[] _partyMemberSlots;
    [SerializeField] Button cancelButton;

    //Clean this up later, just get it working for now and then clean up the code later
    public BattleSystem battleSystemReference { get; set; }
    public Action onCloseParty;
    Pokemon _currentlySelectedPokemon;

    [SerializeField] GameObject overworldSelections;
    [SerializeField] GameObject overworldSelectionsSummaryButton;
    [SerializeField] GameObject overworldSelectionsSwitchButton;
    [SerializeField] GameObject overworldSelectionsItemButton;
    [SerializeField] GameObject overworldSelectionsCancelButton;

    [SerializeField] GameObject battleSelections;
    [SerializeField] GameObject battleSelectionShiftButton;
    [SerializeField] GameObject battleSelectionSummaryButton;
    [SerializeField] GameObject battleSelectionCancelButton;

    const int MESSAGEBOX_STANDARD_SIZE = 650;
    const int MESSAGEBOX_SELECTED_SIZE = 515;

    void Awake()
    {
        _partyMemberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void OpenPartySystem()
    {
        SelectFirstBox();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        SetMessageText("Choose a Pokemon");

        SetUpPartySystemCancelButton();
    }

    void ClosePartySystem()
    {
        //this.gameObject.SetActive(false);
        onCloseParty();
    }

    void SelectFirstBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_partyMemberSlots[0].gameObject);
    }

    void SetUpPartySystemCancelButton()
    {
        if(battleSystemReference.GetCurrentPokemonInBattle.currentHitPoints > 0 || battleSystemReference.GetCurrentPokemonInBattle == null)
        {
            cancelButton.onClick.AddListener(() => 
            {
                ClosePartySystem();
                battleSystemReference.ReturnFromPokemonPartySystem();
            });
        }
        else
        {
            cancelButton.onClick.AddListener(() => SetMessageText($"{battleSystemReference.GetCurrentPokemonInBattle.currentName} can no longer battle"));
        }
    }

    public void SetPartyData(List<Pokemon> currentParty,bool inBattle)
    {
        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if(i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i], i);

                if (inBattle)
                {
                    int k = i;
                    _partyMemberSlots[k].GetComponent<Button>().onClick.RemoveAllListeners();
                    _partyMemberSlots[k].GetComponent<Button>().onClick.AddListener(() => OpenBattleSelections(_partyMemberSlots[k]));
                }

                if(i+1 == currentParty.Count)
                {
                    //have to pull the variable out, then change it and set it back in
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = cancelButton;
                    _partyMemberSlots[i].GetComponent<Button>().navigation = navigation;

                    //CancelButton Up
                    navigation = cancelButton.GetComponent<Button>().navigation;
                    navigation.selectOnUp = _partyMemberSlots[i].GetComponent<Button>();
                    cancelButton.GetComponent<Button>().navigation = navigation;
                }
                else
                {
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = _partyMemberSlots[i+1].GetComponent<Button>();
                    _partyMemberSlots[i].GetComponent<Button>().navigation = navigation;
                }
            }
            else
            {
                _partyMemberSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetMessageText(string dialog)
    {
        messageBox.text = dialog;
    }

    public void AdjustMessageBoxWidthSize(int size)
    {
        //get component in parent returns the same object since they both share rect transform
        RectTransform rt = messageBox.transform.parent.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(size, rt.sizeDelta.y);
    }

    void OpenBattleSelections(PartyMemberUI currentPartyMember)
    {
        battleSelections.SetActive(true);
        EventSystem.current.SetSelectedGameObject(battleSelectionShiftButton);
        AdjustMessageBoxWidthSize(MESSAGEBOX_SELECTED_SIZE);

        battleSelectionShiftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionShiftButton.GetComponent<Button>().onClick.AddListener(() => ShiftBattleButton(currentPartyMember));
        //Set Shift Button Here
        //if they choose the same pokemon thats out then they cannot switch them
        //If they choose a fainted pokemon they cannot switch them

        //Set Summary Button Here
        battleSelectionCancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionCancelButton.GetComponent<Button>().onClick.AddListener(() => CancelBattleButton(currentPartyMember));
    }

    void ShiftBattleButton(PartyMemberUI currentPartyMember)
    {
        if (currentPartyMember.CurrentPokemon().currentHitPoints <= 0)
        {
            SetMessageText($"{currentPartyMember.CurrentPokemon().currentName} has no energy left to battle!");
            return;
        }

        if(currentPartyMember.CurrentPokemon() == battleSystemReference.GetCurrentPokemonInBattle)
        {
            SetMessageText($"{currentPartyMember.CurrentPokemon().currentName} is already in battle!");
            return;
        }

        battleSystemReference.PlayerSwitchPokemon(currentPartyMember.CurrentPokemon());
        ClosePartySystem();
    }

    void CancelBattleButton(PartyMemberUI previousSelection)
    {
        battleSelections.SetActive(false);
        EventSystem.current.SetSelectedGameObject(previousSelection.gameObject);
    }
}
