using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerNpcController : EntityAI, IInteractable
{
    [Header("Healer Controller")]
    [SerializeField]
    Dialog greetingDialog = new Dialog(new List<string>()
    {
        "Welcome to our Pokemon Center",
        "Would you like me to heal your Pokemon Back to perfect Health?"
    });

    [SerializeField]
    Dialog yesSelectedDialog = new Dialog(new List<string>()
    {
        "Okay, I'll take your Pokemon for a few second's",
        "Thank you for waiting. We've Restored your Pokemon to full health",
        "We Hope to see you again!"
    });
    [SerializeField] Dialog noSelectedDialog = new Dialog("We Hope to see you again");
    DialogManager dialogManager;

    void Awake()
    {
        base.Initialization();
        dialogManager = GameManager.instance.GetDialogSystem;
    }

    public override void HandleUpdate()
    {

    }

    public IEnumerator OnInteract(Vector2 initiator)
    {        
        GameManager.SetGameState(GameState.Dialog);
        FaceTowardsDirection(initiator);
        dialogManager.ActivateDialog(true);
        for (int i = 0; i < greetingDialog.Lines.Count; i++)
        {
            yield return dialogManager.TypeDialog(greetingDialog.Lines[i]);
        }

        bool playerSelection = false;
        yield return GameManager.instance.GetDialogSystem.SetChoiceBox(() =>
        {
            playerSelection = true;
        }, () =>
        {
            playerSelection = false;
        });

        if (playerSelection == true)
        {
            yield return dialogManager.TypeDialog(yesSelectedDialog.Lines[0]);
            GameManager.instance.GetPlayerController.pokemonParty.HealAllPokemonInParty();

            for (int i = 1; i < yesSelectedDialog.Lines.Count; i++)
            {
                yield return dialogManager.TypeDialog(yesSelectedDialog.Lines[i]);
            }
        }
        else
        {
            for (int i = 0; i < noSelectedDialog.Lines.Count; i++)
            {
                yield return dialogManager.TypeDialog(noSelectedDialog.Lines[i]);
            }
        }

        dialogManager.ActivateDialog(false);
        GameManager.SetGameState(GameState.Overworld);
    }
}
