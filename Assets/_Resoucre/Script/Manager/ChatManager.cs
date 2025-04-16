using Fusion;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    private static ChatManager instance;

    public static ChatManager Instance { get => instance; }

    [SerializeField] private NetworkObject content;
    [SerializeField] private Transform containerContent;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void SendChat(string message)
    {
        RPC_ReceiveChat(LocalData.name , message);
    }

    [Rpc(RpcSources.All , RpcTargets.All)]
    public void RPC_ReceiveChat(string nameCharacter , string message)
    {
        NetworkObject content_network = Runner.Spawn(content , Vector3.zero , Quaternion.identity);
        content_network.transform.parent = containerContent;


        content_network.GetComponent<MesseageContent>().setUpMessage(nameCharacter , message);
    }
}
