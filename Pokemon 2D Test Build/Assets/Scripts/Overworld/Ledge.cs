using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpableLedge { West,South,East}

public class Ledge : MonoBehaviour
{
    [SerializeField] JumpableLedge ledgeFacing;
}
