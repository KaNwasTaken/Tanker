using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public float timeBetweenShots;
    public GameObject projectilePrefab;
}
