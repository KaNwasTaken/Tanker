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
        navMeshAgent.SetDestination(tankTransform.position);
    }

    private void OnDestroy()
    {
        EnemySpawner.currentEnemies -= 1;
    }
}
