using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameManager instance;
    private int playerRequire = 2;
    private TickTimer timer;

    [Networked, OnChangedRender(nameof(SysnGameTimerStart))]
    public float gameTimer { get; set; }

    [Networked,OnChangedRender(nameof(RPC_SysnTimePlaying))]
    public float gameTimePlay { get; set; }

    private Text timeTxt;
    private TextMeshProUGUI gameTimerTxt;

    [SerializeField] private Transform[] pointArray;
    [SerializeField] private NetworkObject weapon;

    [Networked, OnChangedRender(nameof(SysnGameTimerStart))]
    public GameState gameState { get; set; }
    public NetworkBool CanSpawnWeapon { get; set; }

    //Object Pooling
    private Dictionary<NetworkObject, List<NetworkObject>> pools = new Dictionary<NetworkObject, List<NetworkObject>>();

    public ResultGame result;

    //HighScore

    public NetworkObject content;
    public Transform parent;


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
        gameTimerTxt = GameObject.Find("TimeGame").GetComponent<TextMeshProUGUI>();


        timeTxt.gameObject.SetActive(false);

        result = ResultGame.None;
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
        if (HasStateAuthority)
        {
            timer = TickTimer.None;
            gameTimer = 0;
            gameState = GameState.WaitPlayers;
        }
    }

    public override void FixedUpdateNetwork()
    {
        CheckGameStart();

        if(gameState == GameState.Playing)
        {
            RPC_SysnTimePlaying();
        }
    }

    private void CheckGameStart()
    {
        if (gameState == GameState.Playing) return;

        if (Runner.ActivePlayers.Count() >= playerRequire && !CanSpawnWeapon)
        {
            gameState = GameState.Starting;
            gameTimer += Runner.DeltaTime;

            if (gameTimer >= 15)
            {
                gameState = GameState.Playing;
                gameTimer = 0;
                RPC_SysnGame();
                EventChanel.NotifyEvent(KeyEvent.System, null);
            }
        }
    }




    private void SysnGameTimerStart()
    {
        if (gameState == GameState.Starting)
        {
            timeTxt.gameObject.SetActive(true);
            timeTxt.text = gameTimer.ToString("F2");
        }
        else if (gameState == GameState.Playing)
        {
            timeTxt.gameObject.SetActive(false);
        }
    }

    [Rpc(RpcSources.All , RpcTargets.All)]
    public void RPC_SysnTimePlaying()
    {
        gameTimePlay += Runner.DeltaTime;
        int minues = Mathf.FloorToInt(gameTimePlay / 60);
        int second = Mathf.FloorToInt(gameTimePlay % 60);

        gameTimerTxt.text = $"{minues} : {second}";
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
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
        CanSpawnWeapon = true;
    }

    //Object Pooling
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

        foreach (var g in tmp_pools)
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


    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_updateHighScore()
    {
        List<PlayerStats> stats = new List<PlayerStats>();
        foreach(var p in Runner.ActivePlayers)
        {
            if(Runner.TryGetPlayerObject(p , out var objectPlayer))
            {
                PlayerStats stat = objectPlayer.GetComponent<PlayerStats>();
                if(stat != null)
                {
                    stats.Add(stat);
                }
            }
        }

        var getData = stats.OrderByDescending(x => x.score).ToArray();

        for(int i = 0; i < getData.Length; i++)
        {
            NetworkObject tmp = Runner.Spawn(content , parent.transform.position);
            tmp.transform.SetParent(parent.transform);

            Text[] txt = tmp.GetComponentsInChildren<Text>();
            txt[0].text = i.ToString();
            txt[1].text = getData[i].name;
            txt[2].text = getData[i].score.ToString();
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Có Player tham gia phòng");
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

public enum GameState { WaitPlayers, Starting, Playing }

public enum ResultGame { None,Win, Lose}
