using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, BattleOver}

public class BattleSystem : CoreSystem
{
    [SerializeField] Image backgroundArt;
    [SerializeField] BattleUnit playerBattleUnit;
    [SerializeField] BattleUnit enemyBattleUnit;
    TurnAttackDetails playerTurnAttackDetails = new TurnAttackDetails();
    TurnAttackDetails enemyTurnAttackDetails = new TurnAttackDetails();
    DamageDetails damageDetails = new DamageDetails();

    PartySystem partySystem;
    InventorySystem inventorySystem;
    DialogManager dialogSystem;

    [SerializeField] ActionSelectionEventSelector actionSelectionEventSelector;
    [SerializeField] AttackSelectionEventSelector attackSelectionEventSelector;

    List<TurnAttackDetails> currentTurnDetails = new List<TurnAttackDetails>();
    TurnAttackDetails currentAttack;
    MoveBase alteredMove;

    PlayerController _playerController;
    TrainerController _trainerController;
    Pokemon _wildPokemon;
    bool _isTrainerBattle;
    bool _playerPokemonShift;

    bool waitUntilUserFinished = false;

    static WeatherEffectBase _currentWeather;
    public static bool inBattle { get; private set; } = false;

    List<EntryHazardBase> _playerSideEntryHazards = new List<EntryHazardBase>();
    List<EntryHazardBase> _enemySideEntryHazards = new List<EntryHazardBase>();

    [SerializeField] GameObject inGameItem;
    Vector2 inGameItemoffScreenPos;

    const string BUT_IT_FAILED = "But it failed";
    int _escapeAttempts;
    public int battleDuration { get; private set; }

    [SerializeField] LearnNewMoveUI learnNewMoveUI;
    [SerializeField] LevelUpUI levelUpUI;

    [Header("Special Move with Conditions")]
    [SerializeField] MoveBase struggle;
    [SerializeField] MoveBase noRetreat;
    [SerializeField] MoveBase encore;
    [SerializeField] MoveBase dreamEater;
    [SerializeField] MoveBase synthesis;
    [SerializeField] MoveBase moonlight;
    [SerializeField] MoveBase morningSun;
    [SerializeField] MoveBase shoreUp;
    [SerializeField] MoveBase purify;
    [SerializeField] MoveBase rest;
    [SerializeField] MoveBase disabled;
    [SerializeField] MoveBase reflect;
    [SerializeField] MoveBase lightScreen;
    [SerializeField] MoveBase auroraVeil;

    //pooling all the conditions on the pokemon prior to checking them
    List<ConditionBase> allConditionsOnPokemon = new List<ConditionBase>();

    #region End of Turn effects Order reference as of GEN 5

    //1.0 weather ends

    //2.0 Sandstorm damage, Hail damage, Rain Dish, Dry Skin, Ice Body

    //3.0 Future Sight, Doom Desire

    //4.0 Wish

    //5.0 Fire Pledge + Grass Pledge damage
    //5.1 Shed Skin, Hydration, Healer
    //5.2 Leftovers, Black Sludge

    //6.0 Aqua Ring

    //7.0 Ingrain

    //8.0 Leech Seed

    //9.0 (bad) poison damage, burn damage, Poison Heal
    //9.1 Nightmare

    //10.0 Curse(from a Ghost-type)

    //11.0 Bind, Wrap, Fire Spin, Clamp, Whirlpool, Sand Tomb, Magma Storm

    //12.0 Taunt ends

    //13.0 Encore ends

    //14.0 Disable ends, Cursed Body ends

    //15.0 Magnet Rise ends

    //16.0 Telekinesis ends

    //17.0 Heal Block ends

    //18.0 Embargo ends

    //19.0 Yawn

    //20.0 Perish Song

    //21.0 Reflect ends
    //21.1 Light Screen ends
    //21.2 Safeguard ends
    //21.3 Mist ends
    //21.4 Tailwind ends
    //21.5 Lucky Chant ends
    //21.6 Water Pledge + Fire Pledge ends, Fire Pledge + Grass Pledge ends, Grass Pledge + Water Pledge ends

    //22.0 Gravity ends

    //23.0 Trick Room ends

    //24.0 Wonder Room ends

    //25.0 Magic Room ends

    //26.0 Uproar message
    //26.1 Speed Boost, Bad Dreams, Harvest, Moody
    //26.2 Toxic Orb activation, Flame Orb activation, Sticky Barb

    //27.0 Zen Mode

    //28.0 Pokémon is switched in (if previous Pokémon fainted)
    //28.1 Healing Wish, Lunar Dance
    //28.2 Spikes, Toxic Spikes, Stealth Rock(hurt in the order they are first used)

    //29.0 Slow Start

    //40.0 Roost

    #endregion

    public override void Initialization()
    {
        gameObject.SetActive(false);
        partySystem = GameManager.instance.GetPartySystem;
        inventorySystem = GameManager.instance.GetInventorySystem;
        dialogSystem = GameManager.instance.GetDialogSystem;
        inGameItemoffScreenPos = inGameItem.transform.localPosition;
        levelUpUI.HandleStart();
        actionSelectionEventSelector.Initialization(PlayerActionFight, PlayerActionBag, PlayerActionPokemon, PlayerActionRun);
        attackSelectionEventSelector.Initialization();

        if (playerBattleUnit == null)
        {
            Debug.LogWarning($"playerBattleUnit has not been set");
        }

        if (enemyBattleUnit == null)
        {
            Debug.LogWarning($"enemyBattleUnit has not been set");
        }

        if (dialogSystem == null)
        {
            Debug.LogWarning($"dialogBox has not been set");
        }

        if (actionSelectionEventSelector == null)
        {
            Debug.LogWarning($"actionSelectionEventSelector has not been set");
        }

        if (attackSelectionEventSelector == null)
        {
            Debug.LogWarning($"attackSelectionEventSelector has not been set");
        }

        if (inGameItem == null)
        {
            Debug.LogWarning($"inGameItem has not been set");
        }

        if (learnNewMoveUI == null)
        {
            Debug.LogWarning($"learnNewMoveManager has not been set");
        }

        //alteredMove = MoveBase.CreateInstance<MoveBase>();
    }

    public override void HandleUpdate()
    {
        //If B button is pressed go back a menu
        if(Input.GetButtonDown("Fire2"))
        {
            if(attackSelectionEventSelector.isActiveAndEnabled == true)
            {
                EnableActionSelector(true);
                EnableAttackMoveSelector(false);
            }
        }
    }

    #region Battle Setup

    public void SetupBattleArt(BattleFieldLayoutBaseSO levelArt)
    {
        backgroundArt.sprite = levelArt.GetBackground;
        playerBattleUnit.SetBattlePositionArt(levelArt.GetPlayerPosition);
        enemyBattleUnit.SetBattlePositionArt(levelArt.GetEnemyPosition);
    }

    public void StartBattle(PlayerController player, Pokemon wildPokemon)
    {
        _playerController = player;

        Pokemon newWildPokemon = new Pokemon(wildPokemon.pokemonBase,wildPokemon.currentLevel);

        _trainerController = null;
        _wildPokemon = newWildPokemon;
        _isTrainerBattle = false;

        StartCoroutine(SetupBattle());
    }

    public void StartBattle(PlayerController player, TrainerController trainer)
    {
        _playerController = player;
        
        _trainerController = trainer;
        _isTrainerBattle = true;

        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// Goes through animations and sets up both the current pokemon and the enemy pokemon, 
    /// all available attacks along with their PP and names
    /// </summary>
    IEnumerator SetupBattle()
    {
        currentTurnDetails.Clear();
        dialogSystem.SetCurrentDialogBox(dialogBox);
        inBattle = true;
        _playerSideEntryHazards = new List<EntryHazardBase>();
        _enemySideEntryHazards = new List<EntryHazardBase>();
        Pokemon playerPokemon = _playerController.pokemonParty.GetFirstHealthyPokemon();

        if (_isTrainerBattle == true)
        {
            Pokemon enemyPokemon = _trainerController.pokemonParty.GetFirstHealthyPokemon();
            playerBattleUnit.SetDataBattleStart(playerPokemon, _playerController.BackBattleSprite[0]);
            enemyBattleUnit.SetDataBattleStart(enemyPokemon, _trainerController.FrontBattleSprite[0]);

            yield return dialogSystem.TypeDialog($"{_trainerController.TrainerName} wants to battle!");
            yield return new WaitUntil(() => playerBattleUnit.startingAnimationsActive == false && enemyBattleUnit.startingAnimationsActive == false);
            yield return new WaitForSeconds(0.5f);
            yield return dialogSystem.AfterDialogWait();

            yield return enemyBattleUnit.PlayTrainerExitAnimation(true);

            yield return dialogSystem.TypeDialog($"{_trainerController.TrainerName} sent out {enemyPokemon.currentName}");

            enemyBattleUnit.SendOut();

            yield return new WaitForSeconds(0.5f);

            yield return dialogSystem.TypeDialog($"Go {playerPokemon.currentName}");

            yield return playerBattleUnit.PlayTrainerExitAnimation(true);
            playerBattleUnit.SendOut();

            yield return new WaitForSeconds(1f);
        }
        else //WildPokemon
        {
            playerBattleUnit.SetDataBattleStart(playerPokemon, _playerController.BackBattleSprite[0]);
            enemyBattleUnit.SetDataBattleStart(_wildPokemon,null);

            yield return dialogSystem.TypeDialog($"A wild {enemyBattleUnit.pokemon.currentName} has appeared!");
            yield return enemyBattleUnit.PlayTrainerExitAnimation(false);

            yield return dialogSystem.TypeDialog($"Go {playerPokemon.currentName}");
            yield return playerBattleUnit.PlayTrainerExitAnimation(true);
            playerBattleUnit.SendOut();

            yield return new WaitForSeconds(1f);
        }

        _playerController.pokemonParty.SetOriginalPositions();
        _playerController.pokemonParty.CleanUpPartyOrderOnStart(playerBattleUnit.pokemon);
        _escapeAttempts = 0;
        battleDuration = 0;
        enemyBattleUnit.AddPokemonToBattleList(playerBattleUnit.pokemon);
        BattleStartSetup();
        attackSelectionEventSelector.SetMovesList(playerBattleUnit,playerBattleUnit.pokemon.moves);

        _currentWeather = null;

        yield return PokemonEntry(playerBattleUnit,enemyBattleUnit);
        yield return PokemonEntry(enemyBattleUnit,playerBattleUnit);

        actionSelectionEventSelector.NewBattle();
        PlayerActions();
    }

    void BattleStartSetup()
    {
        EnableActionSelector(false);
        EnableAttackMoveSelector(false);
    }

    #endregion

    #region Player Actions

    /// <summary>
    /// sets up the players action box, with the cursor/event system selected on the first box
    /// Sets up the listeners for all the current selections
    /// </summary>
    void PlayerActions()
    {
        dialogSystem.SetDialogText($"What will {playerBattleUnit.pokemon.currentName} do?");
        EnableActionSelector(true);
    }

    /// <summary>
    /// Player Selected the Fight Button
    /// </summary>
    void PlayerActionFight()
    {
        EnableActionSelector(false);

        if(playerBattleUnit.NoMovesAvailable() == true)
        {
            PlayerPokemonHasNoMovesLeft();
        }
        else
        {
            if(playerBattleUnit.IsEncored() == true)
            {
                AttackSelected(playerBattleUnit,playerBattleUnit.lastMoveUsed);
            }
            EnableAttackMoveSelector(true);
        }
    }

    /// <summary>
    /// Player Selected The Pokemon Button
    /// </summary>
    void PlayerActionPokemon()
    {
        if(playerBattleUnit.CanSwitchOutOrFlee() == true)
        {
            partySystem.OpenSystem();
        }
        else
        {
            if(playerBattleUnit.pokemon.GetHoldItemEffects.AlwaysAllowsToSwitchOut() == true)
            {
                partySystem.OpenSystem();
            }
            else
            {
                PlayerCantEscapeActiveWhenTryingToSwitch();
            }
        }
    }

    /// <summary>
    /// Player Selected The Bag Button
    /// </summary>
    void PlayerActionBag()
    {
        inventorySystem.OpenSystem();
    }

    /// <summary>
    /// Player Selected the Run Button
    /// </summary>
    void PlayerActionRun()
    {
        EnableActionSelector(false);
        StartCoroutine(RunFromBattle());
    }

    /// <summary>
    /// Turns on the Action Box, with all available buttons (Fight,Bag,Pokemon and Run) and selects the first option
    /// </summary>
    /// <param name="enabled"></param>
    void EnableActionSelector(bool enabled)
    {
        actionSelectionEventSelector.gameObject.SetActive(enabled);
        if (enabled == true)
        {
            actionSelectionEventSelector.Select();
        }
    }

    /// <summary>
    /// Turns on/off the current Dialog box and the updater for the PP system as well as the type of move it is
    /// </summary>
    /// <param name="enabled"></param>
    void EnableAttackMoveSelector(bool enabled)
    {
        attackSelectionEventSelector.EnableMoveSelector(enabled);
        if (enabled == true)
        {
            attackSelectionEventSelector.Select();
        }
    }

    /// <summary>
    /// Called from the attack button listener passing the correct information to process through what pokemon did what attack
    /// </summary>
    /// <param name="currentPokemon">Current Battle Unit Pokemon</param>
    /// <param name="moveBase">Current Move Being Used</param>
    public void AttackSelected(BattleUnit currentPokemon, Move move)
    {
        if (playerBattleUnit.pokemon.GetHoldItemEffects.PreventTheUseOfCertainMoves(currentPokemon,move.moveBase) == true)
        {
            PlayerPokemonItemPreventsMoveFromBeingUsed(currentPokemon.pokemon.GetHoldItemEffects);
            return;
        }

        if(move.disabled == true)
        {
            PlayerPokemonAttackIsDisabled(currentPokemon.pokemon, move);
            return;
        }

        dialogSystem.SetDialogText("");
        EnableAttackMoveSelector(false);
        playerTurnAttackDetails.SetAttackDetails(move, currentPokemon, enemyBattleUnit);

        _escapeAttempts = 0;
        currentTurnDetails.Add(playerTurnAttackDetails);
        StartCoroutine(EnemyMove());
    }

    public void AttackHasRunOutOfPP()
    {
        StartCoroutine(AttackHasRunOutOfPPIEnumerator());
    }

    IEnumerator AttackHasRunOutOfPPIEnumerator()
    {
        EnableAttackMoveSelector(false);
        yield return dialogSystem.TypeDialog("There's no PP left for this move!");
        yield return new WaitForSeconds(1);
        EnableAttackMoveSelector(true);
    }

    public void PlayerPokemonHasNoMovesLeft()
    {
        StartCoroutine(PlayerPokemonHasNoMovesLeftIEnumerator());
    }

    IEnumerator PlayerPokemonHasNoMovesLeftIEnumerator()
    {
        EnableAttackMoveSelector(false);
        yield return dialogSystem.TypeDialog($"{playerBattleUnit.pokemon.currentName} has no available moves left");
        yield return new WaitForSeconds(1);
        AttackSelected(playerBattleUnit, new Move(struggle));
    }

    public void PlayerPokemonItemPreventsMoveFromBeingUsed(HoldItemBase holdItem)
    {
        StartCoroutine(PlayerPokemonItemPreventsMoveFromBeingUsedIEnumerator(holdItem));
    }

    IEnumerator PlayerPokemonItemPreventsMoveFromBeingUsedIEnumerator(HoldItemBase holdItem)
    {
        EnableAttackMoveSelector(false);
        yield return dialogSystem.TypeDialog($"{holdItem.SpecializedMessage(playerBattleUnit.pokemon,enemyBattleUnit.pokemon)}");
        yield return new WaitForSeconds(1);
        dialogSystem.SetDialogText($"What will {playerBattleUnit.pokemon.currentName} do?");
        EnableAttackMoveSelector(true);
    }

    public void PlayerPokemonAttackIsDisabled(Pokemon pokemon,Move move)
    {
        StartCoroutine(PlayerPokemonAttackIsDisabledIEnumerator(pokemon,move));
    }

    IEnumerator PlayerPokemonAttackIsDisabledIEnumerator(Pokemon pokemon, Move move)
    {
        EnableAttackMoveSelector(false);
        yield return dialogSystem.TypeDialog($"{pokemon.currentName}'s {move.moveBase.MoveName} is disabled");
        yield return new WaitForSeconds(1);
        dialogSystem.SetDialogText($"What will {playerBattleUnit.pokemon.currentName} do?");
        EnableAttackMoveSelector(true);
    }

    public void PlayerCantEscapeActiveWhenTryingToSwitch()
    {
        StartCoroutine(PlayerCantEscapeActiveWhenTryingToSwitchIEnumerator());
    }

    IEnumerator PlayerCantEscapeActiveWhenTryingToSwitchIEnumerator()
    {
        EnableActionSelector(false);
        yield return dialogSystem.TypeDialog($"{playerBattleUnit.pokemon.currentName} can't escape!",true);
        EnableActionSelector(true);
    }

    public void ReturnFromPokemonAlternateSystem()
    {
        actionSelectionEventSelector.Select();
        dialogSystem.SetCurrentDialogBox(dialogBox);
    }

    IEnumerator RunFromBattle()
    {
        if (_isTrainerBattle == true)
        {
            yield return dialogSystem.TypeDialog($"You cant run from a trainer battle", true);
            PlayerActions();
            yield break;
        }

        if(playerBattleUnit.pokemon.GetHoldItemEffects.FleeWithoutFail() == true)
        {
            yield return dialogSystem.TypeDialog(playerBattleUnit.pokemon.GetHoldItemEffects.SpecializedMessage(playerBattleUnit.pokemon,enemyBattleUnit.pokemon), true);
            OnBattleOver(true);
            yield break;
        }

        if(playerBattleUnit.CanSwitchOutOrFlee() == false)
        {
            yield return dialogSystem.TypeDialog($"Can't Escape", true);
            PlayerActions();
            yield break;
        }

        _escapeAttempts++;
        int playerSpeed = playerBattleUnit.pokemon.speed;
        int enemySpeed = enemyBattleUnit.pokemon.speed;

        if (playerSpeed > enemySpeed)
        {
            yield return dialogSystem.TypeDialog($"You got away safely", true);
            OnBattleOver(true);
            yield break;
        }
        else
        {
            float f = ((playerSpeed * 128) / enemySpeed) + (30 * _escapeAttempts);
            f = f % 256;

            int rnd = Random.Range(0, 256);
            if (rnd < f)
            {
                yield return dialogSystem.TypeDialog($"You got away safely", true);
                OnBattleOver(true);
                yield break;
            }
            else
            {
                yield return dialogSystem.TypeDialog($"You were unable to escape!", true);
                StartCoroutine(EnemyMove());
            }
        }
    }

    #endregion

    #region Enemy Actions

    public IEnumerator EnemyMove()
    {
        Move currentAttack = enemyBattleUnit.ReturnRandomMove();

        if(currentAttack == null)
        {
            currentAttack = new Move(struggle);
        }

        enemyTurnAttackDetails.SetAttackDetails(currentAttack, enemyBattleUnit, playerBattleUnit);
        currentTurnDetails.Add(enemyTurnAttackDetails);

        yield return RunThroughTurns(currentTurnDetails);
    }

    IEnumerator TrainerAboutToUsePokemonFeature(Pokemon nextPokemon)
    {
        yield return dialogSystem.TypeDialog($"{_trainerController.TrainerName} is about to use {nextPokemon.pokemonBase.GetPokedexName()}", true);

        yield return dialogSystem.TypeDialog($"Will {_playerController.TrainerName} change Pokemon?");

        yield return dialogSystem.SetChoiceBox(() =>
        {
            waitUntilUserFinished = false;
            partySystem.OpenSystem(true);
        }
        , () =>
        {
            waitUntilUserFinished = true;
            _playerPokemonShift = false;
        });

        yield return new WaitUntil(() => waitUntilUserFinished == true);
        yield return SwitchPokemonIEnumerator(enemyBattleUnit, nextPokemon);
    }

    IEnumerator TrainerBattleOver(bool playerHasWon)
    {
        if (playerHasWon == false)
        {
            enemyBattleUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(0.75f);
        }

        yield return enemyBattleUnit.TrainerToField();
        List<string> trainerLines = _trainerController.OnBattleOverDialog(playerHasWon);

        for (int i = 0; i < trainerLines.Count; i++)
        {
            yield return dialogSystem.TypeDialog($"{trainerLines[i]}", true);
        }

        if (playerHasWon == true)
        {
            //Show dialog of player recieving money
        }
    }

    #endregion

    #region Battle Cycle

    IEnumerator RunThroughTurns(List<TurnAttackDetails> attacksChosen)
    {
        yield return RearrangeAttackOrder();

        while(attacksChosen.Count > 0)
        {
            currentAttack = attacksChosen[0];

            if (currentAttack.attackingPokemon.pokemon.currentHitPoints > 0)
            {
                yield return RunMove(currentAttack.attackingPokemon, currentAttack.targetPokmeon, currentAttack.currentMove);
            }

            attacksChosen.Remove(currentAttack);
        }

        if(_currentWeather != null)
        {
            yield return ShowWeatherEffect(_currentWeather);
            if (_currentWeather != null)
            {
                yield return ApplyWeatherEffectsOnEndTurn(_currentWeather, playerBattleUnit);
                yield return ApplyWeatherEffectsOnEndTurn(_currentWeather, enemyBattleUnit);
            }
        }

        //Called after all attacks have been done

        yield return ApplyEffectsOnEndTurn(playerBattleUnit,enemyBattleUnit);
        yield return ApplyEffectsOnEndTurn(enemyBattleUnit, playerBattleUnit);

        //If current pokemon has fainted then it goes to the party system and waits on the selector
        if (playerBattleUnit.SendOutPokemonOnTurnEnd == true)
        {
            partySystem.OpenSystem();
        }
        else
        {
            if(enemyBattleUnit.SendOutPokemonOnTurnEnd == true)
            {
                //This will be updated with a better AI later
                Pokemon nextEnemyPokemon = _trainerController.pokemonParty.GetFirstHealthyPokemon();

                if(_playerController.pokemonParty.HealthyPokemonCount() > 1)
                {
                    _playerPokemonShift = true;
                    yield return TrainerAboutToUsePokemonFeature(nextEnemyPokemon);
                }
                else
                {
                    yield return SwitchPokemonIEnumerator(enemyBattleUnit, nextEnemyPokemon);
                }
            }
            PlayerActions();
        }
        battleDuration++;
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        //If the pokemon was encored after selecting the move, switch it to the new move style
        if(sourceUnit.IsEncored() == true)
        {
            if (move != sourceUnit.lastMoveUsed)
            {
                move = sourceUnit.lastMoveUsed;
            }
        }

        if(move.disabled == true)
        {
            yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s {move.moveBase.MoveName} is disabled");
            yield break;
        }

        sourceUnit.lastMoveUsed = move;
        //This is here incase the pokemon hits itself in confusion for the smooth animation
        int previousHP = sourceUnit.pokemon.currentHitPoints;

        //due to animations instead of it returning a bool it will return the animation
        ConditionID canUseMove = sourceUnit.pokemon.OnBeforeMove(targetUnit.pokemon);
        //If confused play pre animation

        if (canUseMove != ConditionID.NA)
        {
            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield return sourceUnit.StatusConditionAnimation(canUseMove);
            //If it hit itself in its confusion update the HUD
            yield return sourceUnit.HUD.UpdateHP(previousHP);
            if (sourceUnit.pokemon.currentHitPoints <= 0)
            {
                yield return PokemonHasFainted(sourceUnit);
            }
            yield break;
        }
        //This is here incase the pokemon has a status effect that ended such as being frozen/Sleep
        yield return ShowStatusChanges(sourceUnit.pokemon);

        yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName} used {move.moveBase.MoveName}");
        move.pP--;

        if(targetUnit.pokemon.ability.ReducesPowerPointsBy2() == true)
        {
            move.pP--;
        }

        if (targetUnit.pokemon.currentHitPoints <= 0 && move.moveBase.Target == MoveTarget.Foe && move.moveBase.RecoilPercentage < 100)
        {
            yield return dialogSystem.TypeDialog($"There is no target Pokemon");
            yield break;
        }

        if (targetUnit.pokemon.ability.PreventsTheUseOfSpecificMoves(sourceUnit.pokemon, move.moveBase) == true && targetUnit.pokemon.currentHitPoints > 0)
        {
            targetUnit.OnAbilityActivation();
            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield break;
        }

        int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP and recoil

        if (CheckIfMoveHasSpecializedConditionAndSuccessful(sourceUnit, targetUnit, move.moveBase) == false)
        {
            yield return sourceUnit.PlayAttackAnimation();
            yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
            yield break;
        }

        alteredMove = sourceUnit.pokemon.ability.AlterMoveDetails(move.moveBase);
        alteredMove = sourceUnit.pokemon.GetHoldItemEffects.AlterUserMoveDetails(alteredMove);
        alteredMove = targetUnit.pokemon.GetHoldItemEffects.AlterOpposingMoveDetails(alteredMove);
        alteredMove = sourceUnit.pokemon.ability.BoostsMovePowerWhenLast(currentTurnDetails.Count <= 1, alteredMove);

        if (CheckIfMoveHits(alteredMove, sourceUnit.pokemon, targetUnit.pokemon) == true)
        {
            if (alteredMove.MoveType == MoveType.Status)
            {
                yield return sourceUnit.PlayAttackAnimation();
                yield return RunMoveEffects(alteredMove.MoveEffects, sourceUnit, targetUnit, alteredMove, alteredMove.Target);
            }
            else
            {
                int attackLoop = 1;

                if(alteredMove.MultiStrikeMove == true)
                {
                    if(alteredMove.FixedNumberOfStrikes > 0)
                    {
                        attackLoop = alteredMove.FixedNumberOfStrikes;
                    }
                    else
                    {
                        if (sourceUnit.pokemon.ability.MaximizeMultistrikeMovesHit() == true)
                        {
                            attackLoop = 5;
                        }
                        else
                        {
                            attackLoop = VariableNumberOfStrikes();
                        }
                    }
                }

                targetUnit.pokemon.TakeDamage(damageDetails, alteredMove, sourceUnit.pokemon,targetUnit.shields);

                if (damageDetails.sourceItemUsed == true)
                {
                    if(sourceUnit.pokemon.GetHoldItemEffects.PlayAnimationWhenUsed() == true)
                    {
                        yield return sourceUnit.PlayItemUsedAnimation();
                        yield return dialogSystem.TypeDialog(sourceUnit.pokemon.GetHoldItemEffects.SpecializedMessage(sourceUnit.pokemon, targetUnit.pokemon));
                    }
                    sourceUnit.pokemon.ItemUsed();
                }

                yield return sourceUnit.PlayAttackAnimation();

                if (damageDetails.targetItemUsed == true)
                {
                    if(targetUnit.pokemon.GetHoldItemEffects.PlayAnimationWhenUsed() == true)
                    {
                        yield return targetUnit.PlayItemUsedAnimation();
                    }
                    yield return dialogSystem.TypeDialog(targetUnit.pokemon.GetHoldItemEffects.SpecializedMessage(targetUnit.pokemon, sourceUnit.pokemon));
                    targetUnit.pokemon.ItemUsed();
                }

                bool disableMoveAfterAnimation = targetUnit.pokemon.ability.DisableMove(sourceUnit, move);
                if (disableMoveAfterAnimation == true)
                {
                    if (attackLoop > 1)
                    {
                        attackLoop = 1;
                    }
                }

                if (attackLoop > 1)
                {
                    dialogSystem.SetDialogText("");

                    for (int i = 0; i < attackLoop; i++)
                    {
                        if (i > 0)
                        {
                            targetUnit.pokemon.TakeDamage(damageDetails,alteredMove, sourceUnit.pokemon, targetUnit.shields);
                        }

                        if (alteredMove.Target == MoveTarget.Foe && damageDetails.typeEffectiveness != 0)
                        {
                            targetUnit.PlayHitAnimation();
                        }

                        if (targetUnit.pokemon.ability.DisableMove(sourceUnit, move) == true)
                        {
                            targetUnit.OnAbilityActivation();
                            yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s {move.moveBase.MoveName} is disabled");
                            attackLoop = i + 1;
                            break;
                        }

                        if (damageDetails.damageNullified == true)
                        {
                            break;
                        }

                        yield return targetUnit.HUD.UpdateHP(hpPriorToAttack);

                        if(damageDetails.criticalHit > 1 && i < attackLoop)
                        {
                            yield return dialogSystem.TypeDialog("A Critical Hit!");
                        }

                        if (damageDetails.typeEffectiveness == 0)
                        {
                            break;
                        }

                        if(targetUnit.pokemon.currentHitPoints <=0)
                        {
                            attackLoop = i +1;
                            break;
                        }

                        hpPriorToAttack = targetUnit.pokemon.currentHitPoints;

                        yield return new WaitForSeconds(1f);
                    }
                }
                else
                {
                    if (alteredMove.Target == MoveTarget.Foe && damageDetails.typeEffectiveness != 0)
                    {
                        targetUnit.PlayHitAnimation();
                    }

                    if (disableMoveAfterAnimation == true)
                    {
                        targetUnit.OnAbilityActivation();
                        yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s {move.moveBase.MoveName} is disabled");
                    }

                    if (hpPriorToAttack != targetUnit.pokemon.currentHitPoints)
                    {
                        yield return targetUnit.HUD.UpdateHP(hpPriorToAttack);
                    }
                }
                
                yield return ShowDamageDetails(damageDetails, targetUnit);
                if(damageDetails.damageNullified == true)
                {
                    yield return ApplyStatChanges(damageDetails.defendersStatBoostByAbility, targetUnit, MoveTarget.Foe);
                    yield return ApplyStatChanges(damageDetails.attackersStatBoostByDefendersAbility, sourceUnit, MoveTarget.Self, sourceUnit);

                    yield break;
                }

                if(targetUnit.pokemon.currentHitPoints > 0)
                {
                    yield return ApplyStatChanges(damageDetails.defendersStatBoostByAbility, targetUnit, MoveTarget.Foe);
                    yield return ApplyStatChanges(damageDetails.alterStatAfterTakingDamageFromCertainTypeItem, targetUnit, MoveTarget.Foe);
                }

                if (sourceUnit.pokemon.currentHitPoints > 0)
                {
                    //TO DO
                    //check if the pokemon was the last one, if it is then dont go through this code

                    if (damageDetails.attackersAbilityActivation == true)
                    {
                        sourceUnit.OnAbilityActivation();
                    }
                    yield return ApplyStatChanges(damageDetails.attackersStatBoostByDefendersAbility, targetUnit, MoveTarget.Self,sourceUnit);
                }

                if (alteredMove.MultiStrikeMove == true && damageDetails.typeEffectiveness != 0)
                {
                    yield return dialogSystem.TypeDialog($"Hit {attackLoop} time(s)!");
                }

                if (targetUnit.pokemon.ability.DamagesAttackerUponFinishingHit(targetUnit.pokemon, sourceUnit.pokemon, alteredMove) == true)
                {
                    targetUnit.OnAbilityActivation();
                    yield return ShowStatusChanges(sourceUnit.pokemon);
                    yield return sourceUnit.HUD.UpdateHP(previousHP);
                    previousHP = sourceUnit.pokemon.currentHitPoints;
                }

                //Healing
                if (sourceUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == false)
                {
                    if (alteredMove.DrainsHP == true && sourceUnit.pokemon.currentHitPoints != sourceUnit.pokemon.maxHitPoints && hpPriorToAttack - targetUnit.pokemon.currentHitPoints > 0)
                    {
                        int hpHealed = Mathf.CeilToInt((hpPriorToAttack - targetUnit.pokemon.currentHitPoints) * (alteredMove.HpRecovered));

                        hpHealed = Mathf.Clamp(hpHealed, 1, sourceUnit.pokemon.maxHitPoints - sourceUnit.pokemon.currentHitPoints);
                        sourceUnit.pokemon.UpdateHPRestored(hpHealed);
                        yield return sourceUnit.HUD.UpdateHP(previousHP);
                        yield return dialogSystem.TypeDialog($"{targetUnit.pokemon.currentName} had its energy drained");
                    }
                }

                //Shows that the damage does not effect them and then ends the move right there
                if (damageDetails.typeEffectiveness == 0)
                {
                    yield break;
                }
            }

            if (alteredMove.SecondaryEffects != null && alteredMove.SecondaryEffects.Count > 0 && targetUnit.pokemon.currentHitPoints > 0)
            {
                foreach (var secondaryEffect in alteredMove.SecondaryEffects)
                {
                    int rnd = Random.Range(1, 101);
                    if (rnd <= secondaryEffect.PercentChance)
                    {
                        yield return RunMoveEffects(secondaryEffect, sourceUnit, targetUnit, alteredMove, secondaryEffect.Target, (secondaryEffect.PercentChance<100)) ;

                        if (secondaryEffect.Volatiletatus == ConditionID.CursedUser)
                        {
                            yield return sourceUnit.HUD.UpdateHP(previousHP);
                            if (sourceUnit.pokemon.currentHitPoints <= 0)
                            {
                                yield return PokemonHasFainted(sourceUnit);
                            }
                        }
                    }
                }
            }

            ConditionID newCondition = targetUnit.pokemon.ability.ContactMoveMayCauseStatusEffect(targetUnit.pokemon, sourceUnit.pokemon, alteredMove);

            if(sourceUnit.pokemon.GetHoldItemEffects.ProtectHolderFromEffectsCausedByMakingDirectContact() == true)
            {
                newCondition = ConditionID.NA;
            }

            if (newCondition > ConditionID.NA && newCondition <= ConditionID.ToxicPoison)
            {
                if (sourceUnit.pokemon.status == null)
                {
                    targetUnit.OnAbilityActivation();
                    sourceUnit.pokemon.SetStatus(newCondition, false);
                    yield return ShowStatusChanges(sourceUnit.pokemon);
                }
            }
            else if (newCondition > ConditionID.ToxicPoison && targetUnit.pokemon.currentHitPoints > 0)
            {
                if (sourceUnit.pokemon.HasCurrentVolatileStatus(newCondition) == false)
                {
                    targetUnit.OnAbilityActivation();
                    sourceUnit.pokemon.SetVolatileStatus(newCondition, alteredMove,sourceUnit);
                    yield return ShowStatusChanges(sourceUnit.pokemon);
                }
            }

            //Recoil
            if (alteredMove.RecoilType != Recoil.NA)
            {
                
                if(sourceUnit.pokemon.ability.PreventsRecoilDamage(alteredMove) == false)
                {
                    int recoilDamage = 0;

                    if (alteredMove.RecoilType == Recoil.DamageDealt)
                    {
                        recoilDamage = hpPriorToAttack - targetUnit.pokemon.currentHitPoints;
                        recoilDamage = Mathf.CeilToInt(recoilDamage * alteredMove.RecoilPercentage);
                    }
                    else if (alteredMove.RecoilType == Recoil.UsersCurrentHP)
                    {
                        recoilDamage = sourceUnit.pokemon.currentHitPoints;
                        recoilDamage = Mathf.CeilToInt(recoilDamage * alteredMove.RecoilPercentage);
                    }
                    else//users max hp
                    {
                        recoilDamage = sourceUnit.pokemon.maxHitPoints;
                        recoilDamage = Mathf.CeilToInt(recoilDamage * alteredMove.RecoilPercentage);
                    }

                    previousHP = sourceUnit.pokemon.currentHitPoints;
                    sourceUnit.pokemon.UpdateHPDamage(recoilDamage);
                    yield return sourceUnit.HUD.UpdateHP(previousHP);
                    if(alteredMove.RecoilPercentage < 100)
                    {
                        yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName} is hit with recoil!");
                    }

                    if (sourceUnit.pokemon.currentHitPoints <= 0)
                    {
                        yield return PokemonHasFainted(sourceUnit);
                    }
                }
            }

            //Item altering HP
            int hpDifference;
            if (targetUnit.pokemon.GetHoldItemEffects.HurtsAttacker() == true && sourceUnit.pokemon.currentHitPoints > 0)
            {
                hpDifference = targetUnit.pokemon.GetHoldItemEffects.AlterUserHPAfterAttack(sourceUnit.pokemon, alteredMove, (hpPriorToAttack - targetUnit.pokemon.currentHitPoints));
                previousHP = sourceUnit.pokemon.currentHitPoints;

                if (hpDifference != 0)
                {
                    if (hpDifference > 0)
                    {
                        sourceUnit.pokemon.UpdateHPRestored(hpDifference);
                    }
                    else if (hpDifference < 0)
                    {
                        sourceUnit.pokemon.UpdateHPDamage(-hpDifference);
                    }

                    yield return sourceUnit.HUD.UpdateHP(previousHP);
                    yield return dialogSystem.TypeDialog(targetUnit.pokemon.GetHoldItemEffects.SpecializedMessage(targetUnit.pokemon, sourceUnit.pokemon));

                    if (sourceUnit.pokemon.currentHitPoints <= 0)
                    {
                        yield return PokemonHasFainted(sourceUnit);
                    }
                }
            }
            else if(sourceUnit.pokemon.GetHoldItemEffects.HurtsAttacker() == false && sourceUnit.pokemon.currentHitPoints > 0)
            {
                hpDifference = sourceUnit.pokemon.GetHoldItemEffects.AlterUserHPAfterAttack(sourceUnit.pokemon, alteredMove, (hpPriorToAttack - targetUnit.pokemon.currentHitPoints));
                previousHP = sourceUnit.pokemon.currentHitPoints;

                if (hpDifference != 0)
                {
                    if (hpDifference > 0)
                    {
                        sourceUnit.pokemon.UpdateHPRestored(hpDifference);
                    }
                    else if (hpDifference < 0)
                    {
                        sourceUnit.pokemon.UpdateHPDamage(-hpDifference);
                    }

                    if (sourceUnit.pokemon.GetHoldItemEffects.PlayAnimationWhenUsed() == true)
                    {
                        yield return sourceUnit.PlayItemUsedAnimation();
                    }
                    yield return sourceUnit.HUD.UpdateHP(previousHP);
                    yield return dialogSystem.TypeDialog(sourceUnit.pokemon.GetHoldItemEffects.SpecializedMessage(sourceUnit.pokemon, targetUnit.pokemon));
                }


                if (sourceUnit.pokemon.currentHitPoints <= 0)
                {
                    yield return PokemonHasFainted(sourceUnit);
                }
            }
            
            if(targetUnit.pokemon.GetHoldItemEffects.RemovesMoveBindingEffectsAfterMoveUsed(targetUnit.pokemon) == true)
            {
                if(targetUnit.pokemon.GetHoldItemEffects.RemoveItem == true)
                {
                    if(targetUnit.pokemon.GetHoldItemEffects.PlayAnimationWhenUsed() == true)
                    {
                        yield return targetUnit.PlayItemUsedAnimation();
                    }
                    targetUnit.pokemon.ItemUsed();
                }
            }

            StatBoost statBoost = sourceUnit.pokemon.GetHoldItemEffects.AlterStatAfterUsingSpecificMove(move.moveBase);
            if (statBoost != null)
            {
                yield return sourceUnit.PlayItemUsedAnimation();
                sourceUnit.pokemon.ItemUsed();
                yield return ApplyStatChanges(new List<StatBoost>() { statBoost }, targetUnit, MoveTarget.Self,sourceUnit);
            }
        }
        else
        {
            yield return sourceUnit.PlayAttackAnimation();
            yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s attack missed!");
            StatBoost statBoost = sourceUnit.pokemon.GetHoldItemEffects.RaisesStatUponMissing();
            if(statBoost != null)
            {
                yield return sourceUnit.PlayItemUsedAnimation();
                sourceUnit.pokemon.ItemUsed();
                yield return ApplyStatChanges(new List<StatBoost>() { statBoost }, sourceUnit, MoveTarget.Foe);
            }
        }

        //explosive moves incase they miss
        if (alteredMove.RecoilType == Recoil.UsersMaximumHP && alteredMove.RecoilPercentage >= 100 && sourceUnit.pokemon.currentHitPoints > 0)
        {
            int recoilDamage = sourceUnit.pokemon.currentHitPoints;
            sourceUnit.pokemon.UpdateHPDamage(recoilDamage);
            yield return sourceUnit.HUD.UpdateHP(previousHP);

            if (sourceUnit.pokemon.currentHitPoints <= 0)
            {
                yield return PokemonHasFainted(sourceUnit);
            }
        }

        if (targetUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(targetUnit);
        }
        else
        {
            if(targetUnit.pokemon.GetHoldItemEffects.RestoresAllLoweredStatsToNormalAfterAttackFinished(targetUnit.pokemon) == true)
            {
                yield return targetUnit.PlayItemUsedAnimation();
                yield return dialogSystem.TypeDialog(targetUnit.pokemon.GetHoldItemEffects.SpecializedMessage(targetUnit.pokemon, sourceUnit.pokemon));
                targetUnit.pokemon.ItemUsed();
            }
        }

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
        }
    }

    IEnumerator RunMoveEffects(MoveEffects effects, BattleUnit source, BattleUnit target, MoveBase currentMove,MoveTarget moveTarget, bool wasSecondaryEffect = false)
    {
        if (effects.Boosts != null)
        {
            yield return ApplyStatChanges(effects.Boosts, target, moveTarget, source);
        }

        //Status Condition
        if (effects.Status != ConditionID.NA)
        {
            if (target.pokemon.ability.PreventCertainStatusCondition(effects.Status,GetCurrentWeather) == true)
            {
                if (wasSecondaryEffect == false)
                {
                    target.OnAbilityActivation();
                    target.pokemon.statusChanges.Enqueue(target.pokemon.ability.OnAbilitityActivation(target.pokemon));
                }
            }
            else
            {
                target.pokemon.SetStatus(effects.Status, wasSecondaryEffect);
            }
        }

        //Volatile Status Condition
        if (effects.Volatiletatus != ConditionID.NA)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                if (effects.Volatiletatus == ConditionID.Infatuation)
                {
                    if (CheckIfTargetCanBeInflatuated(source.pokemon, target.pokemon) == true)
                    {
                        target.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,source);
                    }
                    else
                    {
                        yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
                        yield break;
                    }
                }
                else
                {
                    if(target.pokemon.volatileStatus.Exists(x => x.Id == effects.Volatiletatus) == true && wasSecondaryEffect == false)
                    {
                        yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
                        yield break;
                    }
                    target.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,source);
                }
            }
            else if(moveTarget == MoveTarget.Self)
            {
                source.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,source);
            }
            else if(moveTarget == MoveTarget.All)
            {
                if(effects.Volatiletatus == ConditionID.PerishSong)
                {
                    yield return dialogSystem.TypeDialog("All Pokemon that heard the song will faint in 3 turns");
                }

                target.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,source);
                source.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,target);
            }
        }

        if (effects.WeatherEffect != WeatherEffectID.NA)
        {
            yield return StartWeatherEffect(effects.WeatherEffect,source,target);
        }

        if (effects.EntryHazard != EntryHazardID.NA)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                List<EntryHazardBase> currentEntrySide = (target.isPlayerPokemon) ? _playerSideEntryHazards : _enemySideEntryHazards;
                EntryHazardBase currentHazard = EntryHazardsDB.GetEntryHazardBase(effects.EntryHazard);

                if (currentEntrySide.Exists(x => x.Id == currentHazard.Id) == true)
                {
                    currentHazard = currentEntrySide.Find(x => x.Id == effects.EntryHazard);
                    if (currentHazard.CanBeUsed() == false)
                    {
                        yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
                        yield break;
                    }
                    else
                    {
                        yield return dialogSystem.TypeDialog(currentHazard.StartMessage(target));
                    }
                }
                else
                {
                    currentEntrySide.Add(currentHazard);
                    yield return dialogSystem.TypeDialog(currentHazard.StartMessage(target));
                }
            }
            else
            {
                Debug.Log($"Entry hazard is trying to be set on own side of field {effects}");
            }
        }

        if(currentMove.HpRecovered > 0)
        {
            if (source.pokemon.maxHitPoints - source.pokemon.currentHitPoints > 0 && source.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == false)
            {
                int hpPriorToHealing = source.pokemon.currentHitPoints;
                int hpHealed = Mathf.CeilToInt(source.pokemon.maxHitPoints * (currentMove.HpRecovered * source.pokemon.ability.PowerUpCertainMoves(source.pokemon, target.pokemon, currentMove,GetCurrentWeather)));
                WeatherEffectID weatherEffectID = (_currentWeather == null) ? WeatherEffectID.NA : _currentWeather.Id;
                hpHealed = Mathf.CeilToInt(hpHealed * HealthRecoveryModifiers(currentMove, weatherEffectID));
                hpHealed = Mathf.Clamp(hpHealed, 1, source.pokemon.maxHitPoints - source.pokemon.currentHitPoints);
                source.pokemon.UpdateHPRestored(hpHealed);
                yield return source.HUD.UpdateHP(hpPriorToHealing);
                source.pokemon.statusChanges.Enqueue($"{source.pokemon.currentName}'s hp was restored");
            }
            else
            {
                if(currentMove != rest)
                {
                    yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
                }
            }
        }

        yield return ShowStatusChanges(source.pokemon);
        yield return ShowStatusChanges(target.pokemon);
    }

    bool CheckIfMoveHits(MoveBase move, Pokemon source, Pokemon target)
    {
        if(target.currentHitPoints <= 0)
        {
            return false;
        }

        if (move.AlwaysHits == true)
        {
            return true;
        }

        if(source.ability.IncomingAndOutgoingAttacksAlwaysLand() == true|| target.ability.IncomingAndOutgoingAttacksAlwaysLand() == true)
        {
            return true;
        }

        float moveAccuracy = move.MoveAccuracy;

        moveAccuracy *= source.accuracy;

        float targetEvasion = target.evasion;

        if (source.ability.IgnoreStatIncreases(StatAttribute.Evasion) == true && targetEvasion > 1)
        {
            targetEvasion = 1;
        }
        moveAccuracy /= targetEvasion;

        return Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator ApplyEffectsOnEndTurn(BattleUnit sourceUnit,BattleUnit targetUnit)
    {
        if(sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield break;
        }

        int currentHP = sourceUnit.pokemon.currentHitPoints;

        if(sourceUnit.pokemon.ability.CuresStatusAtTurnEnd(sourceUnit.pokemon,GetCurrentWeather))
        {
            sourceUnit.OnAbilityActivation();
            yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.ability.OnAbilitityActivation(sourceUnit.pokemon)}");
        }

        allConditionsOnPokemon.Clear();

        if(sourceUnit.pokemon.status != null)
        {
            allConditionsOnPokemon.Add(sourceUnit.pokemon.status);
        }

        foreach (ConditionBase volatileStatus in sourceUnit.pokemon.volatileStatus)
        {
            allConditionsOnPokemon.Add(volatileStatus);
        }

        ////This copy is here because if it is iterating through it and removes an element while searching it shall break the for each loop
        List<ConditionBase> copyAllConditionsOnPokemon = new List<ConditionBase>(allConditionsOnPokemon);

        foreach (ConditionBase currentCondition in copyAllConditionsOnPokemon)
        {
            if (sourceUnit.pokemon.currentHitPoints <= 0)
            {
                break;
            }
            currentHP = sourceUnit.pokemon.currentHitPoints;

            if(currentCondition is LeechSeed)
            {
                LeechSeed leechSeed = ((LeechSeed)currentCondition);
                if (targetUnit.pokemon.currentHitPoints <= 0)
                {
                    continue;
                }

                sourceUnit.pokemon.OnEndTurn(currentCondition);//user will take the damage regardless if they have heal block or not

                if (targetUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == true)
                {
                    continue;
                }


                if (targetUnit.pokemon.maxHitPoints - targetUnit.pokemon.currentHitPoints > 0)
                {
                    int hpHealed = Mathf.Clamp(leechSeed.HealthStolen, 1, targetUnit.pokemon.maxHitPoints - targetUnit.pokemon.currentHitPoints);
                    if(targetUnit.pokemon.GetHoldItemEffects.Id == HoldItemID.BigRoot)
                    {
                        hpHealed = Mathf.Clamp(Mathf.FloorToInt(hpHealed * 0.3f), 1, targetUnit.pokemon.maxHitPoints - targetUnit.pokemon.currentHitPoints);
                    }
                    targetUnit.pokemon.UpdateHPRestored(hpHealed);
                    yield return ShowStatusChanges(sourceUnit.pokemon);
                    yield return sourceUnit.HUD.UpdateHP(currentHP);
                    yield return targetUnit.HUD.UpdateHP(targetUnit.pokemon.currentHitPoints - hpHealed);
                    continue;
                }
            }
            else if(sourceUnit.pokemon.OnEndTurn(currentCondition) == true)
            {
                yield return sourceUnit.StatusConditionAnimation(currentCondition.Id);
            }

            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield return sourceUnit.HUD.UpdateHP(currentHP);
        }

        //Ability

        if(sourceUnit.pokemon.currentHitPoints > 0)
        {
            int targetPreviousHP = targetUnit.pokemon.currentHitPoints;
            if(sourceUnit.pokemon.ability.ApplyEffectsToOpposingPokemonOnTurnEnd(targetUnit))
            {
                sourceUnit.OnAbilityActivation();
                yield return ShowStatusChanges(targetUnit.pokemon);
                yield return targetUnit.HUD.UpdateHP(targetPreviousHP);
            }
        }

        //Item effects
        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
            yield break;
        }
        else
        {

            currentHP = sourceUnit.pokemon.currentHitPoints;

            sourceUnit.pokemon.GetHoldItemEffects.OnTurnEnd(sourceUnit.pokemon);

            ConditionID condition = sourceUnit.pokemon.GetHoldItemEffects.InflictConditionAtTurnEnd();
            if(condition != ConditionID.NA)
            {
                sourceUnit.pokemon.SetStatusByItem(condition, sourceUnit.pokemon.GetHoldItemEffects.SpecializedMessage(sourceUnit.pokemon,targetUnit.pokemon));
            }

            if(currentHP != sourceUnit.pokemon.currentHitPoints)
            {
                yield return sourceUnit.PlayItemUsedAnimation();
            }

            yield return sourceUnit.HUD.UpdateHP(currentHP);
            yield return ShowStatusChanges(sourceUnit.pokemon);
        }

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
            yield break;
        }
        else
        {
            StatBoost abilityBoost = sourceUnit.pokemon.ability.AlterStatAtTurnEnd();
            List<StatBoost> statBoosts = new List<StatBoost>();
            if(abilityBoost != null)
            {
                statBoosts.Add(abilityBoost);
                sourceUnit.OnAbilityActivation();
            }
            yield return ApplyStatChanges(statBoosts, targetUnit, MoveTarget.Self,sourceUnit);
        }

        //Disabled
        if(sourceUnit.disabledDuration > 0)
        {
            sourceUnit.disabledDuration--;
            if(sourceUnit.disabledDuration == 0)
            {
                sourceUnit.pokemon.moves.FirstOrDefault(x => x.disabled == true).disabled = false;
                if (sourceUnit.isPlayerPokemon == true)
                {
                    yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName} is disabled no more");
                }
            }
        }

        //Shields
        if(sourceUnit.shields.Count > 0)
        {
            for (int i = sourceUnit.shields.Count - 1; i >= 0; i--)
            {
                sourceUnit.shields[i].TurnEndReduction();
                if(sourceUnit.shields[i].duration <= 0)
                {
                    yield return dialogSystem.TypeDialog(sourceUnit.shields[i].EndMessage(sourceUnit.isPlayerPokemon));
                    sourceUnit.shields.RemoveAt(i);
                }
            }
        }
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.statusChanges.Count > 0)
        {
            string message = pokemon.statusChanges.Dequeue();
            if(message != "")
            {
                yield return dialogSystem.TypeDialog(message);
            }
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails, BattleUnit battleUnit)
    {
        if (damageDetails.criticalHit > 1f)
        {
            yield return dialogSystem.TypeDialog("A Critical Hit!");
            if(battleUnit.pokemon.currentHitPoints > 0)
            {
                StatBoost statBoost = battleUnit.pokemon.ability.MaxOutStatUponCriticalHit(battleUnit.pokemon);
                if(statBoost != null)
                {
                    battleUnit.OnAbilityActivation();
                    List<StatBoost> abilityStatBoosts = new List<StatBoost>() { statBoost };
                    yield return ApplyStatChanges(abilityStatBoosts, battleUnit, MoveTarget.Foe);
                }
            }
        }

        if (damageDetails.typeEffectiveness == 0)
        {
            yield return dialogSystem.TypeDialog($"It doesnt effect {battleUnit.pokemon.currentName}");
        }
        else if (damageDetails.typeEffectiveness <= 0.5f)
        {
            yield return dialogSystem.TypeDialog($"It's not very effective");
        }
        else if (damageDetails.typeEffectiveness > 1f)
        {
            yield return dialogSystem.TypeDialog($"It's super effective!");
        }

        if(damageDetails.defendersAbilityActivation == true && battleUnit.pokemon.currentHitPoints > 0)
        {
            battleUnit.OnAbilityActivation();
            yield return ShowStatusChanges(battleUnit.pokemon);
        }
    }

    IEnumerator ApplyStatChanges(List<StatBoost> statBoosts,BattleUnit targetUnit, MoveTarget moveTarget,BattleUnit sourceUnit = null)
    {
        if (statBoosts == null || statBoosts.Count == 0)
        {
            yield break;
        }
        else
        {
            if(statBoosts.Count >= 2)
            {
                for (int i = statBoosts.Count - 1; i >= 0; i--)
                {
                    for (int j = statBoosts.Count - 1; j >= 0; j--)
                    {
                        if(i == j)
                        {
                            continue;
                        }

                        if(statBoosts[j] == null)
                        {
                            statBoosts.RemoveAt(j);
                        }

                        if(statBoosts[i].stat == statBoosts[j].stat)// && i != j)
                        {
                            statBoosts[i].boost += statBoosts[j].boost;
                            statBoosts.RemoveAt(j);
                        }
                    }
                }
            }
        }

        //Check to see if one is up and one is down
        List<StatBoost> positiveEffects = new List<StatBoost>();
        List<StatBoost> negativeEffects = new List<StatBoost>();

        foreach (StatBoost currentStat in statBoosts)
        {
            if(currentStat == null)
            {
                continue;
            }
            else if (currentStat.boost == 0)
            {
                continue;
            }
            else if (currentStat.boost > 0)
            {
                positiveEffects.Add(currentStat);
            }
            else
            {
                negativeEffects.Add(currentStat);
            }
        }

        if(positiveEffects.Count == 0 && negativeEffects.Count == 0)
        {
            yield break;
        }
        
        List<StatBoost> boostedStats = new List<StatBoost>();

        if (moveTarget == MoveTarget.Foe)
        {
            if(targetUnit.pokemon.ApplyStatModifier(positiveEffects) == true)
            {
                if(targetUnit.pokemon.ability.StatChangesHaveOppositeEffect() == true)
                {
                    yield return targetUnit.ShowStatChanges(positiveEffects, false);
                }
                else
                {
                    yield return targetUnit.ShowStatChanges(positiveEffects, true);
                }
            }
            yield return ShowStatusChanges(targetUnit.pokemon);

            bool abilityActivated = false;//This is for multiple stats prevented from being lowered so it doesnt stack the Queue

            List<StatBoost> negativeEffectsCopy = new List<StatBoost>(negativeEffects);
            foreach (StatBoost stat in negativeEffectsCopy)
            {
                if (stat.boost < 0)
                {
                    bool negated = targetUnit.pokemon.ability.PreventStatFromBeingLowered(stat.stat);
                    if (negated == true)
                    {
                        if (abilityActivated == false)
                        {
                            targetUnit.OnAbilityActivation();
                            targetUnit.pokemon.statusChanges.Enqueue(targetUnit.pokemon.ability.OnAbilitityActivation(targetUnit.pokemon));
                            abilityActivated = true;
                        }
                        negativeEffects.Remove(stat);
                    }
                }
            }

            for (int i = 0; i < negativeEffects.Count; i++)
            {
                StatBoost abilityBoost = targetUnit.pokemon.ability.BoostStatSharplyIfAnyStatLowered();
                if(abilityBoost != null)
                {
                    boostedStats.Add(abilityBoost);
                }
            }

            if(targetUnit.pokemon.ApplyStatModifier(negativeEffects) == true)
            {
                if (targetUnit.pokemon.ability.StatChangesHaveOppositeEffect() == true)
                {
                    yield return targetUnit.ShowStatChanges(negativeEffects, true);
                }
                else
                {
                    yield return targetUnit.ShowStatChanges(negativeEffects, false);
                }
            }
            yield return ShowStatusChanges(targetUnit.pokemon);
        }
        else if (moveTarget == MoveTarget.Self && sourceUnit != null)
        {
            if(sourceUnit.pokemon.ApplyStatModifier(positiveEffects) == true)
            {
                if (sourceUnit.pokemon.ability.StatChangesHaveOppositeEffect() == true)
                {
                    yield return sourceUnit.ShowStatChanges(positiveEffects, false);
                }
                else
                {
                    yield return sourceUnit.ShowStatChanges(positiveEffects, true);
                }
            }
            yield return ShowStatusChanges(sourceUnit.pokemon);

            if(sourceUnit.pokemon.ApplyStatModifier(negativeEffects) == true)
            {
                if (sourceUnit.pokemon.ability.StatChangesHaveOppositeEffect() == true)
                {
                    yield return sourceUnit.ShowStatChanges(negativeEffects, true);
                }
                else
                {
                    yield return sourceUnit.ShowStatChanges(negativeEffects, false);
                }
            }
            yield return ShowStatusChanges(sourceUnit.pokemon);
        }

        if(boostedStats.Count > 0)
        {
            targetUnit.OnAbilityActivation();
            yield return ApplyStatChanges(boostedStats, targetUnit, MoveTarget.Self,targetUnit);
        }
    }

    IEnumerator PokemonEntry(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        if(sourceUnit.pokemon.ability != null)
        {
            WeatherEffectID weatherEffect = sourceUnit.pokemon.ability.OnStartWeatherEffect();
            if (weatherEffect != WeatherEffectID.NA)
            {
                sourceUnit.OnAbilityActivation();
                yield return StartWeatherEffect(weatherEffect, sourceUnit, targetUnit, true);
            }

            StatBoost statBoost = sourceUnit.pokemon.ability.OnEntryLowerStat(targetUnit.pokemon.ability.Id);
            if (statBoost != null)
            {
                sourceUnit.OnAbilityActivation();
                List<StatBoost> abilityStatBoosts = new List<StatBoost>() { statBoost };
                yield return ApplyStatChanges(abilityStatBoosts, targetUnit, MoveTarget.Foe, sourceUnit);
            }

            statBoost = sourceUnit.pokemon.ability.OnEntryRaiseStat(targetUnit.pokemon);
            if (statBoost != null)
            {
                sourceUnit.OnAbilityActivation();
                List<StatBoost> abilityStatBoosts = new List<StatBoost>() { statBoost };
                yield return ApplyStatChanges(abilityStatBoosts, targetUnit, MoveTarget.Self, sourceUnit);
            }


            if (sourceUnit.pokemon.ability.NegatesWeatherEffects() == true)
            {
                if (_currentWeather != null || _currentWeather?.Id == WeatherEffectID.NA)
                {
                    sourceUnit.OnAbilityActivation();
                    _currentWeather = null;
                    yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s {sourceUnit.pokemon.ability.Name} cleared the battlefield");
                }
            }

            if(sourceUnit.pokemon.ability.ActivateAbilityUponEntry(sourceUnit.pokemon,targetUnit) == true)
            {
                sourceUnit.OnAbilityActivation();
                yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.ability.OnAbilitityActivation(sourceUnit.pokemon)}");
            }
        }

        yield return dialogSystem.TypeDialog(sourceUnit.pokemon.GetHoldItemEffects.EntryMessage(sourceUnit.pokemon));
    }

    void OnBattleOver(bool hasWon)
    {
        inBattle = false;
        dialogSystem.SetCurrentDialogBox();
        GameManager.instance.EndBattle(hasWon);
    }

    IEnumerator RearrangeAttackOrder()
    {
        if (currentTurnDetails.Count <= 1)
        {
            yield break;
        }

        int firstAttackPriority = currentTurnDetails[0].currentMove.moveBase.Priority;
        int secondAttackPriority = currentTurnDetails[1].currentMove.moveBase.Priority;

        firstAttackPriority += currentTurnDetails[0].attackingPokemon.pokemon.ability.AdjustSpeedPriorityOfMove(currentTurnDetails[0].attackingPokemon.pokemon, currentTurnDetails[0].currentMove.moveBase);
        secondAttackPriority += currentTurnDetails[1].attackingPokemon.pokemon.ability.AdjustSpeedPriorityOfMove(currentTurnDetails[1].attackingPokemon.pokemon, currentTurnDetails[1].currentMove.moveBase);

        if (currentTurnDetails[0].targetPokmeon.pokemon.ability.RemovesSpeedPriorityOfOpposingPokemon() == true)
        {
            firstAttackPriority = 0;
        }

        if (currentTurnDetails[1].targetPokmeon.pokemon.ability.RemovesSpeedPriorityOfOpposingPokemon() == true)
        {
            secondAttackPriority = 0;
        }

        if (firstAttackPriority == secondAttackPriority)
        {
            for (int i = 0; i < currentTurnDetails.Count; i++)
            {
                int adjustedPriority = currentTurnDetails[i].attackingPokemon.pokemon.GetHoldItemEffects.AdjustSpeedPriorityTurn();

                if (adjustedPriority != 0)
                {
                    if (adjustedPriority > 0)
                    {
                        yield return currentTurnDetails[i].attackingPokemon.PlayItemUsedAnimation();
                    }

                    TurnAttackDetails firstAttack = currentTurnDetails[i];
                    currentAttack = currentTurnDetails[1 - i];//Second Attack
                    currentTurnDetails.Clear();
                    if(adjustedPriority > 0)
                    {
                        currentTurnDetails.Add(firstAttack);
                        currentTurnDetails.Add(currentAttack);
                    }
                    else
                    {
                        currentTurnDetails.Add(currentAttack);
                        currentTurnDetails.Add(firstAttack);
                    }

                    break;
                }
                else
                {
                    if (currentTurnDetails[i].attackingPokemon.pokemon.speed > currentTurnDetails[1-i].attackingPokemon.pokemon.speed)
                    {
                        TurnAttackDetails firstAttack = currentTurnDetails[i];
                        currentAttack = currentTurnDetails[1 - i];//Second Attack
                        currentTurnDetails.Clear();
                        currentTurnDetails.Add(firstAttack);
                        currentTurnDetails.Add(currentAttack);
                        break;
                    }
                    else if (currentTurnDetails[i].attackingPokemon.pokemon.speed == currentTurnDetails[1-i].attackingPokemon.pokemon.speed)
                    {
                        bool coinFlip = (Random.value > 0.5f);
                        if (coinFlip == true)
                        {
                            TurnAttackDetails firstAttack = currentTurnDetails[i];
                            currentAttack = currentTurnDetails[1 - i];//Second Attack
                            currentTurnDetails.Clear();
                            currentTurnDetails.Add(firstAttack);
                            currentTurnDetails.Add(currentAttack);
                        }
                        else
                        {
                            TurnAttackDetails firstAttack = currentTurnDetails[i];
                            currentAttack = currentTurnDetails[1 - i];//Second Attack
                            currentTurnDetails.Clear();
                            currentTurnDetails.Add(currentAttack);
                            currentTurnDetails.Add(firstAttack);
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < currentTurnDetails.Count; i++)
            {
                if (currentTurnDetails[i].currentMove.moveBase.Priority > currentTurnDetails[1-i].currentMove.moveBase.Priority)
                {
                    TurnAttackDetails firstAttack = currentTurnDetails[i];
                    currentAttack = currentTurnDetails[1 - i];//Second Attack
                    currentTurnDetails.Clear();
                    currentTurnDetails.Add(firstAttack);
                    currentTurnDetails.Add(currentAttack);
                    break;
                }
            }
        }
    }

    #endregion

    #region PokemonFaint/Switching

    IEnumerator PokemonHasFainted(BattleUnit targetBattleUnit)
    {
        if(targetBattleUnit.pokemonHasFainted == true)
        {
            yield break;
        }

        if (targetBattleUnit.pokemon.currentHitPoints <= 0)
        {
            yield return dialogSystem.TypeDialog($"{targetBattleUnit.pokemon.currentName} has fainted");
            targetBattleUnit.PlayFaintAnimation();

            if (targetBattleUnit.isPlayerPokemon == false)
            {
                int expYield = targetBattleUnit.pokemon.pokemonBase.RewardedExperienceYield;
                int level = targetBattleUnit.pokemon.currentLevel;
                float trainerBonus = (_isTrainerBattle == true) ? 1.5f : 1;
                int pokemonSharingExp = 1;

                pokemonSharingExp = 0;

                //Pokemon in party holding EXP share
                List<Pokemon> playerParty = _playerController.pokemonParty.CurrentPokemonList();
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetHoldItemEffects.ExperienceShared() == true && playerParty[i].currentHitPoints > 0)
                    {
                        if (targetBattleUnit.GetListOfPokemonBattledAgainst.Contains(playerParty[i]) == false)
                        {
                            targetBattleUnit.AddPokemonToBattleList(playerParty[i]);
                        }
                    }
                }

                List<Pokemon> copyPokemonBattledAgainst = new List<Pokemon>(targetBattleUnit.GetListOfPokemonBattledAgainst);

                foreach (Pokemon pokemon in copyPokemonBattledAgainst)
                {
                    if (pokemon.currentLevel >= 100 || pokemon.currentHitPoints <= 0)
                    {
                        targetBattleUnit.GetListOfPokemonBattledAgainst.Remove(pokemon);
                        continue;
                    }
                    pokemonSharingExp++;
                }

                int expGained = Mathf.FloorToInt(expYield * level * trainerBonus / pokemonSharingExp) / 7;
                int expGainedAfteritemAffects;

                foreach (Pokemon pokemon in targetBattleUnit.GetListOfPokemonBattledAgainst)
                {
                    expGainedAfteritemAffects = Mathf.FloorToInt(pokemon.GetHoldItemEffects.ExperienceModifier() * expGained);
                    if (pokemon == playerBattleUnit.pokemon)
                    {
                        
                        yield return GainExperience(playerBattleUnit, expGainedAfteritemAffects);
                    }
                    else
                    {
                        yield return GainExperience(pokemon, expGainedAfteritemAffects);
                    }
                    pokemon.GainEffortValue(targetBattleUnit.pokemon.pokemonBase.rewardedEfforValue);
                }
            }

            yield return new WaitForSeconds(2f);
            yield return CheckForBattleOver(targetBattleUnit);
        }
    }

    IEnumerator CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.isPlayerPokemon)
        {
            Pokemon nextPokemon = _playerController.pokemonParty.GetFirstHealthyPokemon();
            if (nextPokemon != null)
            {
                faintedUnit.SendOutPokemonOnTurnEnd = true;
            }
            else
            {
                if(enemyBattleUnit.pokemon.currentHitPoints <= 0)
                {
                    yield return dialogSystem.TypeDialog($"{enemyBattleUnit.pokemon.currentName} has fainted");
                    enemyBattleUnit.PlayFaintAnimation();
                }

                if(_isTrainerBattle == true)
                {
                    yield return TrainerBattleOver(false);
                }
                OnBattleOver(false);
            }
        }
        else
        {
            if(_isTrainerBattle == true)
            {
                Pokemon nextPokemon = _trainerController.pokemonParty.GetFirstHealthyPokemon();
                if (nextPokemon != null)
                {
                    faintedUnit.SendOutPokemonOnTurnEnd = true;
                }
                else
                {
                    if (playerBattleUnit.pokemon.currentHitPoints <= 0)
                    {
                        yield return dialogSystem.TypeDialog($"{playerBattleUnit.pokemon.currentName} has fainted");
                        playerBattleUnit.PlayFaintAnimation();
                    }
                    yield return TrainerBattleOver(true);
                    OnBattleOver(true);
                }
            }
            else
            {
                OnBattleOver(true);
            }
        }
        yield return null;
    }


    /// <summary>
    /// Used in party selection
    /// </summary>
    public Pokemon GetCurrentPokemonInBattle
    {
        get { return playerBattleUnit.pokemon; }
    }

    public void PlayerContinueAfterPartyShiftSelection()
    {
        dialogSystem.SetCurrentDialogBox(dialogBox);
        waitUntilUserFinished = true;
        _playerPokemonShift = false;
    }

    public void PlayerSwitchPokemon(Pokemon newPokemon)
    {
        dialogSystem.SetCurrentDialogBox(dialogBox);
        StartCoroutine(SwitchPokemonIEnumerator(playerBattleUnit,newPokemon));
        EnableActionSelector(false);
    }

    IEnumerator SwitchPokemonIEnumerator(BattleUnit battleUnit,Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if (battleUnit.pokemon.currentHitPoints > 0)
        {
            currentPokemonFainted = false;
            yield return dialogSystem.TypeDialog($"Come Back {battleUnit.pokemon.currentName}!");
            battleUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        if (battleUnit.isPlayerPokemon)
        {
            _playerController.pokemonParty.SwitchPokemonPositions(battleUnit.pokemon, newPokemon);
            enemyBattleUnit.AddPokemonToBattleList(newPokemon);
            enemyBattleUnit.pokemon.CureVolatileStatus(ConditionID.Infatuation);
        }

        enemyBattleUnit.pokemon.CureVolatileStatus(ConditionID.Infatuation);
        playerBattleUnit.pokemon.CureVolatileStatus(ConditionID.Infatuation);

        battleUnit.SetupAndSendOut(newPokemon);

        if(battleUnit.isPlayerPokemon == true)
        {
            attackSelectionEventSelector.SetMovesList(battleUnit, battleUnit.pokemon.moves);
            yield return dialogSystem.TypeDialog($"Go {battleUnit.pokemon.currentName}!");
        }
        else
        {
            yield return dialogSystem.TypeDialog($"{_trainerController.TrainerName} sent out {battleUnit.pokemon.currentName}");
            battleUnit.AddPokemonToBattleList(playerBattleUnit.pokemon);
        }

        yield return ApplyEntryHazardOnSentOut(battleUnit);

        if(_playerPokemonShift == true)
        {
            yield return new WaitForSeconds(0.5f);
            PlayerContinueAfterPartyShiftSelection();
            yield break;
        }

        if (battleUnit.isPlayerPokemon)
        {
            yield return PokemonEntry(playerBattleUnit, enemyBattleUnit);

            if (enemyBattleUnit.pokemon.currentHitPoints <= 0)
            {
                yield return SwitchPokemonIEnumerator(enemyBattleUnit, _trainerController.pokemonParty.GetFirstHealthyPokemon());
            }

            if (currentPokemonFainted == true)
            {
                PlayerActions();
            }
            else
            {
                StartCoroutine(EnemyMove());
            }
        }
        else
        {
            yield return PokemonEntry(enemyBattleUnit, playerBattleUnit);
        }
    }

    #endregion

    #region Entry Hazards

    IEnumerator ApplyEntryHazardOnSentOut(BattleUnit target)
    {
        if (target.pokemon.currentHitPoints <= 0 || target.pokemon.GetHoldItemEffects.PreventsEffectsOfEntryHazards() == true)
        {
            yield break;
        }

        List<EntryHazardBase> currentEntrySide = (target.isPlayerPokemon) ? _playerSideEntryHazards : _enemySideEntryHazards;

        foreach (EntryHazardBase entryHazard in currentEntrySide)
        {
            int currentHP = target.pokemon.currentHitPoints;

            List<StatBoost> entryHazardStatBoosts = new List<StatBoost>() { entryHazard.OnEntryLowerStat(target.pokemon) };
            yield return ApplyStatChanges(entryHazardStatBoosts, target, MoveTarget.Foe);
            entryHazard.OnEntry(target.pokemon);

            yield return ShowStatusChanges(target.pokemon);

            if (currentHP != target.pokemon.currentHitPoints)
            {
                target.PlayHitAnimation();
            }
            yield return target.HUD.UpdateHP(currentHP);

            if (target.pokemon.currentHitPoints <= 0)
            {
                yield return PokemonHasFainted(target);
                yield break;
            }
        }
    }

    #endregion

    #region Weather Effects

    IEnumerator ShowWeatherEffect(WeatherEffectBase weather)
    {
        yield return dialogSystem.TypeDialog(weather.OnEndTurn());
    }

    IEnumerator StartWeatherEffect(WeatherEffectID weatherID,BattleUnit sourceUnit,BattleUnit targetUnit,bool wasAbility = false)
    {
        if (_currentWeather != null)
        {
            if (_currentWeather.Id == weatherID)
            {
                if(wasAbility == false)
                {
                    yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
                }
                yield break;
            }
        }

        if(sourceUnit.pokemon.ability.NegatesWeatherEffects() == true)
        {
            yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
            yield break;
        }

        if(targetUnit.pokemon.ability.NegatesWeatherEffects() == true)
        {
            targetUnit.OnAbilityActivation();

            if(wasAbility == true)
            {
                yield return dialogSystem.TypeDialog($"{sourceUnit.pokemon.currentName}'s {sourceUnit.pokemon.ability.Name} was negated by {targetUnit.pokemon.currentName}'s {targetUnit.pokemon.ability.Name}");
            }
            else
            {
                yield return dialogSystem.TypeDialog($"{BUT_IT_FAILED}");
            }
            yield break;
        }

        _currentWeather = WeatherEffectDB.WeatherEffects[weatherID];
        yield return dialogSystem.TypeDialog(_currentWeather.StartMessage());
        _currentWeather.duration += sourceUnit.pokemon.GetHoldItemEffects.IncreasedWeatherEffectDuration(weatherID);
    }

    public static void RemoveWeatherEffect()
    {
        _currentWeather = null;
    }

    IEnumerator ApplyWeatherEffectsOnEndTurn(WeatherEffectBase weather,BattleUnit sourceUnit)
    {
        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield break;
        }

        int currentHP = sourceUnit.pokemon.currentHitPoints;

        weather.OnEndTurnDamage(sourceUnit.pokemon);
        yield return ShowStatusChanges(sourceUnit.pokemon);

        if(currentHP != sourceUnit.pokemon.currentHitPoints)
        {
            sourceUnit.PlayHitAnimation();
            yield return sourceUnit.HUD.UpdateHP(currentHP);
        }

        currentHP = sourceUnit.pokemon.currentHitPoints;
        if (sourceUnit.pokemon.ability.AffectsHpByXEachTurnWithWeather(sourceUnit.pokemon, weather.Id) == true)
        {
            sourceUnit.OnAbilityActivation();
            yield return ShowStatusChanges(sourceUnit.pokemon);
            if (currentHP < sourceUnit.pokemon.currentHitPoints)
            {
                yield return sourceUnit.HUD.UpdateHP(sourceUnit.pokemon.currentHitPoints - currentHP);
            }
            else
            {
                yield return sourceUnit.HUD.UpdateHP(currentHP);
            }
        }


        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
        }
    }

    public static WeatherEffectID GetCurrentWeather
    {
        get
        {
            if (_currentWeather != null)
            {
                return _currentWeather.Id;
            }
            return WeatherEffectID.NA;
        }
    }

    #endregion

    #region Items/Catching Pokemon

    public void UsePokeballFromInventory(PokeballItem pokeball)
    {
        dialogSystem.SetCurrentDialogBox(dialogBox);
        EnableActionSelector(false);
        StartCoroutine(PlayerThrewPokeball(enemyBattleUnit,pokeball));
    }

    IEnumerator PlayerThrewPokeball(BattleUnit targetUnit,PokeballItem currentPokeball)
    {
        if(_isTrainerBattle == true)
        {
            EnableActionSelector(false);
            yield return dialogSystem.TypeDialog($"You cant capture Trainers Pokemon", true);
            PlayerActions();
            yield break;
        }
        
        inGameItem.SetActive(true);
        InBattleItem currentBall = inGameItem.GetComponent<InBattleItem>();
        currentBall.SetInBattleItem(currentPokeball);
        Vector3 ballHeightJump = new Vector3(0, 75, 0);

        yield return dialogSystem.TypeDialog($"{_playerController.TrainerName} used {currentBall.GetItemName}");
        //SetPokeball to its closed art
        currentBall.SetItemArt();

        //Pokeball gets thrown to desired position
        yield return currentBall.FollowTheRoute();

        //Bounce up slightly
        yield return currentBall.MoveToPosition(currentBall.transform.localPosition + ballHeightJump, 0.25f);

        //Animate PokeBall opening
        yield return currentBall.PokeballAnimation(true);

        //animate enemy sprite going into pokeball
        yield return targetUnit.CaptureAnimation(currentBall.transform.localPosition);

        //Pokeball Closes
        yield return currentBall.PokeballAnimation(false);

        //pokeball drops to location
        yield return currentBall.MoveToPosition(currentBall.transform.localPosition + new Vector3(0, -135, 0), 0.5f);
        yield return new WaitForSeconds(1f);

        // start capture mechanics of it rocking
        int shakeCount = CatchingMechanics.CatchRate(targetUnit.pokemon, currentBall.CurrentPokeball,battleDuration);

        for (int i = 0; i < Mathf.Min(shakeCount,3); i++)
        {
            bool shakeRight = Random.value > 0.5f;
            yield return currentBall.ShakePokeball(shakeRight);
            yield return new WaitForSeconds(0.75f);
        }

        //pokemon captured or broken free
        if(shakeCount == 4)
        {
            //pokemon is caught
            yield return dialogSystem.TypeDialog($"{targetUnit.pokemon.currentName} was Caught!",true);
            yield return currentBall.FadeItem(false);
            GameManager.instance.CapturedNewPokemon(enemyBattleUnit.pokemon, currentBall.CurrentPokeball);
            OnBattleOver(true);
        }
        else
        {
            //pokemon is set free
            yield return currentBall.PokeballAnimation(true);
            yield return targetUnit.EscapeCaptureAnimation(currentBall.transform.localPosition);
            inGameItem.transform.localPosition = inGameItemoffScreenPos;

            if(shakeCount == 0)
            {
                yield return dialogSystem.TypeDialog($"Oh no! The Pokémon broke free!",true);
            }
            else if(shakeCount == 1)
            {
                yield return dialogSystem.TypeDialog($"Aww! It appeared to be caught!",true);
            }
            else if (shakeCount == 2)
            {
                yield return dialogSystem.TypeDialog($"Aargh! Almost had it!",true);
            }
            else if (shakeCount == 3)
            {
                yield return dialogSystem.TypeDialog($"Gah! It was so close, too!",true);
            }

            playerBattleUnit.pokemon.ability.FetchPokeBallFirstFailedThrow(currentPokeball, playerBattleUnit.pokemon);

            StartCoroutine(EnemyMove());
        }
    }

    public void PlayerUsedItemWhileInBattle()
    {
        dialogSystem.SetCurrentDialogBox(dialogBox);
        playerBattleUnit.HUD.UpdateHud();
        StartCoroutine(EnemyMove());
        EnableActionSelector(false);
    }

    #endregion

    #region Experience and Leveling Mechanics

    IEnumerator GainExperience(BattleUnit targetUnit,int expGained)
    {
        int expBeforeAnim = targetUnit.pokemon.currentExp;
        targetUnit.pokemon.currentExp += expGained;
        yield return dialogSystem.TypeDialog($"{targetUnit.pokemon.currentName} gained {expGained} exp", true);
        yield return targetUnit.HUD.GainExpAnimation(expGained, expBeforeAnim);

        //Level up Here

        StandardStats StatsBeforeLevel = targetUnit.pokemon.GetStandardStats();

        while (targetUnit.pokemon.LevelUpCheck() == true)
        {
            expGained -= targetUnit.pokemon.pokemonBase.GetExpForLevel(targetUnit.pokemon.currentLevel) - expBeforeAnim;
            expBeforeAnim = targetUnit.pokemon.pokemonBase.GetExpForLevel(targetUnit.pokemon.currentLevel);
            yield return dialogSystem.TypeDialog($"{targetUnit.pokemon.currentName} grew to level {targetUnit.pokemon.currentLevel}!", true);
            targetUnit.HUD.SetLevel();
            //Play level up animation

            yield return levelUpUI.DisplayLevelUp(StatsBeforeLevel, targetUnit.pokemon.GetStandardStats());
            targetUnit.HUD.UpdateHPWithoutAnimation();

            List<LearnableMove> newMove = targetUnit.pokemon.GetLearnableMoveAtCurrentLevel();

            if (newMove.Count > 0)
            {
                yield return LearnNewMove(targetUnit.pokemon,newMove);
                attackSelectionEventSelector.SetMovesList(targetUnit, targetUnit.pokemon.moves);
            }

            if (targetUnit.pokemon.currentLevel >= 100)
            {
                break;
            }
            yield return targetUnit.HUD.GainExpAnimation(expGained, targetUnit.pokemon.pokemonBase.GetExpForLevel(targetUnit.pokemon.currentLevel));

            StatsBeforeLevel = targetUnit.pokemon.GetStandardStats();
        }
    }

    IEnumerator GainExperience(Pokemon pokemon, int expGained)
    {
        int expBeforeAnim = pokemon.currentExp;
        pokemon.currentExp += expGained;
        yield return dialogSystem.TypeDialog($"{pokemon.currentName} gained {expGained} exp", true);

        StandardStats StatsBeforeLevel = pokemon.GetStandardStats();

        while (pokemon.LevelUpCheck() == true)
        {
            expGained -= pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel) - expBeforeAnim;
            expBeforeAnim = pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel);
            yield return dialogSystem.TypeDialog($"{pokemon.currentName} grew to level {pokemon.currentLevel}!", true);

            yield return levelUpUI.DisplayLevelUp(StatsBeforeLevel, pokemon.GetStandardStats(),pokemon);

            List<LearnableMove> newMove = pokemon.GetLearnableMoveAtCurrentLevel();

            if (newMove.Count > 0)
            {
                yield return LearnNewMove(pokemon, newMove);
            }

            if (pokemon.currentLevel >= 100)
            {
                break;
            }
            StatsBeforeLevel = pokemon.GetStandardStats();
        }
    }

    IEnumerator LearnNewMove(Pokemon currentPokemon,List<LearnableMove> newMoves)
    {
        foreach (LearnableMove learnableMove in newMoves)
        {
            if (currentPokemon.moves.Count < PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                currentPokemon.LearnMove(learnableMove.moveBase);
                yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
            }
            else
            {

                bool playerSelection = true;

                while (playerSelection == true)
                {
                    yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} is trying to learn {learnableMove.moveBase.MoveName}.", true);
                    yield return dialogSystem.TypeDialog($"But {currentPokemon.currentName} can't learn more than four moves.", true);
                    yield return dialogSystem.TypeDialog($"Delete a move to make room for {learnableMove.moveBase.MoveName}?");

                    yield return dialogSystem.SetChoiceBox(() =>
                    {
                        playerSelection = true;
                    }
                    , () =>
                    {
                        playerSelection = false;
                    });

                    if(playerSelection == true)
                    {
                        yield return learnNewMoveUI.OpenToLearnNewMove(currentPokemon, learnableMove.moveBase);

                        if(learnNewMoveUI.PlayerDoesNotWantToLearnMove == false)
                        {
                            yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} forgot how to use {learnNewMoveUI.previousMoveName}", true);
                            yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
                            playerSelection = false;
                            continue;
                        }
                    }

                    playerSelection = !learnNewMoveUI.PlayerDoesNotWantToLearnMove;

                    if (playerSelection == false)
                    {
                        yield return dialogSystem.TypeDialog($"Stop Learning {learnableMove.moveBase.MoveName}?");
                        yield return dialogSystem.SetChoiceBox(() =>
                        {
                            playerSelection = false;
                        }
                        , () =>
                        {
                            playerSelection = true;
                        });

                        if (playerSelection == false)
                        {
                            yield return dialogSystem.TypeDialog($"{currentPokemon.currentName} did not learn {learnableMove.moveBase.MoveName}");
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Specialized Attacks

    public static bool CheckIfTargetCanBeInflatuated(Pokemon sourcePokemon, Pokemon targetPokemon)
    {
        if(sourcePokemon.gender == targetPokemon.gender)
        {
            return false;
        }
        else if(targetPokemon.gender == Gender.NA)
        {
            return false;
        }
        else if(targetPokemon.HasCurrentVolatileStatus(ConditionID.Infatuation) == true)
        {
            return false;
        }
        return true;
    }

    int VariableNumberOfStrikes()
    {
        int randomNumber = Random.Range(0, 8);

        switch (randomNumber)
        {
            case int n when (n <= 2):
                return 2;
            case int n when (n > 2 && n <= 5):
                return 3;
            case int n when (n == 6):
                return 4;
            default:
                return 5;
        }
    }

    bool CheckIfMoveHasSpecializedConditionAndSuccessful(BattleUnit sourceUnit, BattleUnit targetUnit, MoveBase moveBase)
    {
        if(moveBase == noRetreat)
        {
            return (NoRetreatSuccessful(sourceUnit));
        }
        else if (moveBase == encore)
        {
            return EncoreSuccessful(targetUnit);
        }
        else if(moveBase == dreamEater)
        {
            return (DreamEaterSuccessful(targetUnit));
        }
        else if(moveBase == purify)
        {
            return (PurifySuccessful(sourceUnit));
        }
        else if(moveBase == rest)
        {
            return (RestSuccessful(sourceUnit));
        }
        else if(moveBase == disabled)
        {
            return (DisableSucessful(targetUnit));
        }
        else if (moveBase == reflect || moveBase == lightScreen || moveBase == auroraVeil)
        {
            return (ShieldSucessful(sourceUnit,targetUnit, moveBase));
        }

        return true;
    }

    bool NoRetreatSuccessful(BattleUnit sourceUnit)
    {
        if(sourceUnit.cantEscapeGivenToSelf == true)
        {
            return false;
        }
        else
        {
            if(sourceUnit.pokemon.volatileStatus.Exists(x => x.Id == ConditionID.CantEscape))
            {
                return true;
            }
        }
        sourceUnit.cantEscapeGivenToSelf = true;
        return true;
    }

    bool EncoreSuccessful(BattleUnit targetUnit)
    {
        if(targetUnit.lastMoveUsed == null)
        {
            return false;
        }
        else if(targetUnit.lastMoveUsed.pP > 0)
        {
            return true;
        }
        else if(targetUnit.lastMoveUsed.moveBase == struggle)
        {
            return false;
        }
        else if (targetUnit.lastMoveUsed.moveBase == encore)
        {
            return false;
        }
        return false;
    }

    bool DreamEaterSuccessful(BattleUnit targetUnit)
    {
        if (targetUnit.pokemon.status?.Id == ConditionID.Sleep)
        {
            return true;
        }
        return false;
    }

    bool PurifySuccessful(BattleUnit sourceUnit)
    {
        if (sourceUnit.pokemon.status != null)
        {
            if(sourceUnit.pokemon.status.Id != ConditionID.NA)
            {
                sourceUnit.pokemon.CureStatus();
                sourceUnit.pokemon.statusChanges.Enqueue($"{sourceUnit.pokemon.currentName} cured its status condition");

                return true;
            }
        }
        return false;
    }

    bool RestSuccessful(BattleUnit sourceUnit)
    {
        if(sourceUnit.pokemon.maxHitPoints - sourceUnit.pokemon.currentHitPoints > 0 || sourceUnit.pokemon.status != null)
        {
            if(sourceUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == false)
            {
                sourceUnit.pokemon.CureStatus();
                sourceUnit.pokemon.SetStatus(ConditionID.Sleep,false);
                sourceUnit.pokemon.statusChanges.Clear();
                sourceUnit.pokemon.statusChanges.Enqueue($"{sourceUnit.pokemon.currentName} went to sleep to become healthy");
                sourceUnit.pokemon.status.StatusTime = 2;
                return true;
            }
        }
        return false;
    }

    bool DisableSucessful(BattleUnit targetUnit)
    {
        if(targetUnit.lastMoveUsed == null)
        {
            return false;
        }

        if(targetUnit.NoMovesAvailable() == true)
        {
            return false;
        }

        foreach (Move move in targetUnit.pokemon.moves)
        {
            if (move.disabled == true)
            {
                return false;
            }
        }

        targetUnit.lastMoveUsed.disabled = true;
        targetUnit.disabledDuration = 5;
        targetUnit.pokemon.statusChanges.Enqueue($"{targetUnit.pokemon.currentName}'s {targetUnit.lastMoveUsed.moveBase.MoveName} was disabled!");
        return true;
    }

    float HealthRecoveryModifiers(MoveBase moveBase,WeatherEffectID iD)
    {
        if(moveBase == moonlight||moveBase == synthesis|| moveBase == morningSun)
        {
            if(iD == WeatherEffectID.Sunshine)
            {
                return 1.33f;
            }
            else if(iD == WeatherEffectID.Rain|| iD == WeatherEffectID.Sandstorm||iD == WeatherEffectID.Hail)
            {
                return 0.5f;
            }
        }

        if (moveBase == shoreUp)
        {
            if (iD == WeatherEffectID.Sandstorm)
            {
                return 1.33f;
            }
        }

        return 1f;
    }

    bool ShieldSucessful(BattleUnit sourceUnit,BattleUnit targetUnit,MoveBase shieldType)
    {
        ShieldType shield;

        switch (shieldType)
        {
            case MoveBase n when (n == lightScreen):
                shield = ShieldType.LightScreen;
                break;
            case MoveBase n when (n == reflect):
                shield = ShieldType.Reflect;
                break;
            default://AuroraVeil
                shield = ShieldType.AuroraVeil;
                break;
        }

        if (sourceUnit.shields.Exists(x => x.GetShieldType == shield) == true)
        {
            return false;
        }

        if(shield == ShieldType.AuroraVeil)
        {
            if(GetCurrentWeather != WeatherEffectID.Hail)
            {
                return false;
            }

            if(sourceUnit.pokemon.ability.NegatesWeatherEffects() == true || targetUnit.pokemon.ability.NegatesWeatherEffects() == true)
            {
                return false;
            }
        }

        ShieldBase shieldBase;

        switch (shield)
        {
            case ShieldType.LightScreen:
                shieldBase = new LightScreen(sourceUnit.pokemon.GetCurrentItem);
                break;
            case ShieldType.Reflect:
                shieldBase = new Reflect(sourceUnit.pokemon.GetCurrentItem);
                break;
            default://AuroraVeil
                shieldBase = new AuroraVeil(sourceUnit.pokemon.GetCurrentItem);
                break;
        }

        sourceUnit.shields.Add(shieldBase);
        sourceUnit.pokemon.statusChanges.Clear();
        sourceUnit.pokemon.statusChanges.Enqueue(shieldBase.StartMessage(sourceUnit.isPlayerPokemon));
        return true;
    }

    #endregion
}
