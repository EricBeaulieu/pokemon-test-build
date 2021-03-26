using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerBattleUnit;
    [SerializeField] BattleUnit enemyBattleUnit;
    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;

    public event Action<bool> OnBattleOver;
    public event Action<bool> OpenPokemonParty;

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

    WeatherEffect _currentWeather;
    public int weatherDuration {get; set;}

    List<EntryHazard> _playerSideEntryHazards = new List<EntryHazard>();
    List<EntryHazard> _enemySideEntryHazards = new List<EntryHazard>();

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

    public void StartBattle(PokemonParty playerParty,Pokemon wildPokemon)
    {
        _playerParty = playerParty;
        _wildPokemon = wildPokemon;
        _isTrainerBattle = false;
        StartCoroutine(SetupBattle());
        _currentTurnDetails = new List<TurnAttackDetails>();
    }

    public void StartBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        _playerParty = playerParty;
        _trainerParty = trainerParty;
        _playerController = playerParty.GetComponent<PlayerController>();
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
        if(_isTrainerBattle == true)
        {
            playerBattleUnit.gameObject.SetActive(false);
            enemyBattleUnit.gameObject.SetActive(false);

            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);

            playerImage.sprite = _playerController.BackBattleSprite[0];
            trainerImage.sprite = _trainerController.FrontBattleSprite[0];

            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} wants to battle!",true);

            enemyBattleUnit.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(false);
            Pokemon enemyPokemon = _trainerParty.GetFirstHealthyPokemon();
            enemyBattleUnit.Setup(enemyPokemon);
            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} sent out {enemyPokemon.currentName}");

            yield return new WaitForSeconds(0.5f);

            playerBattleUnit.gameObject.SetActive(true);
            playerImage.gameObject.SetActive(false);
            Pokemon playerPokemon = _playerParty.GetFirstHealthyPokemon();
            playerBattleUnit.Setup(playerPokemon);
            yield return dialogBox.TypeDialog($"Go {playerPokemon.currentName}");

            yield return new WaitForSeconds(0.5f);
        }
        else //WildPokemon
        {
            playerBattleUnit.Setup(_playerParty.GetFirstHealthyPokemon());
            enemyBattleUnit.Setup(_wildPokemon);

            yield return dialogBox.TypeDialog($"A wild {enemyBattleUnit.pokemon.currentName} has appeared!");
        }

        dialogBox.BattleStartSetup();
        attackSelectionEventSelector.SetMovesList(playerBattleUnit,playerBattleUnit.pokemon.moves,this);

        SetupPlayerActions();
        PlayerActions();
    }

    #region Player Actions

    void SetupPlayerActions()
    {
        actionSelectionEventSelector.SetUp();
        actionSelectionEventSelector.ReturnFightButton().onClick.AddListener(delegate { PlayerActionFight(); });
        actionSelectionEventSelector.ReturnPokemonButton().onClick.AddListener(delegate { PlayerActionPokemon(); });
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
        OpenPokemonParty(true);
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
            OpenPokemonParty(true);
        }
        else
        {
            if(enemyBattleUnit.SendOutPokemonOnTurnEnd == true)
            {
                //This will be updated with a better AI later
                Pokemon nextEnemyPokemon = _trainerParty.GetFirstHealthyPokemon();

                yield return SwitchPokemonIEnumerator(enemyBattleUnit, nextEnemyPokemon);
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
        //This is here incase the pokemon has a status effect that ended such as being frozen
        yield return ShowStatusChanges(sourceUnit.pokemon);

        yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} used {moveBase.MoveName}");

        if(targetUnit.pokemon.currentHitPoints <=0 && moveBase.Target == MoveTarget.Foe)
        {
            yield return dialogBox.TypeDialog($"There is no target Pokemon");
            yield break;
        }

        if(CheckIfMoveHits(moveBase,sourceUnit.pokemon,targetUnit.pokemon) == true)
        {
            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            if(moveBase.Target == MoveTarget.Foe)
            {
                targetUnit.PlayHitAnimation();
            }

            if (moveBase.MoveType == MoveType.Status)
            {
                yield return RunMoveEffects(moveBase.MoveEffects, sourceUnit, targetUnit,moveBase.Target);
            }
            else
            {
                int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP
                DamageDetails damageDetails = targetUnit.pokemon.TakeDamage(moveBase, sourceUnit.pokemon);

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
                        yield return RunMoveEffects(secondaryEffect, sourceUnit, targetUnit, secondaryEffect.Target);

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
        moveAccuracy /= target.evasion;

        return Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator PokemonHasFainted(BattleUnit targetBattleUnit)
    {
        if (targetBattleUnit.pokemon.currentHitPoints <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetBattleUnit.pokemon.currentName} has fainted");
            targetBattleUnit.PlayFaintAnimation();
            //apply experience here

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

    public void PlayerSwitchPokemon(Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemonIEnumerator(playerBattleUnit,newPokemon));
        EnableActionSelector(false);

        if (enemyBattleUnit.SendOutPokemonOnTurnEnd == true)
        {
            //This will be updated with a better AI later
            Pokemon nextEnemyPokemon = _trainerParty.GetFirstHealthyPokemon();

            StartCoroutine(SwitchPokemonIEnumerator(enemyBattleUnit, nextEnemyPokemon));
        }
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

        battleUnit.Setup(newPokemon);

        if(battleUnit.isPlayerPokemon == true)
        {
            attackSelectionEventSelector.SetMovesList(battleUnit, battleUnit.pokemon.moves, this);
            yield return dialogBox.TypeDialog($"Go {battleUnit.pokemon.currentName}!");
        }
        else
        {
            yield return dialogBox.TypeDialog($"{_trainerController.TrainerName} sent out {battleUnit.pokemon.currentName}");
        }

        yield return ApplyEntryHazardOnSentOut(battleUnit);

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

    IEnumerator RunMoveEffects(MoveEffects effects, BattleUnit source, BattleUnit target,MoveTarget moveTarget)
    {
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                target.pokemon.ApplyStatModifier(effects.Boosts);
            }
            else if (moveTarget == MoveTarget.Self)
            {
                source.pokemon.ApplyStatModifier(effects.Boosts);
            }
        }

        //Status Condition
        if(effects.Status != ConditionID.NA)
        {
            target.pokemon.SetStatus(effects.Status);
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
            if (_currentWeather != null)
            {
                if (_currentWeather.Id == effects.WeatherEffect)
                {
                    yield return dialogBox.TypeDialog("But it failed");
                    yield break;
                }
            }

            _currentWeather = WeatherEffectDB.WeatherEffects[effects.WeatherEffect];
            if (_currentWeather.StartMessage != null)
            {
                weatherDuration = _currentWeather.OnStartDuration;
                yield return dialogBox.TypeDialog(_currentWeather?.StartMessage);
            }
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
}
