using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        player.AnimTriggerTrue();
    }

    private void AttackEvent()
    {
        player.AttackHander();
    }
}
