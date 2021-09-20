using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerNpcController : Entity, IInteractable
{
    [SerializeField] HealerNpcBaseSO healerBase;
    DialogManager dialogManager;

    void Awake()
    {
        base.Initialization(healerBase);
        dialogManager = GameManager.instance.GetDialogSystem;
    }

    public override void HandleUpdate()
    {

    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        if(Vector2.Distance(transform.position,initiator) > OVER_THE_COUNTER_MAX_DISTANCE)
        {
            yield break;
        }
        
        GameManager.SetGameState(GameState.Dialog);
        FaceTowardsDirection(initiator);
        dialogManager.ActivateDialog(true);
        for (int i = 0; i < healerBase.GetGreetingDialog.Lines.Count; i++)
        {
            yield return dialogManager.TypeDialog(healerBase.GetGreetingDialog.Lines[i]);
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
            yield return dialogManager.TypeDialog(healerBase.GetYesSelectedDialog.Lines[0]);
            GameManager.instance.GetPlayerController.pokemonParty.HealAllPokemonInParty();

            for (int i = 1; i < healerBase.GetYesSelectedDialog.Lines.Count; i++)
            {
                yield return dialogManager.TypeDialog(healerBase.GetYesSelectedDialog.Lines[i]);
            }
        }
        else
        {
            for (int i = 0; i < healerBase.GetNoSelectedDialog.Lines.Count; i++)
            {
                yield return dialogManager.TypeDialog(healerBase.GetNoSelectedDialog.Lines[i]);
            }
        }

        dialogManager.ActivateDialog(false);
        GameManager.SetGameState(GameState.Overworld);
    }

    public HealerNpcBaseSO GetHealerNpcBase
    {
        get { return healerBase; }
    }
}
