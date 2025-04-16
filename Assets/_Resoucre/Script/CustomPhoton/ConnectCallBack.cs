using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class ConnectCallBack : RunnerCallBackBase
{
    public override void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Kết nối tới sever thành công: " + runner.GameMode);
    }

    public override void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }
}
