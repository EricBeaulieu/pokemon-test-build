using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hustle : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Hustle; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Hustle(); }
    public override string Description()
    {
        return "Boosts the Attack stat, but lowers accuracy.";
    }
    public override float AlterStat(WeatherEffectID iD, StatAttribute statAffected)
    {
        if(statAffected == StatAttribute.Attack)
        {
            return 1.5f;
        }
        return base.AlterStat(iD, statAffected);
    }
    public override MoveBase AlterMoveDetails(MoveBase move)
    {
        if(move.MoveType == MoveType.Physical)
        {
            move = move.Clone();
            move.adjustedMoveAccuracyPercentage(-0.2f);
        }
        return base.AlterMoveDetails(move);
    }
}
