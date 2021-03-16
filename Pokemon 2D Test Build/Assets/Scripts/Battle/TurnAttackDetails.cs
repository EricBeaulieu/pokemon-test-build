using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAttackDetails
{
    public MoveBase currentMove { get; set; }
    public BattleUnit attackingPokemon { get; set; }
    public BattleUnit targetPokmeon { get; set; }
}
