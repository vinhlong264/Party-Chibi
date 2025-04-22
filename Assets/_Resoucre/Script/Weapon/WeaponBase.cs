using Fusion;
using UnityEngine;

public abstract class WeaponBase : NetworkBehaviour, IWeapon
{
    [SerializeField] protected int dameWeapon;
    [SerializeField] protected float timeStun;
    [SerializeField] protected float weightWeapon;
    private DameWeaponHandler handler;

    private NetworkBool isPicked;
    public NetworkBool IsPicked { get => isPicked; }

    public DameWeaponHandler Handler { get => handler; }

    public override void Spawned()
    {
        isPicked = false;
    }

    private void Start()
    {
        handler = GetComponentInChildren<DameWeaponHandler>();
    }

    public virtual void ExcuteWeapon(Vector3 startPos, Vector3 dir)
    {

    }

    public void SetPostion(Transform pos, Vector3 rotation)
    {
        this.transform.localRotation = Quaternion.Euler(rotation);
        this.transform.parent = pos;
        this.transform.position = pos.position;

        isPicked = true;
    }

    public NetworkObject GetNetWorkObject()
    {
        return this.Object;
    }
}
