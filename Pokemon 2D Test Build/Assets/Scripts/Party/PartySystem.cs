using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySystem : MonoBehaviour
{
    [SerializeField] Text messageBox;
    [SerializeField] SummarySystem summarySystem;

    PartyMemberUI[] _partyMemberSlots;
    [SerializeField] Button cancelButton;

    BattleSystem battleSystemReference;
    InventorySystem inventorySystemReference;
    public Action onCloseParty;
    List<Pokemon> _currentParty;
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

    GameObject _lastSelected;

    const int MESSAGEBOX_STANDARD_SIZE = 650;
    const int MESSAGEBOX_SELECTED_SIZE = 515;

    public void Initialization(BattleSystem battleSystem, InventorySystem inventorySystem)
    {
        battleSystemReference = battleSystem;
        inventorySystemReference = inventorySystem;
        _partyMemberSlots = GetComponentsInChildren<PartyMemberUI>();
        summarySystem.OnClosedSummary += SelectBox;
    }

    public void HandleUpdate()
    {
        //If B button is pressed go back a menu
        if (Input.GetButtonDown("Fire2"))
        {
            if (battleSelections.activeInHierarchy == true | overworldSelections.activeInHierarchy == true)
            {
                //onCloseParty();
            }
        }
    }

    public void OpenPartySystem(bool inBattle,bool wasShiftSwap)
    {
        _lastSelected = null;

        SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        SetMessageText("Choose a Pokemon");

        SetUpPartySystemCancelButton(inBattle,wasShiftSwap);
    }

    public void OpenPartySystemDueToInventoryItem(bool inBattle, Item item, bool usingItem)
    {
        _lastSelected = null;

        SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        if(usingItem == true)
        {
            SetMessageText($"Use {item.ItemBase.name} on which pokemon");
            SetUpPartySystemCancelButtonFromInventory(inBattle);
        }
        else
        {
            SetMessageText($"Give {item.ItemBase.name} to pokemon");

        }

    }

    void ClosePartySystem()
    {
        onCloseParty();
    }

    void SelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(_partyMemberSlots[0].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }

    void SetUpPartySystemCancelButton(bool inBattle,bool wasShiftSwap)
    {
        if(inBattle == true)
        {
            if(battleSystemReference.GetCurrentPokemonInBattle.currentHitPoints > 0 || battleSystemReference.GetCurrentPokemonInBattle == null)
            {
                cancelButton.onClick.AddListener(() => 
                {
                    ClosePartySystem();
                    if(wasShiftSwap == false)
                    {
                        battleSystemReference.ReturnFromPokemonPartySystem();
                    }
                    else
                    {
                        battleSystemReference.PlayerContinueAfterPartyShiftSelection();
                    }
                });
            }
            else
            {
                cancelButton.onClick.AddListener(() => SetMessageText($"{battleSystemReference.GetCurrentPokemonInBattle.currentName} can no longer battle"));
            }
        }
        else
        {
            cancelButton.onClick.AddListener(() => { ClosePartySystem(); });
        }
        cancelButton.GetComponent<PartyCancelUI>().OnHandleStart();
    }

    void SetUpPartySystemCancelButtonFromInventory(bool inBattle)
    {
        cancelButton.onClick.AddListener(() =>
        {
            ClosePartySystem();
            inventorySystemReference.OpenInventorySystem(inBattle);
        });
        cancelButton.GetComponent<PartyCancelUI>().OnHandleStart();
    }

    public void SetPartyData(List<Pokemon> currentParty,bool inBattle)
    {
        _currentParty = currentParty;

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if(i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i], i);

                int k = i;
                if (inBattle)
                {
                    _partyMemberSlots[k].GetComponent<Button>().onClick.RemoveAllListeners();
                    _partyMemberSlots[k].GetComponent<Button>().onClick.AddListener(() => OpenBattleSelections(_partyMemberSlots[k]));
                }
                else
                {
                    _partyMemberSlots[k].GetComponent<Button>().onClick.RemoveAllListeners();
                    _partyMemberSlots[k].GetComponent<Button>().onClick.AddListener(() => OpenOverworldSelections(_partyMemberSlots[k]));
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

        battleSelectionSummaryButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionSummaryButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SummaryButton(System.Array.IndexOf(_partyMemberSlots, currentPartyMember));
            _lastSelected = battleSelectionSummaryButton;
            EventSystem.current.SetSelectedGameObject(null);
        });

        battleSelectionCancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionCancelButton.GetComponent<Button>().onClick.AddListener(() => CancelSubMenuButton(currentPartyMember));
    }

    void OpenOverworldSelections(PartyMemberUI currentPartyMember)
    {
        overworldSelections.SetActive(true);
        EventSystem.current.SetSelectedGameObject(overworldSelectionsSummaryButton);
        AdjustMessageBoxWidthSize(MESSAGEBOX_SELECTED_SIZE);

        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SummaryButton(System.Array.IndexOf(_partyMemberSlots, currentPartyMember));
            _lastSelected = overworldSelectionsSummaryButton;
            EventSystem.current.SetSelectedGameObject(null);
        });

        overworldSelectionsCancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsCancelButton.GetComponent<Button>().onClick.AddListener(() => CancelSubMenuButton(currentPartyMember));
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

    void CancelSubMenuButton(PartyMemberUI previousSelection)
    {
        battleSelections.SetActive(false);
        overworldSelections.SetActive(false);
        EventSystem.current.SetSelectedGameObject(previousSelection.gameObject);
    }

    void SummaryButton(int pokemonIndex)
    {
        summarySystem.OnSummaryMenuOpened(_currentParty, pokemonIndex);
    }
}
