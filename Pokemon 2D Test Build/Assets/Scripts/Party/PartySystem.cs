using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySystem : MonoBehaviour
{
    [SerializeField] PartyDialogBox dialogBox;
    [SerializeField] SummarySystem summarySystem;

    PartyMemberUI[] _partyMemberSlots;
    [SerializeField] Button cancelButton;

    BattleSystem battleSystemReference;
    InventorySystem inventorySystemReference;
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
        gameObject.SetActive(false);
        battleSystemReference = GameManager.instance.GetBattleSystem;
        inventorySystemReference = GameManager.instance.GetInventorySystem;
        _partyMemberSlots = GetComponentsInChildren<PartyMemberUI>();
        summarySystem.Initialization();
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
        dialogBox.SetDialogText("Choose a Pokemon");

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
            dialogBox.SetDialogText($"Use {item.ItemBase.name} on which pokemon");
        }
        else
        {
            dialogBox.SetDialogText($"Give {item.ItemBase.name} to pokemon");
        }
        SetUpPartySystemCancelButtonFromInventory();
    }

    void ClosePartySystem()
    {
        if (BattleSystem.inBattle == true)
        {
            GameManager.SetGameState(GameState.Battle);
        }
        else
        {
            GameManager.SetGameState(GameState.Overworld);
        }
        gameObject.SetActive(false);
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
                cancelButton.onClick.AddListener(() => dialogBox.SetDialogText($"{battleSystemReference.GetCurrentPokemonInBattle.currentName} can no longer battle"));
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
                        int hpDif = currentPokemon.currentHitPoints;
                        if (item.ItemBase.UseItem(currentPokemon) == true)
                        {
                            dialogBox.SetDialogText($"{item.ItemBase.ItemName} was used on {currentPokemon.currentName}");
                            hpDif = (hpDif != currentPokemon.currentHitPoints) ? currentPokemon.currentHitPoints - hpDif : 0;
                            inventorySystemReference.RemoveItem(item);
                            StartCoroutine(WaitForInputAfterItemUsage(true, _partyMemberSlots[k], hpDif));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                        }
                        else
                        {
                            dialogBox.SetDialogText($"{item.ItemBase.ItemName} had no effect");
                            StartCoroutine(WaitForInputAfterItemUsage(false, _partyMemberSlots[k]));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                        }
                    });
                }
                else //Give
                {
                    _partyMemberSlots[k].GetButton.onClick.AddListener(() =>
                    {
                        Pokemon currentPokemon = _partyMemberSlots[k].CurrentPokemon();
                        if (currentPokemon.GetCurrentItem != null)
                        {
                            ItemBase oldItem = currentPokemon.GetCurrentItem;
                            StartCoroutine(GivePokemonItemWhileHoldingItem(oldItem,item,currentPokemon));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                        }
                        else
                        {
                            StartCoroutine(WaitForInputAfterItemGiven(item,currentPokemon));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
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

    public void AdjustMessageBoxWidthSize(int size)
    {
        //get component in parent returns the same object since they both share rect transform
        RectTransform rt = dialogBox.transform.parent.GetComponent<RectTransform>();
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
            dialogBox.SetDialogText($"{currentPartyMember.CurrentPokemon().currentName} has no energy left to battle!");
            return;
        }

        if(currentPartyMember.CurrentPokemon() == battleSystemReference.GetCurrentPokemonInBattle)
        {
            dialogBox.SetDialogText($"{currentPartyMember.CurrentPokemon().currentName} is already in battle!");
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

    IEnumerator WaitForInputAfterItemUsage(bool itemWasUsed, PartyMemberUI partyMemberUI,int hpRecovered = 0)
    {
        yield return partyMemberUI.UpdateAfterItemUse(hpRecovered);
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
        
        inventorySystemReference.ReturnFromPartySystemAfterItemUsage(true);
    }

    public void ReturnFromSummarySystem()
    {
        SelectBox();
    }

    IEnumerator GivePokemonItemWhileHoldingItem(ItemBase oldItem, Item newItem,Pokemon currentPokemon)
    {
        yield return dialogBox.TypeDialog($"{currentPokemon.currentName} is already holding {oldItem.ItemName}", true);

        if(oldItem == newItem.ItemBase)
        {
            ClosePartySystem();
            inventorySystemReference.ReturnFromPartySystemAfterItemUsage(false);
        }

        yield return dialogBox.TypeDialog($"Would you like to switch the two items");

        bool itemWasSwitched = false;
        yield return dialogBox.SetChoiceBox(() =>
        {
            itemWasSwitched = true;
            inventorySystemReference.AddItem(oldItem);
            inventorySystemReference.RemoveItem(newItem);
        });

        if(itemWasSwitched == true)
        {
            yield return dialogBox.TypeDialog($"{oldItem.ItemName} was taken and replaced with {newItem.ItemBase.ItemName}.");
        }

        ClosePartySystem();
        inventorySystemReference.ReturnFromPartySystemAfterItemUsage(false);
    }

    IEnumerator WaitForInputAfterItemGiven(Item item,Pokemon pokemon)
    {
        pokemon.GivePokemonItemToHold(item.ItemBase);
        yield return dialogBox.TypeDialog($"{pokemon.currentName} was given {item.ItemBase.ItemName} to hold");

        bool waitingForInput = false;
        yield return new WaitForSeconds(1f);
        while (waitingForInput == false)
        {
            if (Input.anyKeyDown)
            {
                waitingForInput = true;
            }
            yield return null;
        }

        inventorySystemReference.RemoveItem(item);
        ClosePartySystem();
        inventorySystemReference.ReturnFromPartySystemAfterItemUsage(false);
    }
}
