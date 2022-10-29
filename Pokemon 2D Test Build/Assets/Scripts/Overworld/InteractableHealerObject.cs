using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHealerObject : MonoBehaviour, IInteractable
{
    [Tooltip("The player can only interact with this from the specified direction")]
    [SerializeField] bool specifiedDirectionOnly = false;
    [Tooltip("Only works when Specified Direction Only is true")]
    [SerializeField] FacingDirections interactablesDirection;
    [SerializeField]
    Dialog startingDialog = new Dialog(new List<string>()
    {
        "What a nice comfy bed",
        "Would you like to sleep here"
    });

    [SerializeField]
    Dialog yesSelectedDialog;
    [SerializeField] Dialog noSelectedDialog;
    DialogManager dialogManager;

    void Awake()
    {
        dialogManager = GameManager.instance.GetDialogSystem;
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        if (specifiedDirectionOnly == true)
        {
            Vector2 curDir = new Vector2(Mathf.Round(transform.position.x - vector2.x), Mathf.Round(transform.position.y - vector2.y));

            if (GlobalTools.CurrentDirectionFacing((Vector2)transform.position, vector2) != interactablesDirection)
            {
                yield break;
            }
        }

        GameManager.SetGameState(GameState.Dialog);
        dialogManager.ActivateDialog(true);
        for (int i = 0; i < startingDialog.Lines.Count; i++)
        {
            yield return dialogManager.TypeDialog(startingDialog.Lines[i]);
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
            for (int i = 0; i < yesSelectedDialog.Lines.Count; i++)
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
