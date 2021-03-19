using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAttackDetails
{
    public MoveBase currentMove { get; private set; }
    public BattleUnit attackingPokemon { get; private set; }
    public BattleUnit targetPokmeon { get; private set; }

    public TurnAttackDetails(MoveBase curMove,BattleUnit attPokemon,BattleUnit tarPokemon)
    {
        currentMove = curMove;
        attackingPokemon = attPokemon;
        targetPokmeon = tarPokemon;
    }
}
