using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : MonoBehaviour
{
    Transform tankTransform;
    NavMeshAgent navMeshAgent;

    void Start()
    {
        tankTransform = TankSingleton.Instance.TankBodyObject.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, tankTransform.position) > 6)
            navMeshAgent.SetDestination(tankTransform.position);
        else navMeshAgent.SetDestination(transform.position);
    }

    private void OnDestroy()
    {
        WaveManager.currentEnemies -= 1;
    }
}
