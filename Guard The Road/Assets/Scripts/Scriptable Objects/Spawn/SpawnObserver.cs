using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Spawn Observer")]
public class SpawnObserver : ScriptableObject
{
    public string agentName;
    public int nSpawned;
    public int spawnTarget;
    public bool isSpawned;

}
