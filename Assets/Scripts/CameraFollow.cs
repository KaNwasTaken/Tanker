using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float heightAbove;
    [SerializeField] float distanceAway;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = (target.position + new Vector3(0, heightAbove, distanceAway));
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
