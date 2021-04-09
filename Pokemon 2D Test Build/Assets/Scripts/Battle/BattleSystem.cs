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
    //inbattle,wasShift
    public event Action<bool,bool> OpenPokemonParty;

    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] ActionSelectionEventSelector actionSelectionEventSelector;
    [SerializeField] AttackSelectionEventSelector attackSelectionEventSelector;

    List<TurnAttackDetails> _currentTurnDetails;

    PlayerController _playerController;
    TrainerController _trainerController;
    PokemonParty _playerParty;
    PokemonParty _trainerParty;
    Pokemon _wildPokemon;
    bool _isTrainerBattle;
    bool _playerPokemonShift;

    static WeatherEffect _currentWeather;
    public int weatherDuration {get; set;}

    List<EntryHazard> _playerSideEntryHazards = new List<EntryHazard>();
    List<EntryHazard> _enemySideEntryHazards = new List<EntryHazard>();

    [SerializeField] GameObject inGameItem;
    Vector2 inGameItemoffScreenPos;
    public event Action<Pokemon> OnPokemonCaptured;

    int _escapeAttempts;

    [SerializeField] LearnNewMoveUI learnNewMoveUI;
    [SerializeField] LevelUpUI levelUpUI;


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

    //28.0 Pok�mon is switched in (if previous Pok�mon fainted)
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

    public void SetupBattleArt(LevelArtDetails levelArt)
    {
        backgroundArt.sprite = levelArt.background;
        playerBattleUnit.SetBattlePositionArt(levelArt.playerPosition);
        enemyBattleUnit.SetBattlePositionArt(levelArt.enemyPosition);
    }

    public void StartBattle(PokemonParty playerParty,Pokemon wildPokemon)
    {
        _playerParty = playerParty;
        _playerController = playerParty.GetComponent<PlayerController>();

        Pokemon newWildPokemon = new Pokemon(wildPokemon.pokemonBase,wildPokemon.currentLevel);

        _trainerController = null;
        _wildPokemon = newWildPokemon;
        _isTrainerBattle = false;

        StartCoroutine(SetupBattle());
        _currentTurnDetails = new List<TurnAttackDetails>();
    }

    public void StartBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        _playerParty = playerParty;
        _playerController = playerParty.GetComponent<PlayerController>();

        _trainerParty = trainerParty;
        _trainerController = trainerParty.GetComponent<TrainerController>();
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
        if (_isTrainerBattle == true)
        {
            playerBattleUnit.ShowPokemonImage(false);
            enemyBattleUnit.ShowPokemonImage(false);

            playerBattleUnit.Trainer.gameObject.SetActive(true);
            enemyBattleUnit.Trainer.gameObject.SetActive(true);

            playerBattleUnit.Trainer.sprite = _playerController.BackBattleSprite[0];
            enemyBattleUnit.Trainer.sprite = _trainerController.FrontBattleSprite[0];

            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} wants to battle!",true);

            enemyBattleUnit.ShowPokemonImage(true);
            enemyBattleUnit.Trainer.gameObject.SetActive(false);
            Pokemon enemyPokemon = _trainerParty.GetFirstHealthyPokemon();
            enemyBattleUnit.Setup(enemyPokemon,true,_trainerController != null);
            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} sent out {enemyPokemon.currentName}");

            yield return new WaitForSeconds(0.5f);

            playerBattleUnit.ShowPokemonImage(true);
            playerBattleUnit.Trainer.gameObject.SetActive(false);
            Pokemon playerPokemon = _playerParty.GetFirstHealthyPokemon();
            playerBattleUnit.Setup(playerPokemon,true, _playerController != null);
            yield return dialogBox.TypeDialog($"Go {playerPokemon.currentName}");

            yield return new WaitForSeconds(0.5f);
        }
        else //WildPokemon
        {
            playerBattleUnit.ShowPokemonImage(false);
            playerBattleUnit.Trainer.gameObject.SetActive(true);

            playerBattleUnit.Setup(_playerParty.GetFirstHealthyPokemon(),true, _playerController != null);
            enemyBattleUnit.Setup(_wildPokemon,true,_trainerController != null);

            while(playerBattleUnit.startingAnimationsActive == true && enemyBattleUnit.startingAnimationsActive == true)
            {
                yield return null;
            }

            yield return dialogBox.TypeDialog($"A wild {enemyBattleUnit.pokemon.currentName} has appeared!");
        }

        _playerParty.SetOriginalPositions();
        _playerParty.CleanUpPartyOrderOnStart(playerBattleUnit.pokemon);
        _escapeAttempts = 0;
        enemyBattleUnit.AddPokemonToBattleList(playerBattleUnit.pokemon);
        dialogBox.BattleStartSetup();
        attackSelectionEventSelector.SetMovesList(playerBattleUnit,playerBattleUnit.pokemon.moves,this);

        _currentWeather = null;

        yield return ActivatePokemonAbilityUponEntry(playerBattleUnit,enemyBattleUnit);
        yield return ActivatePokemonAbilityUponEntry(enemyBattleUnit,playerBattleUnit);

        SetupPlayerActions();
        PlayerActions();
    }

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
        EnableAttackMoveSelector(true);
    }

    /// <summary>
    /// Player Selected The Pokemon Button
    /// </summary>
    void PlayerActionPokemon()
    {
        OpenPokemonParty(true,false);
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
    public void AttackSelected(BattleUnit currentPokemon, MoveBase moveBase)
    {
        EnableAttackMoveSelector(false);
        TurnAttackDetails turnAttack = new TurnAttackDetails(moveBase, currentPokemon, enemyBattleUnit);

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

    #endregion

    public IEnumerator EnemyMove()
    {
        Move currentAttack = enemyBattleUnit.pokemon.ReturnRandomMove();

        TurnAttackDetails turnAttack = new TurnAttackDetails(currentAttack.moveBase, enemyBattleUnit, playerBattleUnit);
        _currentTurnDetails.Add(turnAttack);
        //yield return RunMove(_enemyBattleUnit, _playerBattleUnit, currentAttack.moveBase);

        yield return RunThroughTurns(_currentTurnDetails);
    }

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

                    if(currentAttack.currentMove.Priority > attack.currentMove.Priority)
                    {
                        continue;
                    }

                    if(currentAttack.currentMove.Priority == attack.currentMove.Priority)
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

        yield return ApplyEffectsOnEndTurn(playerBattleUnit);
        yield return ApplyEffectsOnEndTurn(enemyBattleUnit);

        //If current pokemon has fainted then it goes to the party system and waits on the selector
        if (playerBattleUnit.SendOutPokemonOnTurnEnd == true)
        {
            OpenPokemonParty(true,false);
        }
        else
        {
            if(enemyBattleUnit.SendOutPokemonOnTurnEnd == true)
            {
                //This will be updated with a better AI later
                Pokemon nextEnemyPokemon = _trainerParty.GetFirstHealthyPokemon();

                _playerPokemonShift = true;
                yield return TrainerAboutToUsePokemonFeature(nextEnemyPokemon);
            }
            PlayerActions();
        }
    }

    IEnumerator ApplyEffectsOnEndTurn(BattleUnit sourceUnit)
    {
        if(sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield break;
        }

        int currentHP = sourceUnit.pokemon.currentHitPoints;

        sourceUnit.pokemon.OnEndTurn();
        yield return ShowStatusChanges(sourceUnit.pokemon);
        yield return sourceUnit.HUD.UpdateHP(currentHP);

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit,MoveBase moveBase)
    {
        //This is here incase the pokemon hits itself in confusion for the smooth animation
        int previousHP = sourceUnit.pokemon.currentHitPoints;

        bool canUseMove = sourceUnit.pokemon.OnBeforeMove();
        if(canUseMove == false)
        {
            yield return ShowStatusChanges(sourceUnit.pokemon);
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

        yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} used {moveBase.MoveName}");

        if(targetUnit.pokemon.currentHitPoints <=0 && moveBase.Target == MoveTarget.Foe)
        {
            yield return dialogBox.TypeDialog($"There is no target Pokemon");
            yield break;
        }
        //Moved here so it shows an attack animation, just skips out on the pokemon recieving the hit animation
        yield return sourceUnit.PlayAttackAnimation();

        if (CheckIfMoveHits(moveBase,sourceUnit.pokemon,targetUnit.pokemon) == true)
        {
            if (moveBase.MoveType == MoveType.Status)
            {
                yield return RunMoveEffects(moveBase.MoveEffects, sourceUnit, targetUnit,moveBase.Target);
            }
            else
            {
                int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP
                DamageDetails damageDetails = targetUnit.pokemon.TakeDamage(moveBase, sourceUnit.pokemon);

                if (moveBase.Target == MoveTarget.Foe && damageDetails.typeEffectiveness == 0)
                {
                    targetUnit.PlayHitAnimation();
                }

                yield return targetUnit.HUD.UpdateHP(hpPriorToAttack);
                yield return ShowDamageDetails(damageDetails, targetUnit);

                //Shows that the damage does not effect them and then ends the move right there
                if(damageDetails.typeEffectiveness == 0)
                {
                    yield break;
                }
            }

            if(moveBase.SecondaryEffects != null && moveBase.SecondaryEffects.Count > 0 && targetUnit.pokemon.currentHitPoints > 0)
            {
                foreach (var secondaryEffect in moveBase.SecondaryEffects)
                {
                    int rnd = Random.Range(1, 101);
                    if(rnd <= secondaryEffect.PercentChance)
                    {
                        yield return RunMoveEffects(secondaryEffect, sourceUnit, targetUnit, secondaryEffect.Target,true);

                        if(secondaryEffect.Volatiletatus == ConditionID.cursedUser)
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
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName}'s attack missed!");
        }

        if (targetUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(targetUnit);
        }
    }

    bool CheckIfMoveHits(MoveBase move,Pokemon source,Pokemon target)
    {
        if(move.AlwaysHits == true)
        {
            return true;
        }

        float moveAccuracy = move.MoveAccuracy;

        moveAccuracy *= source.accuracy;

        float targetEvasion = target.evasion;

        if (source.ability?.IgnoreStatIncreases != null)
        {
            if (source.ability.IgnoreStatIncreases.Invoke(StatAttribute.Evasion) == true && targetEvasion > 1)
            {
                targetEvasion = 1;
            }
        }
        moveAccuracy /= targetEvasion;

        return Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator PokemonHasFainted(BattleUnit targetBattleUnit)
    {
        if (targetBattleUnit.pokemon.currentHitPoints <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetBattleUnit.pokemon.currentName} has fainted");
            targetBattleUnit.PlayFaintAnimation();
            
            if(targetBattleUnit.isPlayerPokemon == false)
            {
                int expYield = targetBattleUnit.pokemon.pokemonBase.RewardedExperienceYield;
                int level = targetBattleUnit.pokemon.currentLevel;
                float trainerBonus = (_isTrainerBattle==true) ? 1.5f : 1;
                float pokemonSharingExp = 1;

                if(targetBattleUnit.GetListOfPokemonBattledAgainst.Count > 1)
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
                    if(sharing > 1)
                    {
                        pokemonSharingExp = sharing;
                    }
                }

                int expGained = Mathf.FloorToInt(expYield * level * trainerBonus / pokemonSharingExp) / 7;

                foreach (Pokemon pokemon in targetBattleUnit.GetListOfPokemonBattledAgainst)
                {
                    if(pokemon == playerBattleUnit.pokemon)
                    {
                        yield return GainExperience(playerBattleUnit,expGained);
                    }
                    else
                    {
                        yield return GainExperience(pokemon, expGained);
                    }
                    pokemon.GainEffortValue(targetBattleUnit.pokemon.pokemonBase.rewardedEfforValue);
                }
            }

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetBattleUnit);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.isPlayerPokemon)
        {
            Pokemon nextPokemon = _playerParty.GetFirstHealthyPokemon();
            if (nextPokemon != null)
            {
                faintedUnit.SendOutPokemonOnTurnEnd = true;
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            if(_isTrainerBattle == true)
            {
                Pokemon nextPokemon = _trainerParty.GetFirstHealthyPokemon();
                if (nextPokemon != null)
                {
                    faintedUnit.SendOutPokemonOnTurnEnd = true;
                }
                else
                {
                    OnBattleOver(true);
                }
            }
            else
            {
                OnBattleOver(true);
            }
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails,BattleUnit battleUnit)
    {
        if(damageDetails.criticalHit > 1f)
        {
            yield return dialogBox.TypeDialog("A Critical Hit!");
        }

        if(damageDetails.typeEffectiveness == 0)
        {
            yield return dialogBox.TypeDialog($"It doesnt effect {battleUnit.pokemon.currentName}");
        }
        else if(damageDetails.typeEffectiveness <= 0.5f)
        {
            yield return dialogBox.TypeDialog($"It's not very effective");
        }
        else if(damageDetails.typeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"It's super effective!");
        }
    }

    public Pokemon GetCurrentPokemonInBattle
    {
        get { return playerBattleUnit.pokemon; }
    }

    IEnumerator TrainerAboutToUsePokemonFeature(Pokemon nextPokemon)
    {
        yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} is about to use {nextPokemon.pokemonBase.GetPokedexName()}", true);

        yield return dialogBox.TypeDialog($"Will {_playerController.TrainerName} change Pokemon?");

        yield return dialogBox.SetChoiceBox(() => 
        {
            OpenPokemonParty(true,true);
        }
        , ()=> 
        {
            _playerPokemonShift = false;
        });

        yield return SwitchPokemonIEnumerator(enemyBattleUnit, nextPokemon);
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
            _playerParty.SwitchPokemonPositions(battleUnit.pokemon, newPokemon);
            enemyBattleUnit.AddPokemonToBattleList(newPokemon);
        }

        battleUnit.Setup(newPokemon,false,true);

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
            yield return RunThroughTurns(_currentTurnDetails);
        }
    }

    public void ReturnFromPokemonPartySystem()
    {
        actionSelectionEventSelector.SelectBox();
    }

    IEnumerator RunMoveEffects(MoveEffects effects, BattleUnit source, BattleUnit target,MoveTarget moveTarget,bool wasSecondaryEffect = false)
    {
        if (effects.Boosts != null)
        {
            yield return ApplyStatChanges(effects.Boosts, source, target, moveTarget);
        }

        //Status Condition
        if(effects.Status != ConditionID.NA)
        {
            target.pokemon.SetStatus(effects.Status,wasSecondaryEffect);
        }

        //Volatile Status Condition
        if (effects.Volatiletatus != ConditionID.NA)
        {
            if(moveTarget == MoveTarget.Foe)
            {
                target.pokemon.SetVolatileStatus(effects.Volatiletatus);
            }
            else
            {
                source.pokemon.SetVolatileStatus(effects.Volatiletatus);
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
                List<EntryHazard> currentEntrySide = (target.isPlayerPokemon) ? _playerSideEntryHazards: _enemySideEntryHazards;
                EntryHazard currentHazard = EntryHazardsDB.EntryHazards[effects.EntryHazard];

                if (currentEntrySide.Contains(currentHazard) == true)
                {
                    currentHazard = currentEntrySide.Find(x => x.Id == effects.EntryHazard);
                    if (currentHazard.CanBeUsed() == false)
                    {
                        yield return dialogBox.TypeDialog("But it failed");
                        yield break;
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog(currentHazard?.StartMessage(target));
                        currentHazard?.OnStart?.Invoke(currentHazard);
                    }
                }
                else
                {
                    currentEntrySide.Add(currentHazard);
                    yield return dialogBox.TypeDialog(currentHazard?.StartMessage(target));
                    currentHazard?.OnStart?.Invoke(currentHazard);
                }
            }
            else
            {
                Debug.Log($"Entry hazard is trying to be set on own side of field {effects}");
            }
        }

        yield return ShowStatusChanges(source.pokemon);
        yield return ShowStatusChanges(target.pokemon);
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while(pokemon.statusChanges.Count > 0)
        {
            string message = pokemon.statusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    #region Entry Hazards

    IEnumerator ApplyEntryHazardOnSentOut(BattleUnit target)
    {
        if (target.pokemon.currentHitPoints <= 0)
        {
            yield break;
        }

        List<EntryHazard> currentEntrySide = (target.isPlayerPokemon) ? _playerSideEntryHazards : _enemySideEntryHazards;

        foreach (EntryHazard entryHazard in currentEntrySide)
        {
            int currentHP = target.pokemon.currentHitPoints;
            if (entryHazard?.OnEntry != null)
            {
                entryHazard?.OnEntry?.Invoke(target.pokemon);
            }

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

    IEnumerator ShowWeatherEffect(WeatherEffect weather)
    {
        string message = weather?.OnEndTurn?.Invoke(this);
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
                    yield return dialogBox.TypeDialog("But it failed");
                }
                yield break;
            }
        }

        _currentWeather = WeatherEffectDB.WeatherEffects[weatherID];
        if (_currentWeather.StartMessage != null)
        {
            weatherDuration = _currentWeather.OnStartDuration;
            yield return dialogBox.TypeDialog(_currentWeather?.StartMessage);
        }
    }

    public void RemoveWeatherEffect()
    {
        _currentWeather = null;
    }

    IEnumerator ApplyWeatherEffectsOnEndTurn(WeatherEffect weather,BattleUnit sourceUnit)
    {
        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield break;
        }

        if(weather?.OnEndTurnDamage == null)
        {
            yield break;
        }

        int currentHP = sourceUnit.pokemon.currentHitPoints;

        weather?.OnEndTurnDamage?.Invoke(sourceUnit.pokemon);
        yield return ShowStatusChanges(sourceUnit.pokemon);

        if(currentHP != sourceUnit.pokemon.currentHitPoints)
        {
            sourceUnit.PlayHitAnimation();
        }

        yield return sourceUnit.HUD.UpdateHP(currentHP);

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
        }
    }

    IEnumerator PlayerThrewPokeball(BattleUnit targetUnit)
    {
        if(_isTrainerBattle == true)
        {
            EnableActionSelector(false);
            yield return dialogBox.TypeDialog($"You cant capture Trainers Pokemon", true);
            PlayerActions();
            yield break;
        }

        yield return dialogBox.TypeDialog($"{_playerController.TrainerName} used Pokeball");

        inGameItem.SetActive(true);
        InBattleItem currentBall = inGameItem.GetComponent<InBattleItem>();
        Vector3 ballHeightJump = new Vector3(0, 75, 0);

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
        int shakeCount = CatchingMechanics.CatchRate(targetUnit.pokemon, currentBall.currentPokeball);

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
            OnPokemonCaptured(enemyBattleUnit.pokemon);
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
                yield return dialogBox.TypeDialog($"Oh no! The Pok�mon broke free!",true);
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

        if(playerSpeed > enemySpeed)
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
            if(rnd < f)
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

            List<LearnableMove> newMove = targetUnit.pokemon.GetLeranableMoveAtCurrentLevel();

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

            List<LearnableMove> newMove = pokemon.GetLeranableMoveAtCurrentLevel();

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

    IEnumerator ActivatePokemonAbilityUponEntry(BattleUnit sourceUnit,BattleUnit targetUnit)
    {
        if(sourceUnit.pokemon.ability == null)
        {
            yield break;
        }

        if(sourceUnit.pokemon.ability.OnStartWeatherEffect != WeatherEffectID.NA)
        {
            yield return StartWeatherEffect(sourceUnit.pokemon.ability.OnStartWeatherEffect, true);
            yield break;
        }

        if(sourceUnit.pokemon.ability?.OnStartLowerStat != null)
        {
            List<StatBoost> abilityStatBoosts = new List<StatBoost>() { sourceUnit.pokemon.ability.OnStartLowerStat };
            yield return ApplyStatChanges(abilityStatBoosts, sourceUnit, targetUnit, MoveTarget.Foe);
        }
    }

    IEnumerator ApplyStatChanges(List<StatBoost> statBoosts,BattleUnit sourceUnit,BattleUnit targetUnit,MoveTarget moveTarget)
    {
        //Check to see if one is up and one is down
        List<StatBoost> positiveEffects = new List<StatBoost>();
        List<StatBoost> negativeEffects = new List<StatBoost>();

        foreach (StatBoost currentStat in statBoosts)
        {
            if (currentStat.boost == 0)
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

        if (moveTarget == MoveTarget.Foe)
        {
            targetUnit.pokemon.ApplyStatModifier(positiveEffects);
            yield return targetUnit.ShowStatChanges(positiveEffects, true);
            yield return ShowStatusChanges(targetUnit.pokemon);

            if (targetUnit.pokemon.ability?.PreventStatFromBeingLowered != null)
            {
                bool abilityActivated = false;//This is for multiple stats prevented from being lowered so it doesnt stack the Queue

                List<StatBoost> negativeEffectsCopy = new List<StatBoost>(negativeEffects);
                foreach (StatBoost stat in negativeEffectsCopy)
                {
                    if (stat.boost < 0)
                    {
                        bool? negated = targetUnit.pokemon.ability.PreventStatFromBeingLowered.Invoke(stat.stat);
                        if (negated == true)
                        {
                            if (abilityActivated == false)
                            {
                                targetUnit.pokemon.statusChanges.Enqueue(targetUnit.pokemon.ability.OnAbilitityActivation(targetUnit.pokemon));
                                abilityActivated = true;
                            }
                            negativeEffects.Remove(stat);
                        }
                    }
                }
            }

            targetUnit.pokemon.ApplyStatModifier(negativeEffects);
            yield return targetUnit.ShowStatChanges(negativeEffects, false);
            yield return ShowStatusChanges(targetUnit.pokemon);
        }
        else if (moveTarget == MoveTarget.Self)
        {
            sourceUnit.pokemon.ApplyStatModifier(positiveEffects);
            yield return sourceUnit.ShowStatChanges(positiveEffects, true);
            yield return ShowStatusChanges(sourceUnit.pokemon);

            sourceUnit.pokemon.ApplyStatModifier(negativeEffects);
            yield return sourceUnit.ShowStatChanges(negativeEffects, false);
            yield return ShowStatusChanges(sourceUnit.pokemon);
        }
    }

    public static WeatherEffectID GetCurrentWeather
    {
        get
        {
            if(_currentWeather != null)
            {
                return _currentWeather.Id;
            }
            return WeatherEffectID.NA;
        }
    }
}
