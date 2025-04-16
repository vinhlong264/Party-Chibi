using Fusion;
using UnityEngine;
public interface IWeapon
{
    void SetPostion(Transform pos, Vector3 rotation);
    NetworkObject GetNetWorkObject();
}
