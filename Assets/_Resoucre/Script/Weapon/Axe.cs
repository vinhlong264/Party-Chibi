using UnityEngine;

public class Axe : WeponBase
{
    public override void DameHandler(int dame)
    {
        foreach(var target in targetReceive)
        {
            if (target != null)
            {
                target.TakeDame(dame);
            }
        }
    }
}
