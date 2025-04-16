using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameManager instance;
    private int playerRequire = 2;
    public float startTime;
    private Text timeTxt;
    [SerializeField] private Transform[] pointArray;
    [SerializeField] private NetworkObject weapon;

    [Networked]
    public NetworkBool gameStart { get; set; }
    public NetworkBool CanSpawn { get; set; }


    //Object Pooling
    private Dictionary<NetworkObject, List<NetworkObject>> pools = new Dictionary<NetworkObject, List<NetworkObject>>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        timeTxt = GameObject.Find("TimeStar").GetComponent<Text>();
        timeTxt.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public override void Spawned()
    {
        Runner.AddCallbacks(this);
    }

    public NetworkObject GetObjectToPools(NetworkObject key)
    {
        List<NetworkObject> tmp_pools = new List<NetworkObject>();

        if (pools.ContainsKey(key))
        {
            tmp_pools = pools[key];
        }
        else
        {
            pools.Add(key, tmp_pools);
        }

        foreach(var g in tmp_pools)
        {
            if (g.gameObject.activeSelf)
            {
                continue;
            }

            g.gameObject.SetActive(true);
            return g;
        }

        NetworkObject tmp_networkObject = Runner.Spawn(key);
        tmp_pools.Add(tmp_networkObject);
        tmp_networkObject.gameObject.SetActive(true);
        return tmp_networkObject;
    }


    private void CheckGameStart()
    {
        if (Runner.ActivePlayers.Count() >= playerRequire && !CanSpawn)
        {
            StartCoroutine(TimeStartGame());
        }
    }

    IEnumerator TimeStartGame()
    {
        startTime = 4;
        timeTxt.gameObject.SetActive(true);
        while(startTime > 0)
        {
            startTime -= 1f;
            timeTxt.text = $"{(int)startTime}";
            yield return new WaitForSeconds(1f);
        }
        gameStart = true;   
        timeTxt.gameObject.SetActive(false);

        RPC_SysnGame();
    }

    [Rpc(RpcSources.StateAuthority , RpcTargets.All)]
    public void RPC_SysnGame()
    {
        if (!Runner.IsSharedModeMasterClient) return;

        for (int i = 0; i < pointArray.Length; i++)
        {
            NetworkObject tmp = GetObjectToPools(weapon);
            if (tmp == null)
            {
                tmp.transform.position = pointArray[i].position;
                tmp.transform.rotation = Quaternion.identity;
            }
        }
        CanSpawn = true;
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Có Player tham gia phòng");
        CheckGameStart();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    #region INetworkRunnerCallbacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }


    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
      
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
      
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
      
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
       
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
  
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
      
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
      
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
      
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    
    }

    #endregion
}
