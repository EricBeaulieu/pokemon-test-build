using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySystem : CoreSystem
{
    [SerializeField] SummarySystem summarySystem;
    LearnNewMoveUI learnNewMoveUI;

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

    SelectableBoxUI selectableBox;

    const int MESSAGEBOX_STANDARD_SIZE = 650;
    const int MESSAGEBOX_SELECTED_SIZE = 570;
    const string STANDARD_MESSAGE = "Choose A Pokemon";

    public override void Initialization()
    {
        gameObject.SetActive(false);
        battleSystem = GameManager.instance.GetBattleSystem;
        inventorySystem = GameManager.instance.GetInventorySystem;
        dialogSystem = GameManager.instance.GetDialogSystem;
        learnNewMoveUI = GameManager.instance.GetLearnNewMoveSystem;
        SetupPartyMemberFunctionality();
        summarySystem.Initialization();
        selectableBox = new SelectableBoxUI(_partyMemberSlots[0].gameObject);
        SetUpStaticArt();
    }

    public override void HandleUpdate()
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

    public override void OpenSystem(bool wasShiftSwap = false)
    {
        StandardPartySystemOpenPre();
        SetPartyData();
        StandardPartySystemOpenMid();
        dialogSystem.SetDialogText(STANDARD_MESSAGE);
        SetUpPartySystemCancelButton(wasShiftSwap);
    }

    public void OpenPartySystemDueToInventoryItem(Item item, bool usingItem)
    {
        StandardPartySystemOpenPre();
        SetPartyItem(item, usingItem);
        StandardPartySystemOpenMid();
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

    void StandardPartySystemOpenPre()
    {
        GameManager.SetGameState(GameState.Party);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        gameObject.SetActive(true);
    }

    void StandardPartySystemOpenMid()
    {
        selectableBox.SetLastSelected(null);
        selectableBox.SelectBox();
        summarySystem.CloseSummarySystem();
        overworldSelections.SetActive(false);
        battleSelections.SetActive(false);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
    }

    protected override void CloseSystem()
    {
        if (BattleSystem.InBattle == true)
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

    void SetUpPartySystemCancelButton(bool wasShiftSwap)
    {
        cancelButton.onClick.RemoveAllListeners();
        if (BattleSystem.InBattle == true)
        {
            if(battleSystem.GetCurrentPokemonInBattle.currentHitPoints > 0 || battleSystem.GetCurrentPokemonInBattle == null)
            {
                cancelButton.onClick.AddListener(() => 
                {
                    CloseSystem();
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
            cancelButton.onClick.AddListener(() => { CloseSystem(); });
        }
        cancelButton.GetComponent<PartyCancelUI>().OnHandleStart();
    }

    void SetUpPartySystemCancelButtonFromInventory()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            CloseSystem();
            inventorySystem.OpenSystem();
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
                if (BattleSystem.InBattle == true)
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
                    navigation.selectOnDown = _partyMemberSlots[i+1].GetButton;
                    _partyMemberSlots[i].GetButton.navigation = navigation;
                }
            }
            else
            {
                _partyMemberSlots[i].gameObject.SetActive(false);
            }
        }
    }

    void SetPartyItem(Item item,bool useItem)//False == give
    {
        currentParty = playersParty.CurrentPokemonList();
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);

        for (int i = 0; i < _partyMemberSlots.Length; i++)
        {
            if (i < currentParty.Count)
            {
                _partyMemberSlots[i].gameObject.SetActive(true);
                if(useItem == true)
                {
                    _partyMemberSlots[i].SetData(currentParty[i],item.ItemBase);
                }
                else
                {
                    _partyMemberSlots[i].SetData(currentParty[i]);
                }

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
                            if (item.ItemBase is TMHMItem)
                            {
                                StartCoroutine(LearnNewMove(currentPokemon, (TMHMItem)item.ItemBase));
                            }
                            else if (item.ItemBase is EvolutionStoneBase)
                            {
                                StartCoroutine(PlayerEvolvedFromPartyScene(currentPokemon, (EvolutionStoneBase)item.ItemBase));
                                inventorySystem.RemoveItem(item);
                            }
                            else
                            {
                                dialogSystem.SetDialogText($"{item.ItemBase.ItemName} was used on {currentPokemon.currentName}");
                                hpDif = (hpDif != currentPokemon.currentHitPoints) ? currentPokemon.currentHitPoints - hpDif : 0;
                                inventorySystem.RemoveItem(item);
                                StartCoroutine(WaitForInputAfterItemUsage(true, _partyMemberSlots[k], hpDif));
                            }
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

    void AdjustMessageBoxWidthSize(int size)
    {
        RectTransform rt = dialogBox.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(size, rt.sizeDelta.y);
    }

    void OpenBattleSelections(PartyMemberUI currentPartyMember)
    {
        dialogSystem.SetDialogText($"Do what with {currentPartyMember.CurrentPokemon().currentName}");
        currentPartyMember.isCurrentlySelected = true;
        battleSelections.SetActive(true);
        selectableBox.SelectBox(battleSelectionShiftButton);
        AdjustMessageBoxWidthSize(MESSAGEBOX_SELECTED_SIZE);

        battleSelectionShiftButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionShiftButton.GetComponent<Button>().onClick.AddListener(() => ShiftBattleButton(currentPartyMember));

        battleSelectionSummaryButton.GetComponent<Button>().onClick.RemoveAllListeners();
        battleSelectionSummaryButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SummaryButton(System.Array.IndexOf(_partyMemberSlots, currentPartyMember));
            selectableBox.SetLastSelected(battleSelectionSummaryButton);
            selectableBox.Deselect();
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
        selectableBox.SelectBox(overworldSelectionsSummaryButton);
        AdjustMessageBoxWidthSize(MESSAGEBOX_SELECTED_SIZE);

        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.RemoveAllListeners();
        overworldSelectionsSummaryButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            SummaryButton(System.Array.IndexOf(_partyMemberSlots, currentPartyMember));
            selectableBox.SetLastSelected(overworldSelectionsSummaryButton);
            selectableBox.Deselect();
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
        CloseSystem();
    }

    void CancelSubMenuButton(PartyMemberUI previousSelection)
    {
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
        previousSelection.isCurrentlySelected = false;
        battleSelections.SetActive(false);
        overworldSelections.SetActive(false);
        selectableBox.SelectBox(previousSelection.gameObject);
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

        CloseSystem();
        if (BattleSystem.InBattle == true && itemWasUsed == true)
        {
            battleSystem.PlayerUsedItemWhileInBattle();
            yield break;
        }
        
        inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
    }

    public void ReturnFromSummarySystem()
    {
        selectableBox.SelectBox();
    }

    IEnumerator GivePokemonItemWhileHoldingItem(ItemBase oldItem, Item newItem,Pokemon currentPokemon,bool returnToInventorySystem)
    {
        yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} is already holding {oldItem.ItemName}", true);

        if(oldItem == newItem.ItemBase)
        {
            if(returnToInventorySystem == true)
            {
                CloseSystem();
                inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
                yield break;
            }
            selectableBox.SelectBox(specificPartyMemberUI.gameObject);
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
            CloseSystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
        }
        else
        {
            dialogSystem.SetDialogText(STANDARD_MESSAGE);
            specificPartyMemberUI.isCurrentlySelected = false;
            selectableBox.SelectBox(specificPartyMemberUI.gameObject);
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
            CloseSystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(false);
        }
        else
        {
            dialogSystem.SetDialogText(STANDARD_MESSAGE);
            specificPartyMemberUI.isCurrentlySelected = false;
            selectableBox.SelectBox(specificPartyMemberUI.gameObject);
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
                    selectableBox.SetLastSelected(_partyMemberSlots[k].gameObject);
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
                selectableBox.SetLastSelected(cancelButton.gameObject);
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
        selectableBox.SelectBox();
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
        selectableBox.SelectBox(itemSelectionsGiveButton);

        itemSelectionsGiveButton.GetComponent<Button>().onClick.RemoveAllListeners();
        itemSelectionsGiveButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            specificPartyMemberUI = currentPartyMember;
            selectableBox.Deselect();
            CloseSystem();
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
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
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
        selectableBox.SelectBox(pokemonPos.gameObject);
        dialogSystem.SetDialogText(STANDARD_MESSAGE);
    }

    public void ReturnToPartySystemAfterGivingItemToHoldFromInventory(Item item = null)
    {
        GameManager.SetGameState(GameState.Party);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        AdjustMessageBoxWidthSize(MESSAGEBOX_STANDARD_SIZE);
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

    IEnumerator LearnNewMove(Pokemon currentPokemon, TMHMItem newMove)
    {
        if(currentPokemon.moves.Exists(x => x.moveBase == newMove.GetMove) == true)
        {
            yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} already knows {newMove.GetMove.MoveName}!", true);
            CloseSystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
            yield break;
        }

        if (currentPokemon.moves.Count < PokemonBase.MAX_NUMBER_OF_MOVES)
        {
            currentPokemon.LearnMove(newMove.GetMove);
            yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} learned {newMove.GetMove.MoveName}!", true);
            inventorySystem.RemoveItem(newMove);
            CloseSystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
            yield break;
        }
        else
        {
            yield return learnNewMoveUI.PokemonWantsToLearnNewMoves(currentPokemon, newMove);

            CloseSystem();
            inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
        }
    }

    IEnumerator PlayerEvolvedFromPartyScene(Pokemon pokemon,EvolutionStoneBase evolutionStone)
    {
        PokemonBase newEvolution = pokemon.pokemonBase.Evolutions.Find(x => x.RequiredStone == evolutionStone).NewPokemonEvolution();
        yield return GameManager.instance.GetEvolutionSystem.PokemonEvolving(pokemon, newEvolution);

        CloseSystem();
        inventorySystem.ReturnFromPartySystemAfterItemUsage(true);
    }

    //Main
    public static Sprite mainPartyMemberHealthySelectedBackground { get; private set; }
    public static Sprite mainPartyMemberHealthyNonSelectedBackground { get; private set; }
    public static Sprite mainPartyMemberFaintedSelectedBackground { get; private set; }
    public static Sprite mainPartyMemberFaintedNonSelectedBackground { get; private set; }
    public static Sprite mainPartyMemberSwitchSelectedBackground { get; private set; }
    public static Sprite mainPartyMemberSwitchSourceBackground { get; private set; }
    //Standard
    public static Sprite partyMemberHealthySelectedBackground { get; private set; }
    public static Sprite partyMemberHealthyNonSelectedBackground { get; private set; }
    public static Sprite partyMemberFaintedSelectedBackground { get; private set; }
    public static Sprite partyMemberFaintedNonSelectedBackground { get; private set; }
    public static Sprite partyMemberSwitchSelectedBackground { get; private set; }
    public static Sprite partyMemberSwitchSourceBackground { get; private set; }

    void SetUpStaticArt()
    {
        mainPartyMemberHealthySelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotHealthySelected.png", typeof(Sprite));
        mainPartyMemberHealthyNonSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotHealthy.png", typeof(Sprite));
        mainPartyMemberFaintedSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotFaintedSelected.png", typeof(Sprite));
        mainPartyMemberFaintedNonSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotFainted.png", typeof(Sprite));
        mainPartyMemberSwitchSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotSwitchSelected.png", typeof(Sprite));
        mainPartyMemberSwitchSourceBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/MainSlotSwitch.png", typeof(Sprite));

        partyMemberHealthySelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotHealthySelected.png", typeof(Sprite));
        partyMemberHealthyNonSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotHealthy.png", typeof(Sprite));
        partyMemberFaintedSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotFaintedSelected.png", typeof(Sprite));
        partyMemberFaintedNonSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotFainted.png", typeof(Sprite));
        partyMemberSwitchSelectedBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotSwitchSelected.png", typeof(Sprite));
        partyMemberSwitchSourceBackground = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Art/Party/PokemonParty/StandardSlotSwitch.png", typeof(Sprite));
    }

    public static Sprite ReturnBackgroundArt(int currentHealth, bool isFirstSlot = false, bool isSelected = false, bool switching = false)
    {
        if (isFirstSlot == true)
        {
            if (switching == true)
            {
                if (isSelected == true)
                {
                    return mainPartyMemberSwitchSelectedBackground;
                }
                else//isSelected == false
                {
                    return mainPartyMemberSwitchSourceBackground;
                }
            }

            if (currentHealth > 0)
            {
                if (isSelected == true)
                {
                    return mainPartyMemberHealthySelectedBackground;
                }
                else//isSelected == false
                {
                    return mainPartyMemberHealthyNonSelectedBackground;
                }
            }
            else
            {
                if (isSelected == true)
                {
                    return mainPartyMemberFaintedSelectedBackground;
                }
                else//isSelected == false
                {
                    return mainPartyMemberFaintedNonSelectedBackground;
                }
            }
        }
        else//Not First Slot
        {
            if (switching == true)
            {
                if (isSelected == true)
                {
                    return partyMemberSwitchSelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberSwitchSourceBackground;
                }
            }

            if (currentHealth > 0)
            {
                if (isSelected == true)
                {
                    return partyMemberHealthySelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberHealthyNonSelectedBackground;
                }
            }
            else
            {
                if (isSelected == true)
                {
                    return partyMemberFaintedSelectedBackground;
                }
                else//isSelected == false
                {
                    return partyMemberFaintedNonSelectedBackground;
                }
            }
        }
    }
}
