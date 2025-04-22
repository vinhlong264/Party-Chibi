using Fusion;
using UnityEngine;

public class UI_Manager : NetworkBehaviour
{
    private static UI_Manager instance;

    public static UI_Manager Instance { get => instance; }

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
        if(content_network != null)
        {
            Debug.Log("Gửi tin nhắn thành công");
            content_network.transform.SetParent(containerContent.transform);
            content_network.GetComponent<MesseageContent>().setUpMessage(nameCharacter, message);
        }
    }
}
