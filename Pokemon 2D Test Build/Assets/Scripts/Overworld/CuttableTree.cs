using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SaveableEntity))]
public class CuttableTree : MonoBehaviour,IInteractable, ISaveable
{
    SpriteRenderer spriteRenderer;
    const string standardMessage = "This tree looks like it can be cut down!";
    const string cutAvailable = "Would you like to cut it?";

    [SerializeField] Sprite standardTree;
    [SerializeField] List<Sprite> treeCutAnimation;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.SnapToGrid();
    }

    public void RestoreTree()
    {
        if(gameObject.activeInHierarchy == true)
        {
            return;
        }

        Debug.Log("help", gameObject);

        spriteRenderer.sprite = standardTree;
        gameObject.SetActive(true);
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        Pokemon pokemon = GameManager.instance.GetPlayerController.pokemonParty.ContainsMove("Cut");
        DialogManager dialogManager = GameManager.instance.GetDialogSystem;

        if(pokemon == null)
        {
            yield return dialogManager.ShowDialogBox(new Dialog(standardMessage));
        }
        else
        {
            bool cutUsed = false;
            string pokemonUse = $"{pokemon.currentName} used cut!";
            dialogManager.ActivateDialog(true);
            yield return dialogManager.TypeDialog(standardMessage,true);
            yield return dialogManager.TypeDialog(cutAvailable);
            yield return dialogManager.SetChoiceBox(() =>
            {
                cutUsed = true;
            });

            if(cutUsed == true)
            {
                yield return dialogManager.TypeDialog(pokemonUse);
                dialogManager.ActivateDialog(false);
                GameManager.SetGameState(GameState.Dialog);
                yield return GameManager.instance.GetPlayerController.PlayHMAnimation(pokemon);
                yield return CutAnimation();
                gameObject.SetActive(false);
                GameManager.SetGameState(GameState.Overworld);
            }
            else
            {
                dialogManager.ActivateDialog(false);
            }
        }
    }

    IEnumerator CutAnimation()
    {
        for (int i = 0; i < treeCutAnimation.Count; i++)
        {
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.sprite = treeCutAnimation[i];
        }
        yield return new WaitForSeconds(0.25f);
    }

    public object CaptureState(bool playerSave = false)
    {
        return new Tree { treeIsCutInLevel = (gameObject.activeInHierarchy) };
    }

    public void RestoreState(object state)
    {
        var saveData = (Tree)state;
        gameObject.SetActive(saveData.treeIsCutInLevel);
    }

    [Serializable]
    struct Tree
    {
        public bool treeIsCutInLevel;
    }
}
