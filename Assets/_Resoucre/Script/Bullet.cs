using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDameHandler>().TakeDame(10);
            gameObject.SetActive(false);
        }
    }
}
