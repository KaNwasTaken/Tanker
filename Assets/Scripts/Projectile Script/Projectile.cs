using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(TrailRenderer))]
public abstract class Projectile : MonoBehaviour
{
    protected float lifetime;
    protected bool reachedPosition;
    protected float speed;
    protected float currentLifetime = 0;
    [SerializeField] protected WeaponType weaponType;
    protected ParticleSystem collisionParticles;

    public virtual void Initialized()
    {
        lifetime = weaponType.projectileLifetime;
        speed = weaponType.projectileSpeed;
        collisionParticles = GetComponent<ParticleSystem>();
        SetProjectileState(true);

        StartCoroutine(LifeTimeCoroutine(lifetime));
    }
    public virtual void ProjectileCollision()
    {
        SetProjectileState(false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        collisionParticles.Play();
    }
    IEnumerator LifeTimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        SetProjectileState(false);
        if (TankSingleton.Instance.TankTurretObject.GetComponent<TankWeaponSystem>().currentWeapon == weaponType)
        {
            SetProjectileState(false);
            gameObject.SetActive(false);
            TankShellPool.Instance.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    protected void SetProjectileState(bool isActive)
    {
        GetComponent<MeshRenderer>().enabled = isActive;
        GetComponent<TrailRenderer>().enabled = isActive;
        GetComponent<Collider>().enabled = isActive;

        if (!isActive)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
