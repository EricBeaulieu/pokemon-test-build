using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLink : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SkillLink; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SkillLink(); }
    public override string Description()
    {
        return "Maximizes the number of times multistrike moves hit.";
    }
    public override bool MaximizeMultistrikeMovesHit()
    {
        return true;
    }
}
