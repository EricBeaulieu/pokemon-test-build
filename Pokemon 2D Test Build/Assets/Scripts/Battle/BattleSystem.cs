using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] Image backgroundArt;
    [SerializeField] BattleUnit playerBattleUnit;
    [SerializeField] BattleUnit enemyBattleUnit;

    public event Action<bool> OnBattleOver;
    public event Action<bool> OpenPokemonParty;

    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] ActionSelectionEventSelector actionSelectionEventSelector;
    [SerializeField] AttackSelectionEventSelector attackSelectionEventSelector;

    List<TurnAttackDetails> _currentTurnDetails;

    PlayerController _playerController;
    TrainerController _trainerController;
    Pokemon _wildPokemon;
    bool _isTrainerBattle;
    bool _playerPokemonShift;

    static WeatherEffectBase _currentWeather;

    List<EntryHazardBase> _playerSideEntryHazards = new List<EntryHazardBase>();
    List<EntryHazardBase> _enemySideEntryHazards = new List<EntryHazardBase>();

    [SerializeField] GameObject inGameItem;
    Vector2 inGameItemoffScreenPos;
    public event Action<Pokemon,PokeballItem> OnPokemonCaptured;

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

    public void HandleAwake()
    {
        gameObject.SetActive(false);
        inGameItemoffScreenPos = inGameItem.transform.localPosition;
        levelUpUI.HandleStart();

        if(playerBattleUnit == null)
        {
            Debug.LogWarning($"playerBattleUnit has not been set");
        }

        if (enemyBattleUnit == null)
        {
            Debug.LogWarning($"enemyBattleUnit has not been set");
        }

        if (dialogBox == null)
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
    }

    public void HandleUpdate()
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

        if(dialogBox.WaitingOnUserInput ==true)
        {
            if (Input.GetButtonDown("Fire1") | Input.GetButtonDown("Fire2"))
            {
                dialogBox.WaitingOnUserInput = false;
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
        _currentTurnDetails = new List<TurnAttackDetails>();
    }

    public void StartBattle(PlayerController player, TrainerController trainer)
    {
        _playerController = player;
        
        _trainerController = trainer;
        _isTrainerBattle = true;

        StartCoroutine(SetupBattle());
        _currentTurnDetails = new List<TurnAttackDetails>();
    }

    /// <summary>
    /// Goes through animations and sets up both the current pokemon and the enemy pokemon, 
    /// all available attacks along with their PP and names
    /// </summary>
    IEnumerator SetupBattle()
    {
        _playerSideEntryHazards = new List<EntryHazardBase>();
        _enemySideEntryHazards = new List<EntryHazardBase>();
        Pokemon playerPokemon = _playerController.pokemonParty.GetFirstHealthyPokemon();
        if (_isTrainerBattle == true)
        {
            Pokemon enemyPokemon = _trainerController.pokemonParty.GetFirstHealthyPokemon();
            playerBattleUnit.SetDataBattleStart(playerPokemon, _playerController.BackBattleSprite[0]);
            enemyBattleUnit.SetDataBattleStart(enemyPokemon, _trainerController.FrontBattleSprite[0]);

            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} wants to battle!",true);
            yield return new WaitUntil(() => playerBattleUnit.startingAnimationsActive == false && enemyBattleUnit.startingAnimationsActive == false);
            yield return new WaitForSeconds(0.5f);
            dialogBox.AfterDialogWait();

            yield return enemyBattleUnit.PlayTrainerExitAnimation(true);

            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} sent out {enemyPokemon.currentName}");

            enemyBattleUnit.SendOut();

            yield return new WaitForSeconds(0.5f);

            yield return dialogBox.TypeDialog($"Go {playerPokemon.currentName}");

            yield return playerBattleUnit.PlayTrainerExitAnimation(true);
            playerBattleUnit.SendOut();

            yield return new WaitForSeconds(1f);
        }
        else //WildPokemon
        {
            playerBattleUnit.SetDataBattleStart(playerPokemon, _playerController.BackBattleSprite[0]);
            enemyBattleUnit.SetDataBattleStart(_wildPokemon,null);

            yield return dialogBox.TypeDialog($"A wild {enemyBattleUnit.pokemon.currentName} has appeared!");
            yield return enemyBattleUnit.PlayTrainerExitAnimation(false);

            yield return dialogBox.TypeDialog($"Go {playerPokemon.currentName}");
            yield return playerBattleUnit.PlayTrainerExitAnimation(true);
            playerBattleUnit.SendOut();

            yield return new WaitForSeconds(1f);
        }

        _playerController.pokemonParty.SetOriginalPositions();
        _playerController.pokemonParty.CleanUpPartyOrderOnStart(playerBattleUnit.pokemon);
        _escapeAttempts = 0;
        battleDuration = 0;
        enemyBattleUnit.AddPokemonToBattleList(playerBattleUnit.pokemon);
        dialogBox.BattleStartSetup();
        attackSelectionEventSelector.SetMovesList(playerBattleUnit,playerBattleUnit.pokemon.moves,this);

        _currentWeather = null;

        yield return ActivatePokemonAbilityUponEntry(playerBattleUnit,enemyBattleUnit);
        yield return ActivatePokemonAbilityUponEntry(enemyBattleUnit,playerBattleUnit);

        SetupPlayerActions();
        PlayerActions();
    }

    #endregion

    #region Player Actions

    void SetupPlayerActions()
    {
        actionSelectionEventSelector.SetUp();
        actionSelectionEventSelector.ReturnFightButton().onClick.AddListener(delegate { PlayerActionFight(); });
        actionSelectionEventSelector.ReturnPokemonButton().onClick.AddListener(delegate { PlayerActionPokemon(); });
        actionSelectionEventSelector.ReturnBagButton().onClick.AddListener(delegate { PlayerActionBag(); });
        actionSelectionEventSelector.ReturnRunButton().onClick.AddListener(delegate { PlayerActionRun(); });
    }

    /// <summary>
    /// sets up the players action box, with the cursor/event system selected on the first box
    /// Sets up the listeners for all the current selections
    /// </summary>
    void PlayerActions()
    {
        dialogBox.SetDialogText($"What will {playerBattleUnit.pokemon.currentName} do?");
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
            OpenPokemonParty(false);
        }
        else
        {
            PlayerCantEscapeActiveWhenTryingToSwitch();
        }
    }

    /// <summary>
    /// Player Selected The Bag Button
    /// </summary>
    void PlayerActionBag()
    {
        //Open Bag button
        EnableActionSelector(false);
        StartCoroutine(PlayerThrewPokeball(enemyBattleUnit));
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
        dialogBox.EnableActionSelector(enabled);
        if (enabled == true)
        {
            actionSelectionEventSelector.SelectBox();
        }
    }

    /// <summary>
    /// Turns on/off the current Dialog box and the updater for the PP system as well as the type of move it is
    /// </summary>
    /// <param name="enabled"></param>
    void EnableAttackMoveSelector(bool enabled)
    {
        dialogBox.EnableMoveSelector(enabled);
        if (enabled == true)
        {
            attackSelectionEventSelector.SelectBox();
        }
    }

    /// <summary>
    /// Called from the attack button listener passing the correct information to process through what pokemon did what attack
    /// </summary>
    /// <param name="currentPokemon">Current Battle Unit Pokemon</param>
    /// <param name="moveBase">Current Move Being Used</param>
    public void AttackSelected(BattleUnit currentPokemon, Move move)
    {
        dialogBox.SetDialogText("");
        EnableAttackMoveSelector(false);
        TurnAttackDetails turnAttack = new TurnAttackDetails(move, currentPokemon, enemyBattleUnit);

        _escapeAttempts = 0;
        _currentTurnDetails.Add(turnAttack);
        StartCoroutine(EnemyMove());
    }

    public void AttackHasRunOutOfPP()
    {
        StartCoroutine(AttackHasRunOutOfPPIEnumerator());
    }

    IEnumerator AttackHasRunOutOfPPIEnumerator()
    {
        EnableAttackMoveSelector(false);
        yield return dialogBox.TypeDialog("There's no PP left for this move!");
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
        yield return dialogBox.TypeDialog($"{playerBattleUnit.pokemon.currentName} has no available moves left");
        yield return new WaitForSeconds(1);
        AttackSelected(playerBattleUnit, new Move(struggle));
    }

    public void PlayerCantEscapeActiveWhenTryingToSwitch()
    {
        StartCoroutine(PlayerCantEscapeActiveWhenTryingToSwitchIEnumerator());
    }

    IEnumerator PlayerCantEscapeActiveWhenTryingToSwitchIEnumerator()
    {
        EnableActionSelector(false);
        yield return dialogBox.TypeDialog($"{playerBattleUnit.pokemon.currentName} can't escape!",true);
        EnableActionSelector(true);
    }

    public void ReturnFromPokemonPartySystem()
    {
        actionSelectionEventSelector.SelectBox();
    }

    IEnumerator RunFromBattle()
    {
        if (_isTrainerBattle == true)
        {
            yield return dialogBox.TypeDialog($"You cant run from a trainer battle", true);
            PlayerActions();
            yield break;
        }

        _escapeAttempts++;
        int playerSpeed = playerBattleUnit.pokemon.speed;
        int enemySpeed = enemyBattleUnit.pokemon.speed;

        if (playerSpeed > enemySpeed)
        {
            yield return dialogBox.TypeDialog($"You got away safely", true);
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
                yield return dialogBox.TypeDialog($"You got away safely", true);
                OnBattleOver(true);
                yield break;
            }
            else
            {
                yield return dialogBox.TypeDialog($"You were unable to escape!", true);
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

        TurnAttackDetails turnAttack = new TurnAttackDetails(currentAttack, enemyBattleUnit, playerBattleUnit);
        _currentTurnDetails.Add(turnAttack);

        yield return RunThroughTurns(_currentTurnDetails);
    }

    IEnumerator TrainerAboutToUsePokemonFeature(Pokemon nextPokemon)
    {
        yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} is about to use {nextPokemon.pokemonBase.GetPokedexName()}", true);

        yield return dialogBox.TypeDialog($"Will {_playerController.TrainerName} change Pokemon?");

        yield return dialogBox.SetChoiceBox(() =>
        {
            OpenPokemonParty(true);
        }
        , () =>
        {
            _playerPokemonShift = false;
        });

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
            yield return dialogBox.TypeDialog($"{trainerLines[i]}", true);
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
        TurnAttackDetails currentAttack;

        while(attacksChosen.Count > 0)
        {
            currentAttack = attacksChosen[0];

            if (attacksChosen.Count > 1)
            {
                foreach(TurnAttackDetails attack in attacksChosen)
                {
                    if(currentAttack == attack)
                    {
                        continue;
                    }

                    if(currentAttack.currentMove.moveBase.Priority > attack.currentMove.moveBase.Priority)
                    {
                        continue;
                    }

                    if(currentAttack.currentMove.moveBase.Priority == attack.currentMove.moveBase.Priority)
                    {
                        if (currentAttack.attackingPokemon.pokemon.speed > attack.attackingPokemon.pokemon.speed)
                        {
                            continue;
                        }
                        else if (currentAttack.attackingPokemon.pokemon.speed == attack.attackingPokemon.pokemon.speed)
                        {
                            bool coinFlip = (Random.value > 0.5f);
                            if (coinFlip == true)
                            {
                                currentAttack = attack;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else //currentAttack.attackingPokemon.pokemon.speed < attack.attackingPokemon.pokemon.speed
                        {
                            currentAttack = attack;
                        }
                    }
                    else //currentAttack.currentMove.Priority < attack.currentMove.Priority
                    {
                        currentAttack = attack;
                    }
                }
            }
            if (currentAttack.attackingPokemon.pokemon.currentHitPoints > 0)
            {
                yield return RunMove(currentAttack.attackingPokemon, currentAttack.targetPokmeon, currentAttack.currentMove);
            }

            attacksChosen.Remove(currentAttack);
        }

        if(_currentWeather != null)
        {
            yield return ShowWeatherEffect(_currentWeather);

            yield return ApplyWeatherEffectsOnEndTurn(_currentWeather, playerBattleUnit);
            yield return ApplyWeatherEffectsOnEndTurn(_currentWeather, enemyBattleUnit);
        }

        //Called after all attacks have been done

        yield return ApplyEffectsOnEndTurn(playerBattleUnit,enemyBattleUnit);
        yield return ApplyEffectsOnEndTurn(enemyBattleUnit, playerBattleUnit);

        //If current pokemon has fainted then it goes to the party system and waits on the selector
        if (playerBattleUnit.SendOutPokemonOnTurnEnd == true)
        {
            OpenPokemonParty(false);
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
            yield return sourceUnit.HUD.UpdateHPDamage(previousHP);
            if (sourceUnit.pokemon.currentHitPoints <= 0)
            {
                yield return PokemonHasFainted(sourceUnit);
            }
            yield break;
        }
        //This is here incase the pokemon has a status effect that ended such as being frozen/Sleep
        yield return ShowStatusChanges(sourceUnit.pokemon);

        yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} used {move.moveBase.MoveName}");
        move.pP--;

        if (targetUnit.pokemon.currentHitPoints <= 0 && move.moveBase.Target == MoveTarget.Foe && move.moveBase.RecoilPercentage < 100)
        {
            yield return dialogBox.TypeDialog($"There is no target Pokemon");
            yield break;
        }

        if (targetUnit.pokemon.ability.PreventsTheUseOfSpecificMoves(sourceUnit.pokemon, move.moveBase) == true && targetUnit.pokemon.currentHitPoints > 0)
        {
            targetUnit.OnAbilityActivation();
            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield break;
        }

        //Moved here so it shows an attack animation, just skips out on the pokemon recieving the hit animation
        yield return sourceUnit.PlayAttackAnimation();
        int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP and recoil

        if (CheckIfMoveHasSpecializedConditionAndSuccessful(sourceUnit, targetUnit, move.moveBase) == false)
        {
            yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
            yield break;
        }

        if (CheckIfMoveHits(move.moveBase, sourceUnit.pokemon, targetUnit.pokemon) == true)
        {
            if (move.moveBase.MoveType == MoveType.Status)
            {
                yield return RunMoveEffects(move.moveBase.MoveEffects, sourceUnit, targetUnit, move.moveBase, move.moveBase.Target);
            }
            else
            {
                int attackLoop = 1;

                if(move.moveBase.MultiStrikeMove == true)
                {
                    if(move.moveBase.FixedNumberOfStrikes > 0)
                    {
                        attackLoop = move.moveBase.FixedNumberOfStrikes;
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

                DamageDetails damageDetails = targetUnit.pokemon.TakeDamage(move.moveBase, sourceUnit.pokemon);

                if (attackLoop > 1)
                {
                    dialogBox.SetDialogText("");

                    for (int i = 0; i < attackLoop; i++)
                    {
                        if (i > 0)
                        {
                            damageDetails = targetUnit.pokemon.TakeDamage(move.moveBase, sourceUnit.pokemon);
                        }

                        if (move.moveBase.Target == MoveTarget.Foe && damageDetails.typeEffectiveness != 0)
                        {
                            targetUnit.PlayHitAnimation();
                        }

                        if (damageDetails.damageNullified == true)
                        {
                            break;
                        }

                        yield return targetUnit.HUD.UpdateHPDamage(hpPriorToAttack);

                        if(damageDetails.criticalHit > 1 && i < attackLoop)
                        {
                            yield return dialogBox.TypeDialog("A Critical Hit!");
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
                    if (move.moveBase.Target == MoveTarget.Foe && damageDetails.typeEffectiveness != 0)
                    {
                        targetUnit.PlayHitAnimation();
                    }

                    if(hpPriorToAttack > targetUnit.pokemon.currentHitPoints)
                    {
                        yield return targetUnit.HUD.UpdateHPDamage(hpPriorToAttack);
                    }
                }
                
                yield return ShowDamageDetails(damageDetails, targetUnit);
                if(damageDetails.damageNullified == true)
                {
                    yield return targetUnit.HUD.UpdateHPRecovered(targetUnit.pokemon.currentHitPoints- hpPriorToAttack);
                    yield return ApplyStatChanges(damageDetails.defendersStatBoostByAbility, targetUnit, MoveTarget.Foe);
                    yield return ApplyStatChanges(damageDetails.attackersStatBoostByDefendersAbility, sourceUnit, MoveTarget.Self, sourceUnit);

                    yield break;
                }

                if(targetUnit.pokemon.currentHitPoints > 0)
                {
                    yield return ApplyStatChanges(damageDetails.defendersStatBoostByAbility, targetUnit, MoveTarget.Foe);
                }

                if (sourceUnit.pokemon.currentHitPoints > 0)
                {
                    yield return ApplyStatChanges(damageDetails.attackersStatBoostByDefendersAbility, targetUnit, MoveTarget.Self,sourceUnit);
                }

                if (move.moveBase.MultiStrikeMove == true && damageDetails.typeEffectiveness != 0)
                {
                    yield return dialogBox.TypeDialog($"Hit {attackLoop} time(s)!");
                }

                if(sourceUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == false)
                {
                    if (move.moveBase.DrainsHP == true && sourceUnit.pokemon.currentHitPoints != sourceUnit.pokemon.maxHitPoints && hpPriorToAttack - targetUnit.pokemon.currentHitPoints > 0)
                    {
                        int hpHealed = Mathf.CeilToInt((hpPriorToAttack - targetUnit.pokemon.currentHitPoints) * (move.moveBase.HpRecovered));

                        hpHealed = Mathf.Clamp(hpHealed, 1, sourceUnit.pokemon.maxHitPoints - sourceUnit.pokemon.currentHitPoints);
                        sourceUnit.pokemon.UpdateHPRestored(hpHealed);
                        yield return sourceUnit.HUD.UpdateHPRecovered(hpHealed);
                        yield return dialogBox.TypeDialog($"{targetUnit.pokemon.currentName} had its energy drained");
                    }
                }

                //Shows that the damage does not effect them and then ends the move right there
                if (damageDetails.typeEffectiveness == 0)
                {
                    yield break;
                }
                
            }

            if (move.moveBase.SecondaryEffects != null && move.moveBase.SecondaryEffects.Count > 0 && targetUnit.pokemon.currentHitPoints > 0)
            {
                foreach (var secondaryEffect in move.moveBase.SecondaryEffects)
                {
                    int rnd = Random.Range(1, 101);
                    if (rnd <= secondaryEffect.PercentChance)
                    {
                        yield return RunMoveEffects(secondaryEffect, sourceUnit, targetUnit, move.moveBase, secondaryEffect.Target, (secondaryEffect.PercentChance<100)) ;

                        if (secondaryEffect.Volatiletatus == ConditionID.CursedUser)
                        {
                            yield return sourceUnit.HUD.UpdateHPDamage(previousHP);
                            if (sourceUnit.pokemon.currentHitPoints <= 0)
                            {
                                yield return PokemonHasFainted(sourceUnit);
                            }
                        }
                    }
                }
            }

            ConditionID newCondition = targetUnit.pokemon.ability.ContactMoveMayCauseStatusEffect(targetUnit.pokemon, sourceUnit.pokemon, move.moveBase);

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
                    sourceUnit.pokemon.SetVolatileStatus(newCondition, move.moveBase,sourceUnit);
                    yield return ShowStatusChanges(sourceUnit.pokemon);
                }
            }

            //Recoil
            if (move.moveBase.RecoilType != Recoil.NA)
            {
                int recoilDamage = 0;

                if(move.moveBase.RecoilType == Recoil.DamageDealt)
                {
                    recoilDamage = hpPriorToAttack - targetUnit.pokemon.currentHitPoints;
                    recoilDamage = Mathf.CeilToInt(recoilDamage * move.moveBase.RecoilPercentage);
                }
                else if(move.moveBase.RecoilType == Recoil.UsersCurrentHP)
                {
                    recoilDamage = sourceUnit.pokemon.currentHitPoints;
                    recoilDamage = Mathf.CeilToInt(recoilDamage * move.moveBase.RecoilPercentage);
                }
                else//users max hp
                {
                    recoilDamage = sourceUnit.pokemon.maxHitPoints;
                    recoilDamage = Mathf.CeilToInt(recoilDamage * move.moveBase.RecoilPercentage);
                }
                
                previousHP = sourceUnit.pokemon.currentHitPoints;
                sourceUnit.pokemon.UpdateHPDamage(recoilDamage);
                yield return sourceUnit.HUD.UpdateHPDamage(previousHP);
                yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} is hit with recoil!");

                if (sourceUnit.pokemon.currentHitPoints <= 0)
                {
                    yield return PokemonHasFainted(sourceUnit);
                }
            }

            sourceUnit.lastMoveUsed = move;
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName}'s attack missed!");
        }

        //explosive moves incase they miss
        if (move.moveBase.RecoilType == Recoil.UsersMaximumHP && move.moveBase.RecoilPercentage >= 100 && sourceUnit.pokemon.currentHitPoints <= 0)
        {
            int recoilDamage = sourceUnit.pokemon.currentHitPoints;
            sourceUnit.pokemon.UpdateHPDamage(recoilDamage);
            yield return sourceUnit.HUD.UpdateHPDamage(previousHP);

            if (sourceUnit.pokemon.currentHitPoints <= 0)
            {
                yield return PokemonHasFainted(sourceUnit);
            }
        }

        if (targetUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(targetUnit);
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
                        yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
                        yield break;
                    }
                }
                else
                {
                    if(target.pokemon.volatileStatus.Exists(x => x.Id == effects.Volatiletatus) == true && wasSecondaryEffect == false)
                    {
                        yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
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
                    yield return dialogBox.TypeDialog("All Pokemon that heard the song will faint in 3 turns");
                }

                target.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,source);
                source.pokemon.SetVolatileStatus(effects.Volatiletatus, currentMove,target);
            }
        }

        if (effects.WeatherEffect != WeatherEffectID.NA)
        {
            yield return StartWeatherEffect(effects.WeatherEffect);
        }

        if (effects.EntryHazard != EntryHazardID.NA)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                List<EntryHazardBase> currentEntrySide = (target.isPlayerPokemon) ? _playerSideEntryHazards : _enemySideEntryHazards;
                EntryHazardBase currentHazard = EntryHazardsDB.GetEntryHazardBase(effects.EntryHazard);

                if (currentEntrySide.Contains(currentHazard) == true)
                {
                    currentHazard = currentEntrySide.Find(x => x.Id == effects.EntryHazard);
                    if (currentHazard.CanBeUsed() == false)
                    {
                        yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
                        yield break;
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog(currentHazard.StartMessage(target));
                    }
                }
                else
                {
                    currentEntrySide.Add(currentHazard);
                    yield return dialogBox.TypeDialog(currentHazard.StartMessage(target));
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
                int hpHealed = Mathf.CeilToInt(source.pokemon.maxHitPoints * (currentMove.HpRecovered * source.pokemon.ability.PowerUpCertainMoves(source.pokemon, target.pokemon, currentMove,GetCurrentWeather)));
                WeatherEffectID weatherEffectID = (_currentWeather == null) ? WeatherEffectID.NA : _currentWeather.Id;
                hpHealed = Mathf.CeilToInt(hpHealed * HealthRecoveryModifiers(currentMove, weatherEffectID));
                hpHealed = Mathf.Clamp(hpHealed, 1, source.pokemon.maxHitPoints - source.pokemon.currentHitPoints);
                source.pokemon.UpdateHPRestored(hpHealed);
                yield return source.HUD.UpdateHPRecovered(hpHealed);
                source.pokemon.statusChanges.Enqueue($"{source.pokemon.currentName}'s hp was restored");
            }
            else
            {
                if(currentMove != rest)
                {
                    yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
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

        List<ConditionBase> allConditionsOnPokemon = new List<ConditionBase>();
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

            sourceUnit.pokemon.OnEndTurn(currentCondition);
            yield return sourceUnit.StatusConditionAnimation(currentCondition.Id);

            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield return sourceUnit.HUD.UpdateHPDamage(currentHP);

            if(currentCondition.Id == ConditionID.LeechSeed)
            {
                LeechSeed leechSeed = ((LeechSeed)currentCondition);
                if(targetUnit.pokemon.HasCurrentVolatileStatus(ConditionID.HealBlock) == true)
                {
                    continue;
                }
                if(targetUnit.pokemon.maxHitPoints - targetUnit.pokemon.currentHitPoints > 0)
                {
                    int hpHealed = Mathf.Clamp(leechSeed.HealthStolen, 1, targetUnit.pokemon.maxHitPoints - targetUnit.pokemon.currentHitPoints);
                    targetUnit.pokemon.UpdateHPRestored(hpHealed);
                    yield return targetUnit.HUD.UpdateHPRecovered(leechSeed.HealthStolen);
                }
            }
        }

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
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
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.statusChanges.Count > 0)
        {
            string message = pokemon.statusChanges.Dequeue();
            if(message != "")
            {
                yield return dialogBox.TypeDialog(message);
            }
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails, BattleUnit battleUnit)
    {
        if (damageDetails.criticalHit > 1f)
        {
            yield return dialogBox.TypeDialog("A Critical Hit!");
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
            yield return dialogBox.TypeDialog($"It doesnt effect {battleUnit.pokemon.currentName}");
        }
        else if (damageDetails.typeEffectiveness <= 0.5f)
        {
            yield return dialogBox.TypeDialog($"It's not very effective");
        }
        else if (damageDetails.typeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"It's super effective!");
        }

        if(damageDetails.abilityActivation == true && battleUnit.pokemon.currentHitPoints > 0)
        {
            battleUnit.OnAbilityActivation();
            yield return ShowStatusChanges(battleUnit.pokemon);
        }
    }

    IEnumerator ApplyStatChanges(List<StatBoost> statBoosts,BattleUnit targetUnit, MoveTarget moveTarget,BattleUnit sourceUnit = null)
    {
        if (statBoosts.Count == 0)
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
            targetUnit.pokemon.ApplyStatModifier(positiveEffects);
            yield return targetUnit.ShowStatChanges(positiveEffects, true);
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

            targetUnit.pokemon.ApplyStatModifier(negativeEffects);
            yield return targetUnit.ShowStatChanges(negativeEffects, false);
            yield return ShowStatusChanges(targetUnit.pokemon);
        }
        else if (moveTarget == MoveTarget.Self && sourceUnit != null)
        {
            sourceUnit.pokemon.ApplyStatModifier(positiveEffects);
            yield return sourceUnit.ShowStatChanges(positiveEffects, true);
            yield return ShowStatusChanges(sourceUnit.pokemon);

            sourceUnit.pokemon.ApplyStatModifier(negativeEffects);
            yield return sourceUnit.ShowStatChanges(negativeEffects, false);
            yield return ShowStatusChanges(sourceUnit.pokemon);
        }

        if(boostedStats.Count > 0)
        {
            targetUnit.OnAbilityActivation();
            yield return ApplyStatChanges(boostedStats, targetUnit, MoveTarget.Self,targetUnit);
        }
    }

    IEnumerator ActivatePokemonAbilityUponEntry(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        if (sourceUnit.pokemon.ability == null)
        {
            yield break;
        }

        WeatherEffectID weatherEffect = sourceUnit.pokemon.ability.OnStartWeatherEffect();
        if (weatherEffect != WeatherEffectID.NA)
        {
            sourceUnit.OnAbilityActivation();
            yield return StartWeatherEffect(weatherEffect, true);
            yield break;
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
            if(_currentWeather != null || _currentWeather?.Id == WeatherEffectID.NA)
            {
                sourceUnit.OnAbilityActivation();
                _currentWeather = null;
                yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName}'s {sourceUnit.pokemon.ability.Name} cleared the battlefield");
                yield break;
            }
        }
    }

    #endregion

    #region PokemonFaint/Switching

    IEnumerator PokemonHasFainted(BattleUnit targetBattleUnit)
    {
        if (targetBattleUnit.pokemon.currentHitPoints <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetBattleUnit.pokemon.currentName} has fainted");
            targetBattleUnit.PlayFaintAnimation();

            if (targetBattleUnit.isPlayerPokemon == false)
            {
                int expYield = targetBattleUnit.pokemon.pokemonBase.RewardedExperienceYield;
                int level = targetBattleUnit.pokemon.currentLevel;
                float trainerBonus = (_isTrainerBattle == true) ? 1.5f : 1;
                float pokemonSharingExp = 1;

                if (targetBattleUnit.GetListOfPokemonBattledAgainst.Count > 1)
                {
                    int sharing = 0;
                    List<Pokemon> copyPokemonBattledAgainst = new List<Pokemon>(targetBattleUnit.GetListOfPokemonBattledAgainst);
                    foreach (Pokemon pokemon in copyPokemonBattledAgainst)
                    {
                        if (pokemon.currentLevel >= 100 || pokemon.currentHitPoints <= 0)
                        {
                            targetBattleUnit.GetListOfPokemonBattledAgainst.Remove(pokemon);
                            continue;
                        }
                        sharing++;
                    }
                    if (sharing > 1)
                    {
                        pokemonSharingExp = sharing;
                    }
                }

                int expGained = Mathf.FloorToInt(expYield * level * trainerBonus / pokemonSharingExp) / 7;

                foreach (Pokemon pokemon in targetBattleUnit.GetListOfPokemonBattledAgainst)
                {
                    if(pokemon.currentHitPoints <=0)
                    {
                        continue;
                    }

                    if (pokemon == playerBattleUnit.pokemon)
                    {
                        yield return GainExperience(playerBattleUnit, expGained);
                    }
                    else
                    {
                        yield return GainExperience(pokemon, expGained);
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
        dialogBox.WaitingOnUserChoice = false;
        _playerPokemonShift = false;
    }

    public void PlayerSwitchPokemon(Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemonIEnumerator(playerBattleUnit,newPokemon));
        EnableActionSelector(false);
    }

    IEnumerator SwitchPokemonIEnumerator(BattleUnit battleUnit,Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if (battleUnit.pokemon.currentHitPoints > 0)
        {
            currentPokemonFainted = false;
            yield return dialogBox.TypeDialog($"Come Back {battleUnit.pokemon.currentName}!");
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
            attackSelectionEventSelector.SetMovesList(battleUnit, battleUnit.pokemon.moves, this);
            yield return dialogBox.TypeDialog($"Go {battleUnit.pokemon.currentName}!");
        }
        else
        {
            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} sent out {battleUnit.pokemon.currentName}");
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
            yield return ActivatePokemonAbilityUponEntry(playerBattleUnit, enemyBattleUnit);
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
            yield return ActivatePokemonAbilityUponEntry(enemyBattleUnit, playerBattleUnit);
            //yield return RunThroughTurns(_currentTurnDetails);
        }
    }

    #endregion

    #region Entry Hazards

    IEnumerator ApplyEntryHazardOnSentOut(BattleUnit target)
    {
        if (target.pokemon.currentHitPoints <= 0)
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
            yield return target.HUD.UpdateHPDamage(currentHP);

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
        string message = weather.OnEndTurn(this);
        yield return dialogBox.TypeDialog(message);
    }

    IEnumerator StartWeatherEffect(WeatherEffectID weatherID,bool wasAbility = false)
    {
        if (_currentWeather != null)
        {
            if (_currentWeather.Id == weatherID)
            {
                if(wasAbility == false)
                {
                    yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
                }
                yield break;
            }
        }

        if(enemyBattleUnit.pokemon.ability.NegatesWeatherEffects() == true)
        {
            enemyBattleUnit.OnAbilityActivation();

            if(wasAbility == true)
            {
                yield return dialogBox.TypeDialog($"{playerBattleUnit.pokemon.currentName}'s {playerBattleUnit.pokemon.ability.Name} was negated by {enemyBattleUnit.pokemon.currentName}'s {enemyBattleUnit.pokemon.ability.Name}");
            }
            else
            {
                yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
            }
            yield break;
        }
        else if(playerBattleUnit.pokemon.ability.NegatesWeatherEffects() == true)
        {
            playerBattleUnit.OnAbilityActivation();
            if (wasAbility == true)
            {
                yield return dialogBox.TypeDialog($"{enemyBattleUnit.pokemon.currentName}'s {enemyBattleUnit.pokemon.ability.Name} was negated by {playerBattleUnit.pokemon.currentName}'s {playerBattleUnit.pokemon.ability.Name}");
            }
            else
            {
                yield return dialogBox.TypeDialog($"{BUT_IT_FAILED}");
            }
            yield break;
        }

        _currentWeather = WeatherEffectDB.WeatherEffects[weatherID];
        yield return dialogBox.TypeDialog(_currentWeather.StartMessage());
    }

    public void RemoveWeatherEffect()
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
            yield return sourceUnit.HUD.UpdateHPDamage(currentHP);
        }

        currentHP = sourceUnit.pokemon.currentHitPoints;
        if (sourceUnit.pokemon.ability.AffectsHpByXEachTurnWithWeather(sourceUnit.pokemon, weather.Id) == true)
        {
            sourceUnit.OnAbilityActivation();
            yield return ShowStatusChanges(sourceUnit.pokemon);
            if (currentHP < sourceUnit.pokemon.currentHitPoints)
            {
                yield return sourceUnit.HUD.UpdateHPRecovered(sourceUnit.pokemon.currentHitPoints - currentHP);
            }
            else
            {
                yield return sourceUnit.HUD.UpdateHPDamage(currentHP);
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

    IEnumerator PlayerThrewPokeball(BattleUnit targetUnit)
    {
        if(_isTrainerBattle == true)
        {
            EnableActionSelector(false);
            yield return dialogBox.TypeDialog($"You cant capture Trainers Pokemon", true);
            PlayerActions();
            yield break;
        }

        

        inGameItem.SetActive(true);
        InBattleItem currentBall = inGameItem.GetComponent<InBattleItem>();
        Vector3 ballHeightJump = new Vector3(0, 75, 0);

        yield return dialogBox.TypeDialog($"{_playerController.TrainerName} used {currentBall.GetItemName}");
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
            yield return dialogBox.TypeDialog($"{targetUnit.pokemon.currentName} was Caught!",true);
            yield return currentBall.FadeItem(false);
            OnPokemonCaptured(enemyBattleUnit.pokemon,currentBall.CurrentPokeball);
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
                yield return dialogBox.TypeDialog($"Oh no! The Pokémon broke free!",true);
            }
            else if(shakeCount == 1)
            {
                yield return dialogBox.TypeDialog($"Aww! It appeared to be caught!",true);
            }
            else if (shakeCount == 2)
            {
                yield return dialogBox.TypeDialog($"Aargh! Almost had it!",true);
            }
            else if (shakeCount == 3)
            {
                yield return dialogBox.TypeDialog($"Gah! It was so close, too!",true);
            }
            StartCoroutine(EnemyMove());
        }
    }

    #endregion

    #region Experience and Leveling Mechanics

    IEnumerator GainExperience(BattleUnit targetUnit,int expGained)
    {
        int expBeforeAnim = targetUnit.pokemon.currentExp;
        targetUnit.pokemon.currentExp += expGained;
        yield return dialogBox.TypeDialog($"{targetUnit.pokemon.currentName} gained {expGained} exp", true);
        yield return targetUnit.HUD.GainExpAnimation(expGained, expBeforeAnim);

        //Level up Here

        StandardStats StatsBeforeLevel = targetUnit.pokemon.GetStandardStats();

        while (targetUnit.pokemon.LevelUpCheck() == true)
        {
            expGained -= targetUnit.pokemon.pokemonBase.GetExpForLevel(targetUnit.pokemon.currentLevel) - expBeforeAnim;
            expBeforeAnim = targetUnit.pokemon.pokemonBase.GetExpForLevel(targetUnit.pokemon.currentLevel);
            yield return dialogBox.TypeDialog($"{targetUnit.pokemon.currentName} grew to level {targetUnit.pokemon.currentLevel}!", true);
            targetUnit.HUD.SetLevel();
            //Play level up animation

            yield return levelUpUI.DisplayLevelUp(StatsBeforeLevel, targetUnit.pokemon.GetStandardStats());
            targetUnit.HUD.UpdateHPWithoutAnimation();

            List<LearnableMove> newMove = targetUnit.pokemon.GetLearnableMoveAtCurrentLevel();

            if (newMove.Count > 0)
            {
                yield return LearnNewMove(targetUnit.pokemon,newMove);
                attackSelectionEventSelector.SetMovesList(targetUnit, targetUnit.pokemon.moves, this);
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
        yield return dialogBox.TypeDialog($"{pokemon.currentName} gained {expGained} exp", true);

        StandardStats StatsBeforeLevel = pokemon.GetStandardStats();

        while (pokemon.LevelUpCheck() == true)
        {
            expGained -= pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel) - expBeforeAnim;
            expBeforeAnim = pokemon.pokemonBase.GetExpForLevel(pokemon.currentLevel);
            yield return dialogBox.TypeDialog($"{pokemon.currentName} grew to level {pokemon.currentLevel}!", true);

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
                currentPokemon.LearnMove(learnableMove);
                yield return dialogBox.TypeDialog($"{currentPokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
            }
            else
            {

                bool playerSelectingNewMove = true;

                while (playerSelectingNewMove == true)
                {
                    yield return dialogBox.TypeDialog($"{currentPokemon.currentName} is trying to learn {learnableMove.moveBase.MoveName}.", true);
                    yield return dialogBox.TypeDialog($"But {currentPokemon.currentName} can't learn more than four moves.", true);
                    yield return dialogBox.TypeDialog($"Delete a move to make room for {learnableMove.moveBase.MoveName}?");

                    bool ifPlayerSelectsNo = false;

                    yield return dialogBox.SetChoiceBox(() =>
                    {
                        learnNewMoveUI.OpenToLearnNewMove(currentPokemon, learnableMove.moveBase, () =>
                        {
                            dialogBox.WaitingOnUserChoice = false;
                            playerSelectingNewMove = false;
                        });
                        learnNewMoveUI.SelectBox();
                    }
                    , () =>
                    {
                        ifPlayerSelectsNo = true;
                        dialogBox.WaitingOnUserChoice = false;
                    });

                    if (ifPlayerSelectsNo == false)
                    {
                        ifPlayerSelectsNo = learnNewMoveUI.PlayerDoesNotWantToLearnMove;
                    }

                    if (ifPlayerSelectsNo == true)
                    {
                        yield return dialogBox.TypeDialog($"Stop Learning {learnableMove.moveBase.MoveName}?");
                        yield return dialogBox.SetChoiceBox(() =>
                        {
                            playerSelectingNewMove = false;
                            dialogBox.WaitingOnUserChoice = false;
                        }
                        , () =>
                        {
                            playerSelectingNewMove = true;
                            dialogBox.WaitingOnUserChoice = false;
                        });

                        if (playerSelectingNewMove == false)
                        {
                            yield return dialogBox.TypeDialog($"{currentPokemon.currentName} did not learn {learnableMove.moveBase.MoveName}");
                        }
                    }
                    else if (ifPlayerSelectsNo == false && playerSelectingNewMove == false)
                    {
                        yield return dialogBox.TypeDialog($"{currentPokemon.currentName} forgot how to use {learnNewMoveUI.previousMoveName}", true);
                        yield return dialogBox.TypeDialog($"{currentPokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
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

    #endregion

}
