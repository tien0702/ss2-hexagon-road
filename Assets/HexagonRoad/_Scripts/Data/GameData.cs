using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mode", menuName = "Game Mode")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public GameMode Mode { set; get; }
    [field: SerializeField] public int Score { set; get; }
    [field: SerializeField] public int MaxCombo { set; get; }
    [field: SerializeField] public int FullFaceNum { set; get; }
    [field: SerializeField] public int MaxFaces { set; get; }
}
