using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShell : Projectile
{

    private void OnCollisionEnter(Collision collision)
    {
        base.ProjectileCollision();
        if (collision.gameObject.GetComponent<Damageable>())
        {
            collision.gameObject.GetComponent<Damageable>().RemoveHealth(weaponType.contactDamage);
        }
    }


}
