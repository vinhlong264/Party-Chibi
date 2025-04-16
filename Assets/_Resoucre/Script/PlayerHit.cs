using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class PlayerHit : NetworkBehaviour, IDameHandler
{
    [Networked , OnChangedRender(nameof(UpdateHealth))]
    public int HP { get; set; }
    [SerializeField] private Slider healthBar;
    public override void Spawned()
    {
        healthBar.value = HP;
    }

    public void UpdateHealth()
    {
        healthBar.value = HP;
    }

    [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
    public void RPC_TakeDame(int dame)
    {
        HP -= dame;
        Debug.Log("Dame: " + dame);
        if(HP <= 0)
        {
            Runner.Despawn(this.GetComponent<NetworkObject>());
        }

    }

    
    public void TakeDame(int dame)
    {
        RPC_TakeDame(dame);
    }

    public void TakeStun(float timeStun)
    {
        
    }
}
