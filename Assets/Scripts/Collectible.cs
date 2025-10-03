using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Animation Settings")]
    public float spinSpeed = 90f;
    public float hoverSpeed = 0.5f;
    public float hoverHeight = 0.25f;

    [Header("Respawn Settings")]
    public float respawnTime = 3f;

    private Vector3 startPos;
    private bool collected = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!collected)
        {
            // Spin
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

            // Hover
            float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed * Mathf.PI * 2) * hoverHeight;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            Collect();
        }
    }

    void Collect()
    {
        collected = true;
        gameObject.GetComponent<Renderer>().enabled = false; // hide visually
        GetComponent<Collider>().enabled = false; // disable pickup
        StartCoroutine(RespawnAfterDelay());
    }

    System.Collections.IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);

        // Reset
        collected = false;
        transform.position = startPos;
        gameObject.GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
