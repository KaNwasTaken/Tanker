using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSingleton : MonoBehaviour
{
    public static TankSingleton Instance;
    public GameObject TankTurretObject { get; private set; }
    public GameObject TankBodyObject { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        TankTurretObject = gameObject;
        TankBodyObject = gameObject.transform.parent.gameObject;
    }

}
