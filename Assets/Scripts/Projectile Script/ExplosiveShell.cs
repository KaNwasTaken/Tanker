using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShell : Projectile
{
    public float radius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        base.ProjectileCollision();

        Collider[] results = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in results)
        {
            if (collider.GetComponent<Damageable>() != null)
            {
                collider.GetComponent<Damageable>().RemoveHealth(weaponType.contactDamage);
            }
        }
    }
}
