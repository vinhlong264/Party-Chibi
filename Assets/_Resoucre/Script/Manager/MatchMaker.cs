using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MatchMaker : MonoBehaviour, INetworkRunnerCallbacks
{
    public static MatchMaker instance;

    public NetworkRunner runnerPrefabs;
    public NetworkObject gameManager;
    public NetworkRunner Runner { get; private set; }

    private string roomGame = string.Empty;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
    public void setRoomName(string roomCode)
    {
        roomGame = roomCode;
    }

    public void TryConnectSever()
    {
        ConnectSever(string.IsNullOrWhiteSpace(roomGame) ? $"22222" : roomGame);
    }

    private void ConnectSever(string roomCode)
    {
        StartCoroutine(ConnectCroutine(roomCode));
    }

    IEnumerator ConnectCroutine(string roomCode)
    {
        if(Runner != null)
        {
            Runner.Shutdown();
        }

        Runner = Instantiate(runnerPrefabs);
        NetworkEvents networkEvents = Runner.GetComponent<NetworkEvents>();

        void SpawnManager(NetworkRunner runner)
        {
            if (runner.IsSharedModeMasterClient)
            {
                runner.Spawn(gameManager);
            }
            networkEvents.OnSceneLoadDone.RemoveListener(SpawnManager);
        }

        networkEvents.OnSceneLoadDone.AddListener(SpawnManager);
        Runner.AddCallbacks(this);

        Task<StartGameResult> task = Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = roomCode,
            SceneManager = Runner.GetComponent<INetworkSceneManager>(),
            ObjectProvider = Runner.GetComponent<INetworkObjectProvider>()
        });

        while (!task.IsCompleted) yield return null;

        StartGameResult result = task.Result;

        if(result != null)
        {
            if (result.Ok)
            {
                if (Runner.IsSharedModeMasterClient)
                {
                    Runner.LoadScene("Game");
                }
            }
            else
            {
                Debug.Log(result.ShutdownReason);
            }
        }
    }


    #region INetworkRunnerCallbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
       
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
       
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
       
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
       
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    #endregion
}
