using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestinationIdentifier { A,B,C,D,E}
public class Portal : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameSceneBaseSO alternativeScene;
    [SerializeField] bool exitOnly;
    [SerializeField] DestinationIdentifier destinationIdentifier;

    //This is to prevent the player going in and out if they cannot move ontop of it
    public bool canPlayerPassThrough { get; private set; } = true;

    public GameSceneBaseSO AlternativeScene
    {
        get { return alternativeScene; }
    }

    public DestinationIdentifier MatchingIdentifier
    {
        get { return destinationIdentifier; }
    }

    public Vector3 SpawnPoint
    {
        get { return spawnPoint.position; }
    }

    public void PlayerPassedThroughPortal()
    {
        canPlayerPassThrough = false;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canPlayerPassThrough = true;
        }
    }
}
