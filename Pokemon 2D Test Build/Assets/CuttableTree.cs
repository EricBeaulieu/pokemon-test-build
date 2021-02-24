using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableTree : MonoBehaviour
{

    //Called in Animator
    public void OnCutFinish()
    {
        //Add to object pool
        Destroy(this);
    }
}
