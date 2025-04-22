using Fusion;
using UnityEngine;

public class PlayerStats : NetworkBehaviour, IDameHandler
{
    [Networked]
    public float currentHealth { get; set; }

    private int maxHealth;
    private float timeStunDuration;

    [Networked]
    public int score { get; set;}

    [Networked]
    public string namePlayer { get; set; }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            maxHealth = 100;
            currentHealth = maxHealth;
            EventChanel.NotifyEvent(KeyEvent.HealthBar, new object[] { (float)maxHealth, (float)currentHealth });
            namePlayer = LocalData.name;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Score"))
        {
            if (HasInputAuthority)
            {
                RPC_ScoreCollect();
            }
            Runner.Despawn(other.GetComponent<NetworkObject>());
        }
    }

    public void TakeDame(int dame)
    {
        if (Object.HasStateAuthority)
        {
            RPC_TakeDame(dame);
        }
    }

    public void TakeStun(float timeStun)
    {
        timeStunDuration = timeStun;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_ScoreCollect()
    {
        Debug.Log("Score đucợ ăn bởi: " + Runner.LocalPlayer.PlayerId);
        score++;
        GameManager.instance.RPC_updateHighScore();
    }




    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeDame(int dame)
    {
        currentHealth -= dame;
        EventChanel.NotifyEvent(KeyEvent.HealthBar, new object[] { (float)maxHealth, (float)currentHealth });
        Debug.Log("HP: " + currentHealth);
        if (currentHealth <= 0)
        {
            EventChanel.NotifyEvent(KeyEvent.Status, null);
            foreach (var p in Runner.ActivePlayers)
            {
                Debug.Log("Player ID: " + p.PlayerId);

                if (Runner.TryGetPlayerObject(p, out NetworkObject getObject))
                {
                    Debug.Log("Call back");
                    PlayerStats player = getObject.GetComponent<PlayerStats>();
                    if (player != null && player.currentHealth > 0)
                    {
                        CameraFollow follow = FindFirstObjectByType<CameraFollow>();
                        if (follow != null)
                        {
                            follow.SetUpCamera(player.transform);
                        }
                    }
                }
                else
                {
                    Debug.Log("NetworkObject not found for Player ID: " + p.PlayerId);
                }
            }
            Runner.Despawn(this.GetComponent<NetworkObject>());
        }
    }
}
