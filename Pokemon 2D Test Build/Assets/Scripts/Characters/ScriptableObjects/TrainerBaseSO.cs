using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Trainer")]
public class TrainerBaseSO : EntityBaseSO
{
    [SerializeField] Dialog preBattleDialog;
    [SerializeField] Dialog inBattleDialogOnDefeat;
    [SerializeField] Dialog inBattleDialogOnVictory;
    [SerializeField] Dialog postDefeatOverworldDialog;

    [SerializeField] string trainerName;

    public override void Initialization()
    {
        base.Initialization();

        if (trainerName == null || trainerName == "")
        {
            Debug.LogError("This trainer SO is missing its trainer name", this);
        }

        if (preBattleDialog.Lines.Count <= 0)
        {
            Debug.LogError("This trainer SO is missing its preBattleDialog", this);
        }

        if (inBattleDialogOnDefeat.Lines.Count <= 0)
        {
            Debug.LogError("This trainer SO is missing its inBattleDialogOnDefeat", this);
        }

        if (inBattleDialogOnVictory.Lines.Count <= 0)
        {
            Debug.LogError("This trainer SO is missing its inBattleDialogOnVictory", this);
        }

        if (postDefeatOverworldDialog.Lines.Count <= 0)
        {
            Debug.LogError("This trainer SO is missing its postDefeatOverworldDialog", this);
        }
    }

    public Dialog GetPreBattleDialog
    {
        get { return preBattleDialog; }
    }

    public Dialog GetInBattleDialogOnDefeat
    {
        get { return inBattleDialogOnDefeat; }
    }
    public Dialog GetInBattleDialogOnVictory
    {
        get { return inBattleDialogOnVictory; }
    }
    public Dialog GetPostDefeatOverworldDialog
    {
        get { return postDefeatOverworldDialog; }
    }

    public string TrainerName
    {
        get { return trainerName; }
    }
}
