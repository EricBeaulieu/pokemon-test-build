using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestinationIdentifier { A,B,C,D,E,F}
public class Portal : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    public Vector3 SpawnPoint { get { return spawnPoint.position; } }
    [SerializeField] GameSceneBaseSO alternativeScene;
    public GameSceneBaseSO AlternativeScene { get { return alternativeScene; } }
    [SerializeField] bool exitOnly;
    [SerializeField] DestinationIdentifier destinationIdentifier;
    [SerializeField] FadeStyle fadeStyle;
    public FadeStyle FadeStyle { get { return fadeStyle; } }
    [SerializeField] bool faceDirectionAfterMoving;
    public bool FaceDirectionAfterMoving { get { return faceDirectionAfterMoving; } }
    [SerializeField] FacingDirections direction;
    public FacingDirections DirectionUponExiting { get { return direction; } }

    //This is to prevent the player going in and out if they cannot move ontop of it
    public bool canPlayerPassThrough { get; protected set; } = true;

    public DestinationIdentifier MatchingIdentifier
    {
        get { return destinationIdentifier; }
    }

    public void PlayerPassedThroughPortal()
    {
        canPlayerPassThrough = (spawnPoint.transform.localPosition != Vector3.zero);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Portal Called");
            canPlayerPassThrough = true;
        }
    }
}
