using UnityEngine;
using System.Collections.Generic;

public abstract class WeponBase : MonoBehaviour, IWeapon
{
    protected List<IDameHandler> targetReceive = new List<IDameHandler>();
    [SerializeField] protected int dameWeapon;
    [SerializeField] protected float timeStun;
    [SerializeField] protected float weightWeapon;
    [SerializeField] protected Vector3 customRotation;

    public void SetPostion(Transform pos)
    {
        this.transform.SetParent(pos, true);
        this.transform.position = pos.position; 
        this.transform.rotation = Quaternion.Euler(customRotation);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            targetReceive.Add(other.GetComponent<IDameHandler>());
        }
    }

    public abstract void DameHandler(int dame);
}
