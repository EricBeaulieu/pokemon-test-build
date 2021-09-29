using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrary : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Contrary; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Contrary(); }
    public override string Description()
    {
        return "Makes stat changes have an opposite effect.";
    }
    public override bool StatChangesHaveOppositeEffect()
    {
        return true;
    }
}
