using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public float timeBetweenShots;
    public float projectileLifetime;
    public float projectileSpeed;
    public int contactDamage;
    public GameObject projectilePrefab;
    public Sprite weaponSprite;
}
