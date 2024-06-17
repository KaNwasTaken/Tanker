using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRammer : MonoBehaviour
{
    Rigidbody tankRb;
    Collider ramZone;

    public int ramDamage;
    public int thresholdVelocity;

    private void Start()
    {
        tankRb = transform.parent.GetComponent<Rigidbody>();
        ramZone = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        float currentVel = transform.InverseTransformDirection(tankRb.velocity).z;
        if (currentVel > thresholdVelocity)
        {
            if (other.gameObject.GetComponent<Damageable>())
            {
                other.gameObject.GetComponent<Damageable>().RemoveHealth(ramDamage);
            }
        }
        else
        {
            Debug.Log("not enough speed");
            Debug.Log(currentVel);
        }
    }


}
