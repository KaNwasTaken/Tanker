using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShell : Projectile
{

    public override void Initialized()
    {
        base.Initialized();
    }

    public override void ProjectileCollision()
    {
        base.ProjectileCollision();
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.ProjectileCollision();
        if (collision.gameObject.GetComponent<Damageable>())
        {
            collision.gameObject.GetComponent<Damageable>().RemoveHealth(weaponType.contactDamage);
        }
    }


}
