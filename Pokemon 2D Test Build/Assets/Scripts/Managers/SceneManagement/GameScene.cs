using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

[CreateAssetMenu(menuName = "Level/Create New Game Scene")]
public class GameScene : ScriptableObject
{
    [SerializeField] object sceneToLoad;
}
