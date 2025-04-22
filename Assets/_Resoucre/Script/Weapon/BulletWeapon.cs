using Fusion;
using System.Collections;
using UnityEngine;

public class BulletWeapon : WeaponBase
{   
    private Rigidbody rb;
    [SerializeField] private NetworkObject bullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(rb != null);
    }


    public override void ExcuteWeapon(Vector3 startPos , Vector3 dir)
    {
        StartCoroutine(ExcuteCroutine(startPos, dir));
    }

    IEnumerator ExcuteCroutine(Vector3 startPos, Vector3 dir)
    {
        Vector3 star = transform.localScale;
        Vector3 end = transform.localScale * 1.2f;
        transform.localScale = Vector3.Lerp(star, end , 0.25f);
        yield return new WaitForSeconds(1f);
        NetworkObject tmp = GameManager.instance.GetObjectToPools(bullet);
        if(tmp != null)
        {
            Debug.Log("Bullet");
            tmp.transform.position = startPos;
            tmp.transform.rotation = Quaternion.identity;
            tmp.GetComponent<Rigidbody>().AddForce(dir * 20f, ForceMode.Impulse);
        }

        transform.localScale = Vector3.Lerp(end, star, 0.25f);

        yield return new WaitForSeconds(1f);
        tmp.gameObject.SetActive(false);
    }
}
