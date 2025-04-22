using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScore : NetworkBehaviour
{
    [SerializeField] private List<Transform> listPos = new List<Transform> ();
    [SerializeField] private NetworkObject scorePrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventChanel.Register(KeyEvent.System , RPC_SpawnScore);
    }

    private void OnDestroy()
    {
        EventChanel.UnRegister(KeyEvent.System, RPC_SpawnScore);
    }

    private void RPC_SpawnScore(object[] value)
    {
        if(listPos.Count == 0) return;
        
        for(int i = 0; i < listPos.Count; i++)
        {
            Runner.Spawn(scorePrefabs, listPos[i].position, Quaternion.Euler(0, 0, 90));
        }
    }
}
