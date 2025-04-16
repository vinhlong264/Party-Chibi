using Fusion;
using UnityEngine;
public class DameWeaponHandler : NetworkBehaviour
{
    [SerializeField] private Collider _collider;

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }

    public void DeactiveCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDameHandler>().TakeDame(10);
            DeactiveCollider();
        }
    }
}
