using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonSaveData
{
    public string currentBase;
    public int currentLevel;
    public int currentExp;
    public int currentHitPoints;
    public List<MoveSaveData> currentMoves;
    public bool isShiny;
    public bool? currentGender;
    public string currentNature;
    public IndividualValues currentIndividualValues;
    public EffortValues currentEffortValues;
    public string currentNickname = "";
    public AbilityID currentAbilityID;
    public ConditionID currentCondition = ConditionID.NA;
    public string currentOT;
    public string currentOTId;
    public string currentPokeball;
    public string currentItem = null;
}
