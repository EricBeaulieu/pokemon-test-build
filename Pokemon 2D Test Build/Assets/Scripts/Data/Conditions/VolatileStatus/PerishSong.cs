using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerishSong : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.PerishSong; } }
    public override string StartMessage(Pokemon pokemon)
    {
        StatusTime = 4;
        return base.StartMessage(pokemon);//"All Pokemon that heard the song will faint in 3 turns"
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} perish count fell to {StatusTime}");

        if (StatusTime <= 0)
        {
            pokemon.UpdateHP(pokemon.maxHitPoints);
        }
    }
}
