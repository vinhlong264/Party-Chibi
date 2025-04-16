using Fusion;
using System.Collections;
using UnityEngine;

public class RoomManager : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefabs;
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

        NetworkObject tmp = Runner.Spawn(playerPrefabs, new Vector3(Random.Range(-5f , 20f), -0.5f, -7f), Quaternion.identity, Runner.LocalPlayer, (Runner, Object) =>
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
