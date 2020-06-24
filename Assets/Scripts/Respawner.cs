using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] SpawnPoint activeSpawnPoint;

    void SpawnPointActivated(SpawnPoint spawnPoint)
    {
        activeSpawnPoint = spawnPoint;
    }

    public void RespawnPlayer()
    {
        var player = Instantiate(playerPrefab, activeSpawnPoint.transform.position, activeSpawnPoint.transform.rotation);
        Camera.main.GetComponent<CameraController>().UpdatePlayerObject(player.transform);
    }
}
