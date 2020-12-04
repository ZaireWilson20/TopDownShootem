using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake() => PlayerSpawn.AddSpawnPoint(transform);
    private void OnDestroy() => PlayerSpawn.RemoveSpawnPoint(transform);
}
