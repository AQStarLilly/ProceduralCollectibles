using UnityEngine;
using System.Collections.Generic;


public class CollectiblePathSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public Transform platform1;
    public Transform platform2;
    public int collectibleCount = 15;

    [Header("Wave Settings")]
    public float waveAmplitude;
    public float waveFrequency;
    public float verticalAmplitude;
    public float heightOffset = 1f;

    [HideInInspector] public List<Vector3> pathPoints = new List<Vector3>();

    void Start()
    {
        waveAmplitude = Random.Range(0.5f, 3f);
        waveFrequency = Random.Range(0.5f, 3f);
        verticalAmplitude = Random.Range(0.5f, 3f);

        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        pathPoints.Clear();

        Vector3 start = platform1.position;
        Vector3 end = platform2.position;

        Vector3 forward = (end - start).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 up = Vector3.up;

        float platformTopY = Mathf.Max(platform1.position.y, platform2.position.y);

        for (int i = 0; i < collectibleCount; i++)
        {
            float t = (float)i / (collectibleCount - 1);
        
            Vector3 basePos = Vector3.Lerp(start, end, t);

            float baseY = Mathf.Lerp(platform1.position.y, platform2.position.y, t);
            basePos.y = baseY + heightOffset;

            float waveOffset = Mathf.Sin(t * Mathf.PI * waveFrequency) * waveAmplitude;
            float verticalOffset = Mathf.Cos(t * Mathf.PI * waveFrequency) * verticalAmplitude;

            Vector3 pos = basePos + right * waveOffset + up * verticalOffset;

            float currentPlatformY = Mathf.Lerp(platform1.position.y, platform2.position.y, t);
            pos.y = Mathf.Max(pos.y, currentPlatformY + heightOffset);

            pathPoints.Add(pos);
            Debug.Log($"Generated path with {pathPoints.Count} points");

            Instantiate(collectiblePrefab, pos, Quaternion.identity);
        }
    }
}
