using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSaveData
{
    public PokemonBase currentBase;
    public int currentLevel;
    public int currentExp;
    public int currentHitPoints;
    public List<Move> currentMoves;
    public bool isShiny;
    public Gender currentGender;
    public NatureBase currentNature;
    public IndividualValues currentIndividualValues;
    public EffortValues currentEffortValues;
    public string currentNickname = "";
    public AbilityID currentAbilityID;
    public ConditionID currentCondition = ConditionID.NA;
    public string currentOT;
    public string currentOTId;
    public PokeballItem currentPokeball;
    public ItemBase currentItem = null;
}
