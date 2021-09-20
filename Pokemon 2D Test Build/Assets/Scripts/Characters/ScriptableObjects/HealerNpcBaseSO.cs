using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Healer")]
public class HealerNpcBaseSO : EntityBaseSO
{
    [SerializeField] Dialog greetingDialog = new Dialog(new List<string>()
    {
        "Welcome to our Pokemon Center",
        "Would you like me to heal your Pokemon Back to perfect Health?"
    });

    [SerializeField] Dialog yesSelectedDialog = new Dialog(new List<string>()
    {
        "Okay, I'll take your Pokemon for a few second's",
        "Thank you for waiting. We've Restored your Pokemon to full health",
        "We Hope to see you again!"
    });
    [SerializeField] Dialog noSelectedDialog = new Dialog("We Hope to see you again");

    public Dialog GetGreetingDialog
    {
        get { return greetingDialog; }
    }

    public Dialog GetYesSelectedDialog
    {
        get { return yesSelectedDialog; }
    }

    public Dialog GetNoSelectedDialog
    {
        get { return noSelectedDialog; }
    }
}
