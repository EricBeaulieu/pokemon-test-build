using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttableTree : MonoBehaviour,IInteractable, ISaveable
{
    SpriteRenderer spriteRenderer;
    const string standardMessage = "This tree looks like it can be cut down!";
    const string cutAvailable = "Would you like to cut it?";

    [SerializeField] List<Sprite> treeCutAnimation;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GlobalTools.SnapToGrid(transform.position);
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        Pokemon pokemon = GameManager.instance.GetPlayerController.pokemonParty.ContainsCut();
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
                yield return CutAnimation();

                gameObject.SetActive(false);
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

    public object CaptureState()
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
