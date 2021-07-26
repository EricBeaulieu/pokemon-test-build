using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySystem : MonoBehaviour
{
    [SerializeField] DialogBox dialogBox;
    [SerializeField] SummarySystem summarySystem;

    PartyMemberUI[] _partyMemberSlots;
    [SerializeField] Button cancelButton;

    BattleSystem battleSystem;
    InventorySystem inventorySystem;
    DialogManager dialogSystem;
    PokemonParty playersParty;
    List<Pokemon> currentParty;
    static bool currentlySwitchingPokemon = false;
    PartyMemberUI specificPartyMemberUI = null;

    [SerializeField] GameObject overworldSelections;
    [SerializeField] GameObject overworldSelectionsSummaryButton;
    [SerializeField] GameObject overworldSelectionsSwitchButton;
    [SerializeField] GameObject overworldSelectionsItemButton;
    [SerializeField] GameObject overworldSelectionsCancelButton;

    [SerializeField] GameObject itemSelections;
    [SerializeField] GameObject itemSelectionsGiveButton;
    [SerializeField] GameObject itemSelectionsTakeButton;
    [SerializeField] GameObject itemSelectionsCancelButton;

    [SerializeField] GameObject battleSelections;
    [SerializeField] GameObject battleSelectionShiftButton;
    [SerializeField] GameObject battleSelectionSummaryButton;
    [SerializeField] GameObject battleSelectionCancelButton;

    GameObject _lastSelected;

    const int MESSAGEBOX_STANDARD_SIZE = 650;
    const int MESSAGEBOX_SELECTED_SIZE = 570;
    const string STANDARD_MESSAGE = "Choose A Pokemon";

    public void Initialization()
    {
        gameObject.SetActive(false);
        battleSystem = GameManager.instance.GetBattleSystem;
        inventorySystem = GameManager.instance.GetInventorySystem;
        dialogSystem = GameManager.instance.GetDialogSystem;
        SetupPartyMemberFunctionality();
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

    public void OpenPartySystem(bool wasShiftSwap = false)
    {
        GameManager.SetGameState(GameState.Party);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        gameObject.SetActive(true);
        SetPartyData();
        _lastSelected = null;

        SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        dialogSystem.SetDialogText(STANDARD_MESSAGE);

        SetUpPartySystemCancelButton(wasShiftSwap);
    }

    public void OpenPartySystemDueToInventoryItem(Item item, bool usingItem)
    {
        GameManager.SetGameState(GameState.Party);
        dialogSystem.SetCurrentDialogBox(dialogBox);
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
            dialogSystem.SetDialogText($"Use {item.ItemBase.name} on which pokemon");
        }
        else
        {
            dialogSystem.SetDialogText($"Give {item.ItemBase.name} to pokemon");
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
            dialogSystem.SetCurrentDialogBox();
        }
        gameObject.SetActive(false);
    }

    void SelectBox(GameObject gameObject = null)
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

        if(gameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    void SetUpPartySystemCancelButton(bool wasShiftSwap)
    {
        cancelButton.onClick.RemoveAllListeners();
        if (BattleSystem.inBattle == true)
        {
            if(battleSystem.GetCurrentPokemonInBattle.currentHitPoints > 0 || battleSystem.GetCurrentPokemonInBattle == null)
            {
                cancelButton.onClick.AddListener(() => 
                {
                    ClosePartySystem();
                    if(wasShiftSwap == false)
                    {
                        battleSystem.ReturnFromPokemonAlternateSystem();
                    }
                    else
                    {
                        battleSystem.PlayerContinueAfterPartyShiftSelection();
                    }
                });
            }
            else
            {
                cancelButton.onClick.AddListener(() => dialogSystem.SetDialogText($"{battleSystem.GetCurrentPokemonInBattle.currentName} can no longer battle"));
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
            inventorySystem.OpenInventorySystem();
        });
        cancelButton.GetComponent<PartyCancelUI>().OnHandleStart();
    }

    void SetPartyData()
    {
        List<Pokemon> currentParty = GameManager.instance.GetPlayerController.pokemonParty.CurrentPokemonList();
        this.currentParty = currentParty;

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if(i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i]);

                int k = i;
                _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                if (BattleSystem.inBattle == true)
                {
                    _partyMemberSlots[k].GetButton.onClick.AddListener(() => OpenBattleSelections(_partyMemberSlots[k]));
                }
                else
                {
                    _partyMemberSlots[k].GetButton.onClick.AddListener(() => OpenOverworldSelections(_partyMemberSlots[k]));
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
        currentParty = playersParty.CurrentPokemonList();

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if (i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                _partyMemberSlots[i].SetData(currentParty[i]);

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
                            dialogSystem.SetDialogText($"{item.ItemBase.ItemName} was used on {currentPokemon.currentName}");
                            hpDif = (hpDif != currentPokemon.currentHitPoints) ? currentPokemon.currentHitPoints - hpDif : 0;
                            inventorySystem.RemoveItem(item);
                            StartCoroutine(WaitForInputAfterItemUsage(true, _partyMemberSlots[k], hpDif));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                        }
                        else
                        {
                            dialogSystem.SetDialogText($"{item.ItemBase.ItemName} had no effect");
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
                            StartCoroutine(GivePokemonItemWhileHoldingItem(oldItem,item,currentPokemon,true));
                            _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                        }
                        else
                        {
                            StartCoroutine(WaitForInputAfterItemGiven(item, _partyMemberSlots[k],true));
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
        RectTransform rt = dialogBox.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(size, rt.sizeDelta.y);
    }

    void OpenBattleSelections(PartyMemberUI currentPartyMember)
    {
        dialogSystem.SetDialogText($"Do what with {currentPartyMember.CurrentPokemon().currentName}");
        currentPartyMember.isCurrentlySelected = true;
        battleSelections.SetActive(true);
        SelectBox(battleSelectionShiftButton);
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
        dialogSystem.SetDialogText($"Do What with {currentPartyMember.CurrentPokemon().currentName}");
        currentPartyMember.isCurrentlySelected = true;
        overworldSelections.SetActive(true);
        itemSelections.SetActive(false);
        EventSystem.current.SetSelectedGameObject(overworldSelectionsSummaryButton);
        AdjustMessageBoxWidthSize(MESSAGEBOX_SELECTED_SIZE);

        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SummaryButton(System.Array.IndexOf(_partyMemberSlots, currentPartyMember));
            _lastSelected = overworldSelectionsSummaryButton;
            EventSystem.current.SetSelectedGameObject(null);
        });

        overworldSelectionsSwitchButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsSwitchButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPartyMember.SwitchingPokemon = true;
            specificPartyMemberUI = currentPartyMember;
            CancelSubMenuButton(currentPartyMember);
            SetNextPokemonSelectedToSwitchPositions();
        });

        overworldSelectionsItemButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsItemButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OpenItemSelections(currentPartyMember);
        });

        overworldSelectionsCancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsCancelButton.GetComponent<Button>().onClick.AddListener(() => CancelSubMenuButton(currentPartyMember));
    }

    void ShiftBattleButton(PartyMemberUI currentPartyMember)
    {
        if (currentPartyMember.CurrentPokemon().currentHitPoints <= 0)
        {
            dialogSystem.SetDialogText($"{currentPartyMember.CurrentPokemon().currentName} has no energy left to battle!");
            return;
        }

        if(currentPartyMember.CurrentPokemon() == battleSystem.GetCurrentPokemonInBattle)
        {
            dialogSystem.SetDialogText($"{currentPartyMember.CurrentPokemon().currentName} is already in battle!");
            return;
        }

        battleSystem.PlayerSwitchPokemon(currentPartyMember.CurrentPokemon());
        currentPartyMember.isCurrentlySelected = false;
        ClosePartySystem();
    }

    void CancelSubMenuButton(PartyMemberUI previousSelection)
    {
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        previousSelection.isCurrentlySelected = false;
        battleSelections.SetActive(false);
        overworldSelections.SetActive(false);
        EventSystem.current.SetSelectedGameObject(previousSelection.gameObject);
        dialogSystem.SetDialogText(STANDARD_MESSAGE);
    }

    void SummaryButton(int pokemonIndex)
    {
        summarySystem.OnSummaryMenuOpened(currentParty, pokemonIndex);
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
            battleSystem.PlayerUsedItemWhileInBattle();
            yield break;
        }
        
        inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
    }

    public void ReturnFromSummarySystem()
    {
        SelectBox();
    }

    IEnumerator GivePokemonItemWhileHoldingItem(ItemBase oldItem, Item newItem,Pokemon currentPokemon,bool returnToInventorySystem)
    {
        yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} is already holding {oldItem.ItemName}", true);

        if(oldItem == newItem.ItemBase)
        {
            if(returnToInventorySystem == true)
            {
                ClosePartySystem();
                inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
                yield break;
            }
            SelectBox(specificPartyMemberUI.gameObject);
            specificPartyMemberUI = null;
            yield break;
        }

        yield return dialogSystem.TypeDialog($"Would you like to switch the two items",true);

        bool itemWasSwitched = false;
        yield return dialogSystem.SetChoiceBox(() =>
        {
            itemWasSwitched = true;
            inventorySystem.AddItem(oldItem);
            inventorySystem.RemoveItem(newItem);
            currentPokemon.GivePokemonItemToHold(newItem.ItemBase);
        });

        if(itemWasSwitched == true)
        {
            yield return dialogSystem.TypeDialog($"{oldItem.ItemName} was taken and replaced with {newItem.ItemBase.ItemName}.",true);
        }

        if (returnToInventorySystem == true)
        {
            ClosePartySystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
        }
        else
        {
            dialogSystem.SetDialogText(STANDARD_MESSAGE);
            specificPartyMemberUI.isCurrentlySelected = false;
            SelectBox(specificPartyMemberUI.gameObject);
            specificPartyMemberUI = null;
        }
    }

    IEnumerator WaitForInputAfterItemGiven(Item item,PartyMemberUI pokemonPos,bool returnToInventorySystem)
    {
        pokemonPos.CurrentPokemon().GivePokemonItemToHold(item.ItemBase);
        pokemonPos.UpdateHoldItem();
        yield return dialogSystem.TypeDialog($"{pokemonPos.CurrentPokemon().currentName} was given {item.ItemBase.ItemName} to hold",true);

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

        if(returnToInventorySystem == true)
        {
            inventorySystem.RemoveItem(item);
            ClosePartySystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
        }
        else
        {
            dialogSystem.SetDialogText(STANDARD_MESSAGE);
            specificPartyMemberUI.isCurrentlySelected = false;
            SelectBox(specificPartyMemberUI.gameObject);
            specificPartyMemberUI = null;
        }
    }

    void SetNextPokemonSelectedToSwitchPositions()
    {
        currentlySwitchingPokemon = true;
        // if switching and select the same pokemon it cancels out of switching, as well as the cancel button

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (i < currentParty.Count)
            {
                int k = i;
                _partyMemberSlots[k].GetButton.onClick.RemoveAllListeners();
                _partyMemberSlots[k].GetButton.onClick.AddListener(() => 
                {
                    _lastSelected = _partyMemberSlots[k].gameObject;
                    if (_partyMemberSlots[k] == specificPartyMemberUI)
                    {
                        SwitchingPokemonEnded();
                    }
                    else
                    {
                        StartCoroutine(SwitchPokemonPositions(specificPartyMemberUI, _partyMemberSlots[k]));
                    }
                    
                });
            }
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() =>
            {
                _lastSelected = cancelButton.gameObject;
                SwitchingPokemonEnded();
            });
        }
    }

    void SwitchingPokemonEnded()
    {
        for (int i = 0; i < currentParty.Count; i++)
        {
            _partyMemberSlots[i].SwitchingPokemon = false;
        }
        SetPartyData();
        SetUpPartySystemCancelButton(false);
        currentlySwitchingPokemon = false;
        specificPartyMemberUI = null;
        SelectBox();
    }

    IEnumerator SwitchPokemonPositions(PartyMemberUI firstPokemon, PartyMemberUI secondPokemon)
    {
        secondPokemon.SwitchingPokemon = true;

        StartCoroutine(firstPokemon.AnimateSwitchToStandardPosition(false));
        yield return secondPokemon.AnimateSwitchToStandardPosition(false);

        playersParty.SwitchPokemonPositions(firstPokemon.CurrentPokemon(), secondPokemon.CurrentPokemon());
        Pokemon firstPositionPokemon = firstPokemon.CurrentPokemon();
        Pokemon secondPositionPokemon = secondPokemon.CurrentPokemon();
        firstPokemon.SetData(secondPositionPokemon);
        secondPokemon.SetData(firstPositionPokemon);

        StartCoroutine(firstPokemon.AnimateSwitchToStandardPosition(true));
        yield return secondPokemon.AnimateSwitchToStandardPosition(true);

        SwitchingPokemonEnded();
    }

    public static bool GetCurrentlySwitchingPokemon
    {
        get { return currentlySwitchingPokemon; }
    }

    void SetupPartyMemberFunctionality()
    {
        _partyMemberSlots = GetComponentsInChildren<PartyMemberUI>();
        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            _partyMemberSlots[i].Initialization(i);
        }
    }

    public void SetPlayersParty(PokemonParty party)
    {
        playersParty = party;
    }

    void OpenItemSelections(PartyMemberUI currentPartyMember)
    {
        overworldSelections.SetActive(false);
        itemSelections.SetActive(true);
        SelectBox(itemSelectionsGiveButton);

        itemSelectionsGiveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        itemSelectionsGiveButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            specificPartyMemberUI = currentPartyMember;
            EventSystem.current.SetSelectedGameObject(null);
            ClosePartySystem();
            inventorySystem.OpenUpInventorySystemDueToGivingItemFromParty(currentPartyMember.CurrentPokemon());
        });

        itemSelectionsTakeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        itemSelectionsTakeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            ItemBase item = currentPartyMember.CurrentPokemon().GetCurrentItem;
            StartCoroutine(WaitForInputAfterItemTaken(item, currentPartyMember));
        });

        itemSelectionsCancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        itemSelectionsCancelButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OpenOverworldSelections(currentPartyMember);
        });
    }

    IEnumerator WaitForInputAfterItemTaken(ItemBase item, PartyMemberUI pokemonPos)
    {
        itemSelections.SetActive(false);
        if (item == null)
        {
            yield return dialogSystem.TypeDialog($"{pokemonPos.CurrentPokemon().currentName} isn't holding anything.");
        }
        else
        {
            inventorySystem.AddItem(item);
            pokemonPos.CurrentPokemon().GivePokemonItemToHold(null);
            pokemonPos.UpdateHoldItem();
            yield return dialogSystem.TypeDialog($"Received the {item.ItemName} from {pokemonPos.CurrentPokemon().currentName}",true);
        }

        pokemonPos.isCurrentlySelected = false;
        SelectBox(pokemonPos.gameObject);
        dialogSystem.SetDialogText(STANDARD_MESSAGE);
    }

    public void ReturnToPartySystemAfterGivingItemToHoldFromInventory(Item item = null)
    {
        GameManager.SetGameState(GameState.Party);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        gameObject.SetActive(true);
        if(item == null)
        {
            OpenItemSelections(specificPartyMemberUI);
        }
        else
        {
            itemSelections.SetActive(false);

            ItemBase oldItem = specificPartyMemberUI.CurrentPokemon().GetCurrentItem;
            if(oldItem != null)
            {
                StartCoroutine(GivePokemonItemWhileHoldingItem(oldItem, item, specificPartyMemberUI.CurrentPokemon(),false));
            }
            else
            {
                StartCoroutine(WaitForInputAfterItemGiven(item, specificPartyMemberUI, false));
            }
        }
    }
}
