using Fusion;
using UnityEngine;

public class GameManager : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefabs;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefabs, new Vector3(0,0,0), Quaternion.identity, Runner.LocalPlayer); 
        }
    }
}
