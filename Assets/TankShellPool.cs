using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShellPool : MonoBehaviour
{
    public Queue<GameObject> shellPool = new();
    public static TankShellPool Instance;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddToPool(GameObject gameObject, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject instantiatedObj = Instantiate(gameObject);
            instantiatedObj.transform.parent = transform;
            instantiatedObj.SetActive(false);
            shellPool.Enqueue(instantiatedObj);

        }
    }

    public GameObject TakeFromPool(Vector3 position, Quaternion rotation)
    {
        if (shellPool.Count == 0)
        {
            Debug.LogError("No objects in pool!");
            return null;
        }

        GameObject shell = shellPool.Dequeue();
        shell.transform.SetPositionAndRotation(position, rotation);
        shell.SetActive(true);
        return shell;
    }

    public void ReturnToPool(GameObject shell)
    {
        shell.GetComponent<Rigidbody>().velocity = Vector3.zero;
        shell.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        shell.GetComponent<TrailRenderer>().Clear();
        shell.SetActive(false);
        shellPool.Enqueue(shell);
    }
}
