using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLightDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public KeyCode dashKey = KeyCode.E;
    public float dashSpeed = 15f;
    public float activationDistance = 5f; // how close player must be to start dash

    private bool isDashing = false;
    private List<Vector3> pathPoints;
    private int currentIndex = 0;

    private CollectiblePathSpawner spawner;

    void Start()
    {
        // Grab spawner in scene
        spawner = Object.FindFirstObjectByType<CollectiblePathSpawner>();
        if (spawner != null)
        {
            pathPoints = spawner.pathPoints;
            Debug.Log($"[LightDash] Path initialized with {pathPoints.Count} points");
        }
        else
        {
            Debug.LogError("[LightDash] No CollectiblePathSpawner found in scene!");
        }
    }

    void Update()
    {
        if (!isDashing && Input.GetKeyDown(dashKey))
        {
            if (spawner != null && pathPoints != null && pathPoints.Count > 0)
            {
                float dist = Vector3.Distance(transform.position, pathPoints[0]);
                Debug.Log($"[LightDash] Player pressed dash, distance to first point = {dist}");

                if (dist < activationDistance)
                {
                    Debug.Log("[LightDash] Starting dash!");
                    StartCoroutine(DashAlongPath(pathPoints));
                }
                else
                {
                    Debug.Log("[LightDash] Too far from path start.");
                }
            }
        }
    }

    IEnumerator DashAlongPath(List<Vector3> points)
    {
        isDashing = true;

        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("[LightDash] Disabled PlayerController during dash");
        }

        currentIndex = 0;

        while (currentIndex < points.Count)
        {
            Vector3 target = points[currentIndex];

            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);
                yield return null;
            }

            currentIndex++;
            yield return null;
        }

        // Re-enable normal movement
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("[LightDash] Re-enabled PlayerController after dash");
        }

        isDashing = false;
        Debug.Log("[LightDash] Dash complete!");
    }
}