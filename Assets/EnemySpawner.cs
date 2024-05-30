using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject enemy;

    public static int currentEnemies = 0;
    int enemiesOnScreen = 10;
    public Vector3 GetRandomEdgePositionRaycast(Camera camera)
    {

        // Choose a random side of the screen (0 - left, 1 - right, 2 - top, 3 - bottom)
        int side = Random.Range(0, 4);

        // Get a random normalized point within the viewport on the chosen side
        Vector2 viewportPoint = new Vector2();
        switch (side)
        {
            case 0:
                viewportPoint.x = 0f;
                viewportPoint.y = Random.Range(0f, 1f);

                break;
            case 1:
                viewportPoint.x = 1f;
                viewportPoint.y = Random.Range(0f, 1f);

                break;
            case 2:
                viewportPoint.x = Random.Range(0f, 1f);
                viewportPoint.y = 1f;

                break;
            case 3:
                viewportPoint.x = Random.Range(0f, 1f);
                viewportPoint.y = 0f;

                break;
        }

        // Cast a ray from the camera viewport point
        Ray ray = camera.ViewportPointToRay(viewportPoint);

        // Ensure the raycast hits something
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void Start()
    {
        for (int i = 0; i < enemiesOnScreen; i++)
        {
            Instantiate(enemy, GetRandomEdgePositionRaycast(cam), Quaternion.identity);
        }
        currentEnemies = enemiesOnScreen;
    }

    private void Update()
    {
        if (currentEnemies <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(enemy, GetRandomEdgePositionRaycast(cam), Quaternion.identity);
            }
            currentEnemies = enemiesOnScreen;
        }
    }
}
