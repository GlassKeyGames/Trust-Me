using UnityEngine;
using System.Collections;

public class CatchingWall : MonoBehaviour
{
    public Transform catchingWall;
    public Transform targetPosition;

    public float moveSpeed = 8f;
    public float waitBeforeReset = 1.5f;

    private Vector3 startPosition;

    private bool hasActivated = false;

    void Start()
    {
        startPosition = catchingWall.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            hasActivated = true;
            StartCoroutine(MoveWall());
        }
    }

    private IEnumerator MoveWall()
    {
        while (Vector3.Distance(
            catchingWall.position,
            targetPosition.position
        ) > 0.01f)
        {
            catchingWall.position = Vector3.MoveTowards(
                catchingWall.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeReset);

        while (Vector3.Distance(
            catchingWall.position,
            startPosition
        ) > 0.01f)
        {
            catchingWall.position = Vector3.MoveTowards(
                catchingWall.position,
                startPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        hasActivated = false;
    }
}
