using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        collisionParticles.Play();
    }
    IEnumerator LifeTimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        TankShellPool.Instance.ReturnToPool(gameObject);
    }

    protected void SetProjectileState(bool isActive)
    {
        GetComponent<MeshRenderer>().enabled = isActive;
        GetComponent<TrailRenderer>().enabled = isActive;
        GetComponent<Collider>().enabled = isActive;
    }
}
