using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PokemonSavingSystem
{
    static string split = "*";
    static string pokemonData;
    static string empty = "_";
    static string[] splitArray;
    static int indexPos;

    public static void SavePokemon(Pokemon pokemon, string location)
    {
        ResetPokemonData();

        if(pokemon == null)
        {
            PlayerPrefs.SetString(location, empty);
            return;
        }

        AddData(GetAssetPath(pokemon.pokemonBase));
        AddData(pokemon.currentLevel.ToString());
        AddData(pokemon.currentExp.ToString());
        AddData(pokemon.currentHitPoints.ToString());

        for (int i = 0; i < PokemonBase.MAX_NUMBER_OF_MOVES; i++)
        {
            if (i < pokemon.moves.Count)
            {
                AddData(GetAssetPath(pokemon.moves[i].moveBase));
                AddData(pokemon.moves[i].pP.ToString());
            }
            else
            {
                AddData(empty);//move
                AddData(empty);//PP
            }
        }

        AddData(pokemon.isShiny.ToString());

        int genderPos = (int)pokemon.gender;
        AddData(genderPos.ToString());

        AddData(GetAssetPath(pokemon.nature));

        for (int i = 0; i < IndividualValues.VALUES_SAVED_LENGTH; i++)
        {
            AddData(pokemon.individualValues.ReturnValueAtIndex(i).ToString());
        }

        for (int i = 0; i < EffortValues.VALUES_SAVED_LENGTH; i++)
        {
            AddData(pokemon.effortValues.ReturnValueAtIndex(i).ToString());
        }

        if (pokemon.currentName == pokemon.pokemonBase.GetPokedexName())
        {
            AddData(empty);
        }
        else
        {
            AddData(pokemon.currentName);
        }

        int abilityID = (int)pokemon.ability.Id;
        AddData(abilityID.ToString());
        if (pokemon.status == null)
        {
            AddData(empty);
        }
        else
        {
            int conditionID = (int)pokemon.status.Id;
            AddData(conditionID.ToString());
        }
        AddData(pokemon.originalTrainer);
        AddData(pokemon.originalTrainerID);
        AddData(GetAssetPath(pokemon.pokeballCapturedIn));

        Debug.Log($"Location:{location}\n{pokemonData}");

        PlayerPrefs.SetString(location, pokemonData);
    }

    public static Pokemon LoadPokemon(string location)
    {
        StartLoadData(location);

        if(pokemonData == empty)
        {
            return null;
        }

        PokemonSaveData pokemonSaveData = new PokemonSaveData();

        pokemonSaveData.currentBase = GetPokemonBase(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentLevel = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentExp = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentHitPoints = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentMoves = new List<Move>();
        for (int i = 0; i < PokemonBase.MAX_NUMBER_OF_MOVES; i++)
        {
            if (pokemonData == empty)
            {
                //PP
                pokemonData = NextLoadIndex();
                //Next Move
                pokemonData = NextLoadIndex();
                continue;
            }

            Move move = new Move(GetMoveBase(pokemonData));
            pokemonData = NextLoadIndex();
            move.pP = int.Parse(pokemonData);
            pokemonData = NextLoadIndex();
            pokemonSaveData.currentMoves.Add(move);
        }

        pokemonSaveData.isShiny = bool.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentGender = (Gender)int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentNature = GetNature(pokemonData);
        pokemonData = NextLoadIndex();

        int hp = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        int att = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        int def = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        int spAtt = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        int spDef = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        int spd = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        IndividualValues currentIV = new IndividualValues();
        currentIV.LoadValues(hp, att, def, spAtt, spDef, spd);
        pokemonSaveData.currentIndividualValues = currentIV;

        hp = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        att = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        def = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        spAtt = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        spDef = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        spd = int.Parse(pokemonData);
        pokemonData = NextLoadIndex();

        EffortValues currentEV = new EffortValues();
        currentEV.LoadValues(hp, att, def, spAtt, spDef, spd);
        pokemonSaveData.currentEffortValues = currentEV;

        if (pokemonData != empty)
        {
            pokemonSaveData.currentNickname = pokemonData;
        }
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentAbilityID = (AbilityID)int.Parse(pokemonData);
        pokemonData = NextLoadIndex();
        
        if (pokemonData != empty)
        {
            pokemonSaveData.currentCondition = (ConditionID)int.Parse(pokemonData);
        }
        pokemonData = NextLoadIndex();

        pokemonSaveData.currentOT = pokemonData;
        pokemonData = NextLoadIndex();
        pokemonSaveData.currentOTId = pokemonData;
        pokemonData = NextLoadIndex();
        pokemonSaveData.currentPokeball = GetPokeball(pokemonData);

        return new Pokemon(pokemonSaveData);
    }

    static void ResetPokemonData()
    {
        pokemonData = "";
    }

    static void AddData(string data)
    {
        pokemonData += data;
        pokemonData += split;
    }

    static void StartLoadData(string location)
    {
        ResetPokemonData();

        location = PlayerPrefs.GetString(location);

        splitArray = location.Split(char.Parse(split));
        indexPos = 0;

        pokemonData = splitArray[indexPos];
    }

    static string NextLoadIndex()
    {
        indexPos++;
        return splitArray[indexPos];
    }

    static PokemonBase GetPokemonBase(string path)
    {
        return Resources.Load<PokemonBase>(path);
    }

    static MoveBase GetMoveBase(string path)
    {
        return Resources.Load<MoveBase>(path);
    }

    static NatureBase GetNature(string path)
    {
        return Resources.Load<NatureBase>(path);
    }

    static PokeballItem GetPokeball(string path)
    {
        return Resources.Load<PokeballItem>(path);
    }

    static string GetAssetPath(Object obj)
    {
        string currentPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        currentPath = currentPath.Replace("Assets/Resources/", string.Empty);
        return currentPath.Remove(currentPath.Length - 6);
    }
}
