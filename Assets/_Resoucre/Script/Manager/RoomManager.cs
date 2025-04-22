using Fusion;
using System.Collections;
using UnityEngine;

public class RoomManager : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefabs;
    private Vector3 spawnPos;
    public void PlayerJoined(PlayerRef player)
    {

        if (player == Runner.LocalPlayer)
        {
            StartCoroutine(SpawnPlayer(player));
        }
    }

    IEnumerator SpawnPlayer(PlayerRef player)
    {
        yield return new WaitUntil(() => InterfaceManager.instance != null);

        yield return new WaitUntil(() => GameManager.instance != null);

        yield return new WaitUntil(() => GameManager.instance.gameState != GameState.Playing);

        spawnPos = GameObject.Find("PosPlayer").transform.position;

        yield return new WaitUntil(() => spawnPos != Vector3.zero);

        NetworkObject tmp = Runner.Spawn(playerPrefabs, spawnPos + new Vector3(Random.Range(-1f, 1f),0, Random.Range(-1f, 1f)), Quaternion.identity, Runner.LocalPlayer, (Runner, Object) =>
        {
            Player player = Object.GetComponent<Player>();
            if (player != null)
            {
                player.SetUpCamera();
            }
        });

        Runner.SetPlayerObject(player, tmp);
        Debug.Log($"Spawned and assigned NetworkObject for Player ID: {player.PlayerId}");
    }
}
