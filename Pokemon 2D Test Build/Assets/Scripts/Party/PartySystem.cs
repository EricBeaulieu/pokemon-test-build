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

    public void Initialization()
    {
        battleSystemReference = GameManager.instance.GetBattleSystem;
        inventorySystemReference = GameManager.instance.GetInventorySystem;
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

    public void OpenPartySystem(bool wasShiftSwap)
    {
        GameManager.SetGameState(GameState.Party);
        gameObject.SetActive(true);
        SetPartyData();
        _lastSelected = null;

        SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        SetMessageText("Choose a Pokemon");

        SetUpPartySystemCancelButton(wasShiftSwap);
    }

    public void OpenPartySystemDueToInventoryItem(Item item, bool usingItem)
    {
        GameManager.SetGameState(GameState.Party);
        gameObject.SetActive(true);
        SetPartyItem(item, usingItem);
        _lastSelected = null;

        SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        if(usingItem == true)
        {
            SetMessageText($"Use {item.ItemBase.name} on which pokemon");
            SetUpPartySystemCancelButtonFromInventory();
        }
        else
        {
            SetMessageText($"Give {item.ItemBase.name} to pokemon");
            SetUpPartySystemCancelButtonFromInventory();
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

    void SetUpPartySystemCancelButton(bool wasShiftSwap)
    {
        cancelButton.onClick.RemoveAllListeners();
        if (BattleSystem.inBattle == true)
        {
            if(battleSystemReference.GetCurrentPokemonInBattle.currentHitPoints > 0 || battleSystemReference.GetCurrentPokemonInBattle == null)
            {
                cancelButton.onClick.AddListener(() => 
                {
                    ClosePartySystem();
                    if(wasShiftSwap == false)
                    {
                        battleSystemReference.ReturnFromPokemonAlternateSystem();
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

    void SetUpPartySystemCancelButtonFromInventory()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            ClosePartySystem();
            inventorySystemReference.OpenInventorySystem();
        });
        cancelButton.GetComponent<PartyCancelUI>().OnHandleStart();
    }

    void SetPartyData()
    {
        List<Pokemon> currentParty = GameManager.instance.GetPlayerController.pokemonParty.CurrentPokemonList();
        _currentParty = currentParty;

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if(i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i], i);

                int k = i;
                _partyMemberSlots[k].GetComponent<Button>().onClick.RemoveAllListeners();
                if (BattleSystem.inBattle == true)
                {
                    _partyMemberSlots[k].GetComponent<Button>().onClick.AddListener(() => OpenBattleSelections(_partyMemberSlots[k]));
                }
                else
                {
                    _partyMemberSlots[k].GetComponent<Button>().onClick.AddListener(() => OpenOverworldSelections(_partyMemberSlots[k]));
                }

                if(i+1 == currentParty.Count)
                {
                    //have to pull the variable out, then change it and set it back in
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = cancelButton;
                    _partyMemberSlots[i].GetComponent<Button>().navigation = navigation;

                    //CancelButton Up
                    navigation = cancelButton.navigation;
                    navigation.selectOnUp = _partyMemberSlots[i].GetButton;
                    cancelButton.navigation = navigation;
                }
                else
                {
                    var navigation = _partyMemberSlots[i].GetComponent<Button>().navigation;
                    navigation.selectOnDown = _partyMemberSlots[i+1].GetComponent<Button>();
                    _partyMemberSlots[i].GetButton.navigation = navigation;
                }
            }
            else
            {
                _partyMemberSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetPartyItem(Item item,bool useItem)//False == give
    {
        List<Pokemon> currentParty = GameManager.instance.GetPlayerController.pokemonParty.CurrentPokemonList();
        _currentParty = currentParty;

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if (i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i], i);

                int k = i;
                _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                if (useItem == true)
                {
                    _partyMemberSlots[k].GetButton.onClick.AddListener(() => 
                    {
                        Pokemon currentPokemon = _partyMemberSlots[k].CurrentPokemon();
                        if (item.ItemBase.UseItem(currentPokemon) == true)
                        {
                            SetMessageText($"{item.ItemBase.ItemName} was used on {currentPokemon.currentName}");
                            StartCoroutine(WaitForInputAfterItemUsage(true));
                        }
                        else
                        {
                            SetMessageText($"{item.ItemBase.ItemName} had no effect");
                            StartCoroutine(WaitForInputAfterItemUsage(false));
                        }
                    });
                }
                else //Give
                {
                    _partyMemberSlots[k].GetButton.onClick.AddListener(() => 
                    {
                        Pokemon currentPokemon = _partyMemberSlots[k].CurrentPokemon();
                        if (currentPokemon.GetCurrentItem == null)
                        {
                            SetMessageText($"{item.ItemBase.ItemName} was given to {currentPokemon.currentName}");
                            currentPokemon.GivePokemonItemToHold(item.ItemBase);
                            item.Count--;
                            StartCoroutine(WaitForInputAfterItemUsage(false));
                        }
                        else
                        {
                            SetMessageText($"{currentPokemon.currentName} is already holding onto {currentPokemon.GetCurrentItem.ItemName}");
                            StartCoroutine(WaitForInputAfterItemUsage(false));
                        }
                    });
                }

                if (i + 1 == currentParty.Count)
                {
                    //have to pull the variable out, then change it and set it back in
                    var navigation = _partyMemberSlots[i].GetButton.navigation;
                    navigation.selectOnDown = cancelButton;
                    _partyMemberSlots[i].GetButton.navigation = navigation;

                    //CancelButton Up
                    navigation = cancelButton.navigation;
                    navigation.selectOnUp = _partyMemberSlots[i].GetButton;
                    cancelButton.navigation = navigation;
                }
                else
                {
                    var navigation = _partyMemberSlots[i].GetButton.navigation;
                    navigation.selectOnDown = _partyMemberSlots[i + 1].GetButton;
                    _partyMemberSlots[i].GetButton.navigation = navigation;
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

    IEnumerator WaitForInputAfterItemUsage(bool itemWasUsed)
    {
        //If the pokemons hp went up or was recovered then show an animation of the pokemon either going from faint to alive 
        //and show the bar animate. Also update the battle bar 

        bool waitingForInput = false;
        yield return new WaitForSeconds(1f);
        while(waitingForInput == false)
        {
            if (Input.anyKeyDown)
            {
                waitingForInput = true;
            }
            yield return null;
        }

        ClosePartySystem();
        if (BattleSystem.inBattle == true && itemWasUsed == true)
        {
            battleSystemReference.PlayerUsedItemWhileInBattle();
            yield break;
        }
        
        inventorySystemReference.ReturnFromPartySystemAfterItemUsage();
    }
}
