using UnityEngine;
using System.Collections;

public class RetractingSpikes : MonoBehaviour
{
    public Transform spikes;
    public float retractDistance = 1.5f;
    public float retractSpeed = 6f;
    public float waitBeforeRising = 1f;

    private Vector3 raisedPosition;
    private Vector3 loweredPosition;
    private bool isRetracted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raisedPosition = spikes.position;

        loweredPosition = raisedPosition + Vector3.down * retractDistance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRetracted)
        {
            StartCoroutine(RetractSpikes());
        }
    }

    private IEnumerator RetractSpikes()
    {
        isRetracted = true;

        while (Vector3.Distance(spikes.position, loweredPosition) > 0.01f)
        {
            spikes.position = Vector3.MoveTowards(
                spikes.position,
                loweredPosition,
                retractSpeed * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeRising);

        while (Vector3.Distance(spikes.position, raisedPosition) > 0.01f)
        {
            spikes.position = Vector3.MoveTowards(
                spikes.position,
                raisedPosition,
                retractSpeed * Time.deltaTime
            );

            yield return null;
        }

        isRetracted = false;
    }
}
